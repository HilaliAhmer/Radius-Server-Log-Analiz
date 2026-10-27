using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Radius_Log_Browser
{
    public partial class Form1 : Form
    {
        private long currentPosition;

        // watcher debounce
        private readonly System.Windows.Forms.Timer _debounceTimer = new System.Windows.Forms.Timer();
        private bool _pendingChange = false;

        // filter debounce
        private readonly System.Windows.Forms.Timer _filterTimer = new System.Windows.Forms.Timer();
        private CancellationTokenSource _filterCts;

        private readonly Dictionary<string, RadiusRequest> requests = new Dictionary<string, RadiusRequest>();

        // Right click search (mevcut legacy)
        private readonly Dictionary<int, ListViewItem> oldItems = new Dictionary<int, ListViewItem>();
        private readonly Dictionary<int, ListViewItem> newItems = new Dictionary<int, ListViewItem>();
        private string search;
        private int searchKey;

        private ListViewColumnSorter lvwColumnSorter;

        private readonly FileSystemWatcher watcher = new FileSystemWatcher();

        private string Lines;
        private string Directory;
        private string FileName;

        // ✅ Master cache: ListViewItem yerine ham satır modeli (çok hızlı)
        private sealed class LogRow
        {
            public string[] Cols;          // 0..8
            public DateTime Ts;            // Timestamp parse edilmiş
            public bool HasTs;
            public string SearchBlobLower; // tüm kolonların lower birleşimi
            public Color BackColor;
        }

        private readonly List<LogRow> _rows = new List<LogRow>(capacity: 4096);

        public Form1()
        {
            InitializeComponent();

            // --- Filter UI format ---
            dtFrom.Enabled = chkFrom.Checked;
            dtTo.Enabled = chkTo.Checked;

            dtFrom.Format = DateTimePickerFormat.Custom;
            dtFrom.CustomFormat = "dd.MM.yyyy HH:mm:ss";
            dtFrom.ShowUpDown = true;

            dtTo.Format = DateTimePickerFormat.Custom;
            dtTo.CustomFormat = "dd.MM.yyyy HH:mm:ss";
            dtTo.ShowUpDown = true;

            // --- Filter debounce ---
            _filterTimer.Interval = 350;
            _filterTimer.Tick += (s, e) =>
            {
                _filterTimer.Stop();
                _ = ApplyFilterAsync(); // async background filter
            };

            // --- Watcher debounce ---
            _debounceTimer.Interval = 500;
            _debounceTimer.Tick += (s, e) =>
            {
                _debounceTimer.Stop();
                if (!_pendingChange) return;
                _pendingChange = false;

                if (string.IsNullOrEmpty(ofdFile.FileName) || !File.Exists(ofdFile.FileName))
                    return;

                ReadLines(ofdFile.FileName);
                Task.Run(() => procesLogFile(false));
            };

            // --- Sorting ---
            lvwColumnSorter = new ListViewColumnSorter();
            lvLogTable.ListViewItemSorter = lvwColumnSorter;

            // Columns: sadece 1 kere
            if (lvLogTable.Columns.Count == 0)
            {
                lvLogTable.Columns.Add("Timestamp");
                lvLogTable.Columns.Add("Type");
                lvLogTable.Columns.Add("NAP Server");
                lvLogTable.Columns.Add("Access Point IP");
                lvLogTable.Columns.Add("Access Point Name");
                lvLogTable.Columns.Add("MAC-address requester");
                lvLogTable.Columns.Add("SAM-Account-Name");
                lvLogTable.Columns.Add("Responce Type");
                lvLogTable.Columns.Add("Reason Code");

                lvLogTable.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
        }

        // ---------------------------
        // Core: read + parse
        // ---------------------------

        private XDocument SafeParseXml(string xmlChunk)
        {
            try
            {
                return XDocument.Parse(xmlChunk);
            }
            catch
            {
                try
                {
                    return XDocument.Parse("<Events>" + xmlChunk + "</Events>");
                }
                catch
                {
                    return new XDocument(new XElement("Events"));
                }
            }
        }

        public void ReadLines(string path)
        {
            using (FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                stream.Seek(currentPosition, SeekOrigin.Begin);
                using (StreamReader reader = new StreamReader(stream))
                {
                    Lines = reader.ReadToEnd();
                    currentPosition = stream.Position;
                }
            }
        }

        private static bool TryParseTs(string s, out DateTime ts)
        {
            // Logların formatı bazen: "02/25/2026 13:57:55.728"
            // bazen TR formatı vs olabiliyor; o yüzden önce invariant, sonra current culture.
            if (DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out ts))
                return true;

            return DateTime.TryParse(s, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out ts);
        }

        private static string BuildSearchBlobLower(string[] cols)
        {
            // Daha hızlı arama için tek string oluşturuyoruz
            // (Contains yerine IndexOf yapacağız)
            var sb = new StringBuilder(256);
            for (int i = 0; i < cols.Length; i++)
            {
                if (!string.IsNullOrEmpty(cols[i]))
                {
                    sb.Append(cols[i]);
                    sb.Append('\u001F'); // ayraç (rare char)
                }
            }
            return sb.ToString().ToLowerInvariant();
        }

        private void procesLogFile(bool initalrun = false)
        {
            XDocument doc = SafeParseXml(Lines);

            // yeni gelen satırlar
            var newRows = new List<LogRow>(512);
            var newLviDirect = new List<ListViewItem>(512);

            foreach (var ev in doc.Descendants("Event"))
            {
                XElement element = ev.Element("Class");
                if (element == null) continue;

                string classID = element.Value;

                if (requests.ContainsKey(classID))
                {
                    requests[classID].setResponce(ev);

                    string[] cols = new string[]
                    {
                        requests[classID].timestamp,
                        requests[classID].getRequestType(),
                        requests[classID].server,
                        requests[classID].accessPointIP,
                        requests[classID].accessPointName,
                        requests[classID].requesterMacAddress,
                        requests[classID].samAccountName,
                        requests[classID].getResponceType(),
                        requests[classID].getReason()
                    };

                    var row = new LogRow();
                    row.Cols = cols;
                    row.BackColor = requests[classID].getRowColor();

                    if (TryParseTs(cols[0], out var ts))
                    {
                        row.Ts = ts;
                        row.HasTs = true;
                    }
                    else
                    {
                        row.HasTs = false;
                    }

                    row.SearchBlobLower = BuildSearchBlobLower(cols);

                    newRows.Add(row);

                    // filtre aktif değilse direkt UI’ya basmak için ListViewItem hazırla
                    var lvi = new ListViewItem(cols);
                    lvi.BackColor = row.BackColor;
                    newLviDirect.Add(lvi);
                }
                else
                {
                    requests[classID] = new RadiusRequest(ev);
                }
            }

            this.Invoke(new MethodInvoker(delegate
            {
                // master cache ekle
                _rows.AddRange(newRows);

                bool filterActive = !string.IsNullOrWhiteSpace(txtSearch.Text)
                                    || chkFrom.Checked
                                    || chkTo.Checked;

                if (filterActive)
                {
                    TriggerFilter(); // debounce ile async filter
                }
                else
                {
                    lvLogTable.BeginUpdate();
                    lvLogTable.Items.AddRange(newLviDirect.ToArray());
                    lvLogTable.EndUpdate();

                    if (cbScroll.Checked && lvLogTable.Items.Count > 0)
                        lvLogTable.Items[lvLogTable.Items.Count - 1].EnsureVisible();
                }

                if (initalrun)
                {
                    lvLogTable.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                    lvLogTable.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                    RunWatcher();
                }

                watcher.EnableRaisingEvents = true;
            }));
        }

        // ---------------------------
        // Filtering (FAST)
        // ---------------------------

        private void TriggerFilter()
        {
            _filterTimer.Stop();
            _filterTimer.Start();
        }

        private async Task ApplyFilterAsync()
        {
            // önce önceki filtreyi iptal et
            _filterCts?.Cancel();
            _filterCts?.Dispose();
            _filterCts = new CancellationTokenSource();
            var token = _filterCts.Token;

            // UI değerlerini snapshot al (thread-safe)
            string q = (txtSearch.Text ?? "").Trim();
            string qLower = q.ToLowerInvariant();
            bool hasQ = qLower.Length > 0;

            DateTime? from = chkFrom.Checked ? (DateTime?)dtFrom.Value : null;
            DateTime? to = chkTo.Checked ? (DateTime?)dtTo.Value : null;

            // Filtreyi background’da çalıştır
            List<ListViewItem> result = null;

            try
            {
                result = await Task.Run(() =>
                {
                    var list = new List<ListViewItem>(Math.Min(_rows.Count, 5000));

                    for (int idx = 0; idx < _rows.Count; idx++)
                    {
                        token.ThrowIfCancellationRequested();

                        var row = _rows[idx];

                        // Date filter
                        if (from.HasValue || to.HasValue)
                        {
                            if (!row.HasTs) continue;

                            if (from.HasValue && row.Ts < from.Value) continue;
                            if (to.HasValue && row.Ts > to.Value) continue;
                        }

                        // Text filter (tek blob içinde)
                        if (hasQ)
                        {
                            if (row.SearchBlobLower.IndexOf(qLower, StringComparison.Ordinal) < 0)
                                continue;
                        }

                        var lvi = new ListViewItem(row.Cols);
                        lvi.BackColor = row.BackColor;
                        list.Add(lvi);
                    }

                    return list;
                }, token);
            }
            catch (OperationCanceledException)
            {
                return; // yeni filtre geldi, bunu bırak
            }

            if (IsDisposed) return;

            // UI update
            try
            {
                BeginInvoke(new MethodInvoker(() =>
                {
                    lvLogTable.BeginUpdate();
                    lvLogTable.Items.Clear();
                    if (result != null && result.Count > 0)
                        lvLogTable.Items.AddRange(result.ToArray());
                    lvLogTable.EndUpdate();

                    if (cbScroll.Checked && lvLogTable.Items.Count > 0)
                        lvLogTable.Items[lvLogTable.Items.Count - 1].EnsureVisible();
                }));
            }
            catch
            {
                // form kapanırken vs
            }
        }

        private void ClearFilterUI()
        {
            txtSearch.Text = "";
            chkFrom.Checked = false;
            chkTo.Checked = false;
            dtFrom.Enabled = false;
            dtTo.Enabled = false;

            // Filtreyi kapatınca tüm satırları göstermek: hızlıca rebuild
            var list = new List<ListViewItem>(Math.Min(_rows.Count, 5000));
            foreach (var row in _rows)
            {
                var lvi = new ListViewItem(row.Cols);
                lvi.BackColor = row.BackColor;
                list.Add(lvi);
            }

            lvLogTable.BeginUpdate();
            lvLogTable.Items.Clear();
            if (list.Count > 0)
                lvLogTable.Items.AddRange(list.ToArray());
            lvLogTable.EndUpdate();
        }

        // ---------------------------
        // Watcher
        // ---------------------------

        public void RunWatcher()
        {
            watcher.EnableRaisingEvents = false;

            watcher.Path = Directory;
            watcher.Filter = FileName;
            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size;

            watcher.Changed -= OnChanged;
            watcher.Changed += OnChanged;

            watcher.EnableRaisingEvents = true;
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            watcher.EnableRaisingEvents = false;

            _pendingChange = true;
            _debounceTimer.Stop();
            _debounceTimer.Start();
        }

        private void ReloadCurrentFile(bool resetWatcher = true)
        {
            if (string.IsNullOrEmpty(ofdFile.FileName) || !File.Exists(ofdFile.FileName))
            {
                MessageBox.Show("Önce bir log dosyası seçmelisin.");
                return;
            }

            if (resetWatcher) watcher.EnableRaisingEvents = false;

            _rows.Clear();
            requests.Clear();
            oldItems.Clear();
            newItems.Clear();

            lvLogTable.Items.Clear();
            currentPosition = 0;

            Directory = Path.GetDirectoryName(ofdFile.FileName);
            FileName = Path.GetFileName(ofdFile.FileName);

            ReadLines(ofdFile.FileName);
            Task.Run(() => procesLogFile(true));
        }

        // ============================================================
        // ============ DESIGNER’IN BEKLEDİĞİ HANDLER’LAR =============
        // ============================================================

        private void selectFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ofdFile.ShowDialog() == DialogResult.OK)
            {
                lvLogTable.Items.Clear();
                currentPosition = 0;

                Directory = Path.GetDirectoryName(ofdFile.FileName);
                FileName = Path.GetFileName(ofdFile.FileName);

                _rows.Clear();
                requests.Clear();
                oldItems.Clear();
                newItems.Clear();

                ReadLines(ofdFile.FileName);
                Task.Run(() => procesLogFile(true));
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReloadCurrentFile();
        }

        private void cbScroll_Click(object sender, EventArgs e)
        {
            cbScroll.Checked = !cbScroll.Checked;
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void howToUseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            help helpdialog = new help();
            helpdialog.FormBorderStyle = FormBorderStyle.FixedSingle;
            helpdialog.ShowDialog();
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbook|*xlsx", ValidateNames = true })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                    Workbook wb = app.Workbooks.Add(XlSheetType.xlWorksheet);
                    Worksheet ws = (Worksheet)app.ActiveSheet;
                    app.Visible = false;

                    ws.Cells[1, 1] = "Timestamp";
                    ws.Cells[1, 2] = "Type";
                    ws.Cells[1, 3] = "Computername";
                    ws.Cells[1, 4] = "Access Point IP";
                    ws.Cells[1, 5] = "Access Point Name";
                    ws.Cells[1, 6] = "MAC-Address Requester";
                    ws.Cells[1, 7] = "SAM-Account-Name";
                    ws.Cells[1, 8] = "Responce Type";
                    ws.Cells[1, 9] = "Reason Code";

                    int i = 2;
                    foreach (ListViewItem item in lvLogTable.Items)
                    {
                        ws.Cells[i, 1] = item.SubItems[0].Text;
                        ws.Cells[i, 2] = item.SubItems[1].Text;
                        ws.Cells[i, 3] = item.SubItems[2].Text;
                        ws.Cells[i, 4] = item.SubItems[3].Text;
                        ws.Cells[i, 5] = item.SubItems[4].Text;
                        ws.Cells[i, 6] = item.SubItems[5].Text;
                        ws.Cells[i, 7] = item.SubItems[6].Text;
                        ws.Cells[i, 8] = item.SubItems[7].Text;
                        ws.Cells[i, 9] = item.SubItems[8].Text;
                        i++;
                    }

                    wb.SaveAs(sfd.FileName, XlFileFormat.xlWorkbookDefault);
                    app.Quit();

                    MessageBox.Show("Log has been succesfully exported to Excel", "Succesful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopySelectedValuesToClipboard();
        }
        private void copyMacAddressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopySelectedValuesToClipboardforMacAddress();
        }
        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lvLogTable.Items.Clear();
            cmCopy.Enabled = false;
            clearToolStripMenuItem.Enabled = false;
        }

        private void lvLogTable_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            cmCopy.Enabled = lvLogTable.SelectedItems.Count > 0;
        }

        private void lvLogTable_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Modifiers & Keys.Control) != 0)
            {
                if (e.KeyCode == Keys.C) CopySelectedValuesToClipboard();
                else if (e.KeyCode == Keys.A)
                {
                    foreach (ListViewItem item in lvLogTable.Items)
                        item.Selected = true;
                }
            }
        }

        private void lvLogTable_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;

            ListViewHitTestInfo hit = lvLogTable.HitTest(e.Location);
            if (hit?.Item == null)
            {
                cmSearch.Visible = false;
                cmSearchSep.Visible = false;
                contextMenuStrip1.Show(Cursor.Position);
                return;
            }

            if (hit.SubItem == null)
            {
                cmSearch.Visible = false;
                cmSearchSep.Visible = false;
            }
            else
            {
                cmSearch.Text = "Search on '" + hit.SubItem.Text + "'";
                cmSearch.Visible = true;
                cmSearchSep.Visible = true;
                search = hit.SubItem.Text;

                int key = 0;
                foreach (ListViewItem.ListViewSubItem cols in hit.Item.SubItems)
                {
                    if (cols.Text == search)
                    {
                        searchKey = key;
                        break;
                    }
                    key++;
                }
            }

            contextMenuStrip1.Show(Cursor.Position);
        }

        private void lvLogTable_MouseMove(object sender, MouseEventArgs e)
        {
            if (MouseButtons == MouseButtons.Left)
            {
                var item = lvLogTable.HitTest(e.Location).Item;
                if (item != null) item.Selected = true;
            }
            base.OnMouseMove(e);
        }

        private void lvLogTable_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                lvwColumnSorter.Order = (lvwColumnSorter.Order == SortOrder.Ascending)
                    ? SortOrder.Descending
                    : SortOrder.Ascending;
            }
            else
            {
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            lvLogTable.Sort();
        }

        private void cmSearch_Click(object sender, EventArgs e)
        {
            // legacy right-click search: sadece ekranda gösterileni filtreliyor
            if (oldItems.Count == 0)
            {
                foreach (ListViewItem item in lvLogTable.Items)
                    oldItems[item.Index] = item;

                cmRemoveSearch.Visible = true;
            }

            foreach (ListViewItem item in lvLogTable.Items)
            {
                if (item.SubItems.Count > searchKey && item.SubItems[searchKey].Text == search)
                    newItems[item.Index] = item;
            }

            lvLogTable.Items.Clear();
            foreach (var it in newItems.Values)
                lvLogTable.Items.Add(it);

            newItems.Clear();
        }

        private void cmRemoveSearch_Click(object sender, EventArgs e)
        {
            cmRemoveSearch.Visible = false;
            lvLogTable.Items.Clear();

            foreach (var it in oldItems.Values)
                lvLogTable.Items.Add(it);

            oldItems.Clear();
        }

        private void CopySelectedValuesToClipboard()
        {
            var builder = new StringBuilder();
            foreach (ListViewItem item in lvLogTable.SelectedItems)
            {
                builder.AppendLine(
                    item.SubItems[0].Text + "\t" +
                    item.SubItems[1].Text + "\t" +
                    item.SubItems[2].Text + "\t" +
                    item.SubItems[3].Text + "\t" +
                    item.SubItems[4].Text + "\t" +
                    item.SubItems[5].Text + "\t" +
                    item.SubItems[6].Text + "\t" +
                    item.SubItems[7].Text + "\t" +
                    item.SubItems[8].Text
                );
            }

            if (builder.Length > 0)
                Clipboard.SetText(builder.ToString());
        }

        private void CopySelectedValuesToClipboardforMacAddress()
        {
            var builder = new StringBuilder();
            foreach (ListViewItem item in lvLogTable.SelectedItems)
            {
                builder.AppendLine(
                    item.SubItems[5].Text.ToMacAddress()
                );
            }

            if (builder.Length > 0)
                Clipboard.SetText(builder.ToString());
        }

        // -------- Filter UI events (Designer isimleri) --------

        private void btnApplyFilter_Click_1(object sender, EventArgs e)
        {
            TriggerFilter();
        }

        private void btnClearFilter_Click_1(object sender, EventArgs e)
        {
            ClearFilterUI();
        }

        private void txtSearch_TextChanged_1(object sender, EventArgs e)
        {
            TriggerFilter();
        }

        private void chkFrom_CheckedChanged_1(object sender, EventArgs e)
        {
            dtFrom.Enabled = chkFrom.Checked;
            TriggerFilter();
        }

        private void chkTo_CheckedChanged_1(object sender, EventArgs e)
        {
            dtTo.Enabled = chkTo.Checked;
            TriggerFilter();
        }
    }
}
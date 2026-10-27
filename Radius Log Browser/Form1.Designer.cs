namespace Radius_Log_Browser
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.dataSet1 = new System.Data.DataSet();
            this.lvLogTable = new System.Windows.Forms.ListView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmSearch = new System.Windows.Forms.ToolStripMenuItem();
            this.cmRemoveSearch = new System.Windows.Forms.ToolStripMenuItem();
            this.cmSearchSep = new System.Windows.Forms.ToolStripSeparator();
            this.cmCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.loadFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cbScroll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.howToUseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ofdFile = new System.Windows.Forms.OpenFileDialog();
            this.panelTop = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.chkFrom = new System.Windows.Forms.CheckBox();
            this.dtFrom = new System.Windows.Forms.DateTimePicker();
            this.chkTo = new System.Windows.Forms.CheckBox();
            this.dtTo = new System.Windows.Forms.DateTimePicker();
            this.btnApplyFilter = new System.Windows.Forms.Button();
            this.btnClearFilter = new System.Windows.Forms.Button();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.copyMacAddressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataSet1
            // 
            this.dataSet1.DataSetName = "NewDataSet";
            // 
            // lvLogTable
            // 
            this.lvLogTable.AllowColumnReorder = true;
            this.lvLogTable.ContextMenuStrip = this.contextMenuStrip1;
            this.lvLogTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvLogTable.FullRowSelect = true;
            this.lvLogTable.HideSelection = false;
            this.lvLogTable.Location = new System.Drawing.Point(0, 66);
            this.lvLogTable.Name = "lvLogTable";
            this.lvLogTable.Size = new System.Drawing.Size(1063, 524);
            this.lvLogTable.SmallImageList = this.imageList1;
            this.lvLogTable.TabIndex = 0;
            this.lvLogTable.UseCompatibleStateImageBehavior = false;
            this.lvLogTable.View = System.Windows.Forms.View.Details;
            this.lvLogTable.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvLogTable_ColumnClick);
            this.lvLogTable.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lvLogTable_ItemSelectionChanged);
            this.lvLogTable.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvLogTable_KeyDown);
            this.lvLogTable.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lvLogTable_MouseDown);
            this.lvLogTable.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lvLogTable_MouseMove);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmSearch,
            this.cmRemoveSearch,
            this.cmSearchSep,
            this.cmCopy,
            this.copyMacAddressToolStripMenuItem,
            this.clearToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 142);
            // 
            // cmSearch
            // 
            this.cmSearch.Name = "cmSearch";
            this.cmSearch.Size = new System.Drawing.Size(180, 22);
            this.cmSearch.Text = "Search on";
            this.cmSearch.Visible = false;
            this.cmSearch.Click += new System.EventHandler(this.cmSearch_Click);
            // 
            // cmRemoveSearch
            // 
            this.cmRemoveSearch.Name = "cmRemoveSearch";
            this.cmRemoveSearch.Size = new System.Drawing.Size(180, 22);
            this.cmRemoveSearch.Text = "Remove search";
            this.cmRemoveSearch.Visible = false;
            this.cmRemoveSearch.Click += new System.EventHandler(this.cmRemoveSearch_Click);
            // 
            // cmSearchSep
            // 
            this.cmSearchSep.Name = "cmSearchSep";
            this.cmSearchSep.Size = new System.Drawing.Size(177, 6);
            // 
            // cmCopy
            // 
            this.cmCopy.Enabled = false;
            this.cmCopy.Name = "cmCopy";
            this.cmCopy.ShortcutKeyDisplayString = "Ctrl+C";
            this.cmCopy.Size = new System.Drawing.Size(180, 22);
            this.cmCopy.Text = "Copy";
            this.cmCopy.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.clearToolStripMenuItem.Text = "Clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "1476368477_BT_sort_az.png");
            this.imageList1.Images.SetKeyName(1, "1476368485_BT_sort_za.png");
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadFileToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.howToUseToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1063, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // loadFileToolStripMenuItem
            // 
            this.loadFileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectFileToolStripMenuItem,
            this.cbScroll,
            this.toolStripSeparator2,
            this.refreshToolStripMenuItem,
            this.toolStripSeparator1,
            this.closeToolStripMenuItem});
            this.loadFileToolStripMenuItem.Name = "loadFileToolStripMenuItem";
            this.loadFileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.loadFileToolStripMenuItem.Text = "File";
            // 
            // selectFileToolStripMenuItem
            // 
            this.selectFileToolStripMenuItem.Name = "selectFileToolStripMenuItem";
            this.selectFileToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.selectFileToolStripMenuItem.Text = "Select File";
            this.selectFileToolStripMenuItem.Click += new System.EventHandler(this.selectFileToolStripMenuItem_Click);
            // 
            // cbScroll
            // 
            this.cbScroll.Checked = true;
            this.cbScroll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbScroll.Name = "cbScroll";
            this.cbScroll.Size = new System.Drawing.Size(180, 22);
            this.cbScroll.Text = "Auto Scroll Down";
            this.cbScroll.Click += new System.EventHandler(this.cbScroll_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(95, 20);
            this.exportToolStripMenuItem.Text = "Export to Excel";
            this.exportToolStripMenuItem.Visible = false;
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // howToUseToolStripMenuItem
            // 
            this.howToUseToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.howToUseToolStripMenuItem.Image = global::Radius_Log_Browser.Properties.Resources.help_icon;
            this.howToUseToolStripMenuItem.Name = "howToUseToolStripMenuItem";
            this.howToUseToolStripMenuItem.Size = new System.Drawing.Size(28, 20);
            this.howToUseToolStripMenuItem.Click += new System.EventHandler(this.howToUseToolStripMenuItem_Click);
            // 
            // ofdFile
            // 
            this.ofdFile.Filter = "Log files|*.log";
            this.ofdFile.Title = "Select a Radius Log";
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.label1);
            this.panelTop.Controls.Add(this.txtSearch);
            this.panelTop.Controls.Add(this.chkFrom);
            this.panelTop.Controls.Add(this.dtFrom);
            this.panelTop.Controls.Add(this.chkTo);
            this.panelTop.Controls.Add(this.dtTo);
            this.panelTop.Controls.Add(this.btnApplyFilter);
            this.panelTop.Controls.Add(this.btnClearFilter);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 24);
            this.panelTop.Name = "panelTop";
            this.panelTop.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.panelTop.Size = new System.Drawing.Size(1063, 42);
            this.panelTop.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Search:";
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(65, 10);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(260, 20);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged_1);
            // 
            // chkFrom
            // 
            this.chkFrom.AutoSize = true;
            this.chkFrom.Location = new System.Drawing.Point(340, 12);
            this.chkFrom.Name = "chkFrom";
            this.chkFrom.Size = new System.Drawing.Size(49, 17);
            this.chkFrom.TabIndex = 2;
            this.chkFrom.Text = "From";
            this.chkFrom.UseVisualStyleBackColor = true;
            this.chkFrom.CheckedChanged += new System.EventHandler(this.chkFrom_CheckedChanged_1);
            // 
            // dtFrom
            // 
            this.dtFrom.CustomFormat = "dd.MM.yyyy HH:mm:ss";
            this.dtFrom.Enabled = false;
            this.dtFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtFrom.Location = new System.Drawing.Point(395, 10);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.ShowUpDown = true;
            this.dtFrom.Size = new System.Drawing.Size(170, 20);
            this.dtFrom.TabIndex = 3;
            // 
            // chkTo
            // 
            this.chkTo.AutoSize = true;
            this.chkTo.Location = new System.Drawing.Point(575, 12);
            this.chkTo.Name = "chkTo";
            this.chkTo.Size = new System.Drawing.Size(39, 17);
            this.chkTo.TabIndex = 4;
            this.chkTo.Text = "To";
            this.chkTo.UseVisualStyleBackColor = true;
            this.chkTo.CheckedChanged += new System.EventHandler(this.chkTo_CheckedChanged_1);
            // 
            // dtTo
            // 
            this.dtTo.CustomFormat = "dd.MM.yyyy HH:mm:ss";
            this.dtTo.Enabled = false;
            this.dtTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtTo.Location = new System.Drawing.Point(620, 10);
            this.dtTo.Name = "dtTo";
            this.dtTo.ShowUpDown = true;
            this.dtTo.Size = new System.Drawing.Size(170, 20);
            this.dtTo.TabIndex = 5;
            // 
            // btnApplyFilter
            // 
            this.btnApplyFilter.Location = new System.Drawing.Point(805, 8);
            this.btnApplyFilter.Name = "btnApplyFilter";
            this.btnApplyFilter.Size = new System.Drawing.Size(75, 25);
            this.btnApplyFilter.TabIndex = 6;
            this.btnApplyFilter.Text = "Apply";
            this.btnApplyFilter.UseVisualStyleBackColor = true;
            this.btnApplyFilter.Click += new System.EventHandler(this.btnApplyFilter_Click_1);
            // 
            // btnClearFilter
            // 
            this.btnClearFilter.Location = new System.Drawing.Point(885, 8);
            this.btnClearFilter.Name = "btnClearFilter";
            this.btnClearFilter.Size = new System.Drawing.Size(75, 25);
            this.btnClearFilter.TabIndex = 7;
            this.btnClearFilter.Text = "Clear";
            this.btnClearFilter.UseVisualStyleBackColor = true;
            this.btnClearFilter.Click += new System.EventHandler(this.btnClearFilter_Click_1);
            // 
            // copyMacAddressToolStripMenuItem
            // 
            this.copyMacAddressToolStripMenuItem.Name = "copyMacAddressToolStripMenuItem";
            this.copyMacAddressToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.copyMacAddressToolStripMenuItem.Text = "Copy Mac Address";
            this.copyMacAddressToolStripMenuItem.Click += new System.EventHandler(this.copyMacAddressToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1063, 590);
            this.Controls.Add(this.lvLogTable);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "RADIUS Log Browser";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Data.DataSet dataSet1;
        private System.Windows.Forms.ListView lvLogTable;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem loadFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem howToUseToolStripMenuItem;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem cmCopy;
        private System.Windows.Forms.ToolStripMenuItem cmSearch;
        private System.Windows.Forms.ToolStripSeparator cmSearchSep;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cmRemoveSearch;
        private System.Windows.Forms.OpenFileDialog ofdFile;
        private System.Windows.Forms.ToolStripMenuItem cbScroll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.DateTimePicker dtFrom;
        private System.Windows.Forms.DateTimePicker dtTo;
        private System.Windows.Forms.Button btnApplyFilter;
        private System.Windows.Forms.Button btnClearFilter;
        private System.Windows.Forms.CheckBox chkFrom;
        private System.Windows.Forms.CheckBox chkTo;
        private System.Windows.Forms.HelpProvider helpProvider1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem copyMacAddressToolStripMenuItem;
    }
}
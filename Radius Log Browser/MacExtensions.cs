using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Radius_Log_Browser
{
    public static class MacExtensions
    {
        public static string ToMacAddress(this string raw, char separator = ':')
        {
            if (string.IsNullOrWhiteSpace(raw))
                return raw;

            string hex = Regex.Replace(raw, "[^0-9a-fA-F]", "");

            if (hex.Length != 12)
                return raw;

            return Regex.Replace(hex.ToLowerInvariant(), "(.{2})(?!$)", "$1" + separator);
        }
    }
}

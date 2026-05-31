using System;
using System.Collections.Generic;
using System.Text;

namespace RedVault.Utilities
{
    internal class FileHelper
    {
        public static string FormatSize(long bytes)
        {
            string[] suffixes = { "B", "KB", "MB", "GB" };

            double size = bytes;
            int i = 0;

            while (size >= 1024 && i < suffixes.Length - 1)
            {
                size /= 1024;
                i++;
            }

            return $"{size:F1} {suffixes[i]}";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace RedVault.Helpers
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
        
        public static IEnumerable<string> EnumerateFiles(string path, string[] patterns)
        {
            foreach (string pattern in patterns)
            {
                IEnumerable<string> files;

                try
                {
                    files = Directory.EnumerateFiles(path, pattern, new EnumerationOptions { IgnoreInaccessible = true, RecurseSubdirectories = true });
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); continue; }

                foreach (var file in files)
                {
                    yield return file;
                }

            }

        }

        public static bool ContainsString(string path,string searchtext)
        {
            try
            {
                foreach(string line in File.ReadLines(path))
                {
                    if (line.Contains(searchtext, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); return false; }

            return false;

        }

        public static string[] GetUsers()
        {
            string[] excluded = { "Public", "Default", "Default User", "All Users" };

            var list = Directory.GetDirectories(@"C:\Users")
                .Select(Path.GetFileName)
                .Where(item => !string.IsNullOrEmpty(item))
                .Where(item => !excluded.Contains(item, StringComparer.OrdinalIgnoreCase)).ToArray();

            return list;
        }





    }
        
}

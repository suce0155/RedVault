using System;
using System.Collections.Generic;
using System.Text;

namespace RedVault.Utilities
{
    internal class PowerShell
    {
        // Returns valid users under C:\Users
        public static string[] GetUsers()
        {
            string[] excluded = { "Public", "Default", "Default User", "All Users" };

            var list = Directory.GetDirectories(@"C:\Users").Select(Path.GetFileName).Where(item => !excluded.Contains(item, StringComparer.OrdinalIgnoreCase)).ToArray();
            
            return list;
        }

    }
}

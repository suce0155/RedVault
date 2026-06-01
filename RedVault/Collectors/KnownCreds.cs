using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedVault.Collectors
{
    internal class KnownCreds
    {
        public static void EnumWinlogon()
        {
            string[] values =
            {
                "DefaultDomainName",
                "DefaultUserName",
                "DefaultPassword",
                "AltDefaultDomainName",
                "AltDefaultUserName",
                "AltDefaultPassword"
            };

            Console.WriteLine("Enumerating Winlogon Creds...");

            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon1");

                if (key == null)
                {
                    Console.WriteLine("Registry key not found.");
                    return;
                }

                foreach (string item in values)
                {
                    var value = key.GetValue(item, String.Empty).ToString();

                    if (!String.IsNullOrEmpty(value))
                    {
                        Console.WriteLine($"[Found] {item} : {value}");
                    }

                }
                key.Close();

            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

        }

    }
}

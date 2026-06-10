using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
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

        public static void EnumSavedRdpConnections()
        {
            string path1 = @"{0}\Software\Microsoft\Terminal Server Client\Servers\";

            Console.WriteLine("Enumerating Saved RDP Creds...");

            foreach (string name in Registry.Users.GetSubKeyNames())
            {
                
                var servers = Registry.Users.OpenSubKey(String.Format(path1, name));

                if (servers != null) 
                {
                    foreach(string server in servers.GetSubKeyNames())
                    {
                        using (var serverKey = servers.OpenSubKey(server))
                        {
                            string usernameHint = serverKey?.GetValue(server, String.Empty).ToString();
                            Console.WriteLine($"Server: {server}");
                            Console.WriteLine($"UsernameHint: {usernameHint}");
                        }
                    }

                }    
                
            }
        }


        public static void EnumRecentCommands()
        {
            string path = @"{0}\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\RunMRU\";

            Console.WriteLine("Enumerating Recently Run Commands...");

            foreach (string name in Registry.Users.GetSubKeyNames())
            {
                var key = Registry.Users.OpenSubKey(String.Format(path, name));

                if (key != null)
                {
                    foreach (string value in key.GetValueNames())
                    {
                        Console.WriteLine($"{value} = {key.GetValue(value)}");

                    }

                }

            }



        }





    }
}

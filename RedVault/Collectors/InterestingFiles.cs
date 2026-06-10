using System;
using System.Collections.Generic;
using System.Text;

namespace RedVault.Collectors
{
    internal class InterestingFiles
    {
        public static void EnumFilesOfInterest()
        {
            Console.WriteLine("Enumerating Files of Interest...");
            string[] files =
            {
                @"C:\pagefile.sys",
                @"C:\Windows\debug\NetSetup.log",
                @"C:\Windows\repair\sam",
                @"C:\Windows\repair\system",
                @"C:\Windows\repair\software",
                @"C:\Windows\repair\security",
                @"C:\Windows\iis6.log",
                @"C:\Windows\system32\config\AppEvent.Evt",
                @"C:\Windows\system32\config\SecEvent.Evt",
                @"C:\Windows\system32\config\default.sav",
                @"C:\Windows\system32\config\security.sav",
                @"C:\Windows\system32\config\software.sav",
                @"C:\Windows\system32\config\system.sav",
                @"C:\Windows\system32\config\RegBack\SAM",
                @"C:\Windows\system32\config\SAM",
                @"C:\Windows\system32\config\SYSTEM",
                @"C:\Windows\system32\config\RegBack\SYSTEM",

            };

            string[] folders =
            {
                @"C:\Windows\system32\CCM\logs",
                @"C:\ProgramData\Configs",
                @"C:\Program Files\Windows PowerShell"
             };

            foreach (string file in files)
            {
                if (File.Exists(file))
                {
                    Console.WriteLine($"[Found]  {file}");
                }

            }
            foreach (string file in folders)
            {
                if (Directory.Exists(file))
                {
                    foreach(string name in Helpers.FileHelper.EnumerateFiles(file, ["*"]))
                    {
                        Console.WriteLine($"[Found]  {name}");
                    }
                }


            }


        }

    }
}

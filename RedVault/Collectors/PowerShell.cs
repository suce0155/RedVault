using RedVault.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
namespace RedVault.Collectors
{
    internal class PowerShell
    {
        internal class PSFileInfo
        {
            internal string Path { get; set; }
            internal int LineCount { get; set; }
            internal long SizeBytes { get; set; }
        }


        const string HistoryPath =
            @"C:\Users\{0}\AppData\Roaming\Microsoft\Windows\PowerShell\PSReadLine\ConsoleHost_history.txt";

        const string TranscriptPath =
            @"C:\Users\{0}\Documents";

        public static void Run()
        {
            Console.WriteLine("Reading PS History & Trans for readable users....");
            var users = Helpers.FileHelper.GetUsers();

            foreach(var user in users)
            {
                string hpath = String.Format(HistoryPath, user);
                if (File.Exists(hpath))
                {

                    try
                    {

                        PSFileInfo psinfo = new PSFileInfo();
                        psinfo.Path = hpath;
                        psinfo.SizeBytes = new FileInfo(hpath).Length;
                        psinfo.LineCount = File.ReadLines(hpath).Count();

                        Console.WriteLine("[PowerShell History]");
                        Console.WriteLine($"Path: {psinfo.Path}");
                        Console.WriteLine($"Lines: {psinfo.LineCount}");
                        Console.WriteLine($"Size: {Helpers.FileHelper.FormatSize(psinfo.SizeBytes)}");
                        Console.WriteLine();

                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }
                }

                string tpath = String.Format(TranscriptPath, user);

                if (Directory.Exists(tpath))
                {
                    try
                    {

                        int tcount = 0;
                        long tsize = 0;

                        var tdir = Directory.EnumerateFiles(tpath, "PowerShell_transcript*", SearchOption.TopDirectoryOnly);

                        foreach (var file in tdir)
                        {
                            tcount++;
                            tsize += new FileInfo(file).Length;
                        }

                        Console.WriteLine("[PowerShell Transcript]");
                        Console.WriteLine($"Path: {String.Join("", tpath, @"\PowerShell_transcript*")}");
                        Console.WriteLine($"Found: {tcount}");
                        Console.WriteLine($"Total Size: {Helpers.FileHelper.FormatSize(tsize)}");
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }
                }

            }

        }

    }
}

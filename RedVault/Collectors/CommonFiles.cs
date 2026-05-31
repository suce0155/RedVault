using System;
using System.Collections.Generic;
using System.Text;
using RedVault.Helpers;
using Microsoft.Data.Sqlite;


namespace RedVault.Collectors
{
    internal class CommonFiles
    {
        public static void EnumUnattend()
        {
            Console.WriteLine("[Enumerating Unattend Files..]");
            string[] patterns =
            {
                "*sysprep.inf",
                "*sysprep.xml",
                "*unattended.xml",
                "*unattend.xml",
                "*unattend.txt"
            };
            foreach (string files in Helpers.FileHelper.EnumerateFiles(@"C:\",patterns))
            {
                Console.WriteLine(files);

            }
        } 

        public static void EnumExtensions()
        {
            string[] patterns =
            {
                "*pass*.txt",
                "*pass*.xml",
                "*pass*.ini",
                "*cred*",
                "*vnc*",
                "*.config*"
            };
            Console.WriteLine("[Enumerating *pass* *cred* *config*]");
            foreach (string files in Helpers.FileHelper.EnumerateFiles(@"C:\", patterns))
            {
                Console.WriteLine(files);

            }
        }
        public static void EnumPasswordString()
        {
            string[] patterns =
            {
                "*.txt",
                "*.ini",
                "*.cfg",
                "*.config",
                "*.xml",
                "*.ps1"
            };
            Console.WriteLine("[Enumerating *.txt *.ini *.cfg *.config *.xml for 'pass']");

            foreach (string path in Helpers.FileHelper.EnumerateFiles(@"C:\", patterns))
            {
                if (Helpers.FileHelper.ContainsString(path, "pass"))
                {
                    Console.WriteLine(path);
                }

            }
        }


        public static void EnumStickyNotes()
        {
            string dbPath = @"C:\Users\{0}\AppData\Local\Packages\Microsoft.MicrosoftStickyNotes_8wekyb3d8bbwe\LocalState\plum.sqlite";


            Console.WriteLine("Enumerating Sticky Notes..");

            var users = Helpers.FileHelper.GetUsers();

            foreach(string user in users)
            {

                string path = String.Format(dbPath, user);

                if (File.Exists(path))
                {
                    Console.WriteLine("[Sticky Notes DB Found].");
                    Console.WriteLine($"Path: {path}");
                    Console.WriteLine("Notes: ");
                    try
                    {
                        using (SqliteConnection connection = new SqliteConnection($@"Data Source={path};Mode=ReadOnly"))
                        {
                            connection.Open();

                            var command = connection.CreateCommand();
                            command.CommandText = "SELECT TEXT FROM Note";

                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Console.WriteLine(reader.GetString(0));
                                    Console.WriteLine("-----");
                                }
                            }


                        }

                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }

                }

            }
        }






    }
}

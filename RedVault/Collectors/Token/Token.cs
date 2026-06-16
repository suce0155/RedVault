using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Xml.Linq;
using static RedVault.Modules.m_process;
using static RedVault.Modules.m_token;
using static RedVault.Modules.f_token;
using static RedVault.Modules.f_process;
namespace RedVault.Collectors.Token
   
{
    internal class Token
    {

        public static void Whoami()
        {
            Console.Write("* Process Token :");
            if (OpenProcessToken(GetCurrentProcess(), 0x0008, out IntPtr tokenHandle))
            {    
                f_token_print_token_info(tokenHandle, true);
                CloseHandle(tokenHandle);
            }
            else
            {
                Console.WriteLine($"[-] OpenProcessToken failed with error");
            }

            Console.Write("* Thread Token :");
            if (OpenThreadToken(GetCurrentThread(), 0x0008, true, out IntPtr tokenHandle1))
            {
                f_token_print_token_info(tokenHandle1, true);
                CloseHandle(tokenHandle1);
            }
            else if (Marshal.GetLastWin32Error() == 1008)
            {
                Console.WriteLine($"[-] No token.");
            }
            else
            {
                Console.WriteLine($"[-] OpenThreadToken failed with error: {Marshal.GetLastWin32Error()}");
            }



        }
        public static void List()
        {
            Console.WriteLine("* Listable Tokens: \n");
            HashSet<ulong> seen = new();

            var processlist = f_process_enum_processes();
            foreach (var process in processlist)
            {
                if (!f_token_get_process_token(process.ProcessId, out IntPtr hToken))
                    continue;

                try
                {
                    if (!f_token_get_luid(hToken, out ulong luid))
                    {
                        Console.WriteLine($"[-] f_token_get_luid failed: {Marshal.GetLastWin32Error()}");
                        continue;
                    }

                    if (!seen.Add(luid))
                        continue;

                    f_token_print_token_info(hToken, false);
                }
                finally
                {
                    CloseHandle(hToken);
                }
            }
            
            var handlelist = f_process_enum_handles("Token");
            foreach (var handle in handlelist)
            {

                try
                {
                    if (!f_token_get_luid(handle, out ulong luid))
                    {
                        Console.WriteLine($"[-] f_token_get_luid failed: {Marshal.GetLastWin32Error()}");
                        continue;
                    }

                    if (!seen.Add(luid))
                        continue;

                    f_token_print_token_info(handle, false);
                }
                finally
                {
                    CloseHandle(handle);
                }



            }

        }

        public static void Run()
        {
            
        }

        public static void Elevate()
        {

        }


        




    }
}

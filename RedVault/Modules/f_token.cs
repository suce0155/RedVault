using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using static RedVault.Modules.m_process;
using static RedVault.Modules.m_token;

namespace RedVault.Modules
{
    internal class f_token
    {
        public static bool f_token_get_process_token(uint processId, out IntPtr hToken)
        {
            uint p_access = 0x0400; // PROCESS_QUERY_INFORMATION
            uint t_access = 0x0008; // TOKEN_QUERY_INFORMATION
            hToken = IntPtr.Zero;

            IntPtr hProcess = OpenProcess(p_access, false, processId);

            if (hProcess == IntPtr.Zero)
            {
                return false;
            }
            bool result = OpenProcessToken(hProcess, t_access, out hToken);
            CloseHandle(hProcess);
            return result;

        }

        public static void f_token_print_token_info(IntPtr tokenHandle, bool full)
        {

            var tokeninfo = Marshal.AllocHGlobal(Marshal.SizeOf<TOKEN_STATISTICS>());
            TOKEN_STATISTICS statistics = new();
            if (!GetTokenInformation(tokenHandle, TOKEN_INFORMATION_CLASS.TokenStatistics, tokeninfo, Marshal.SizeOf<TOKEN_STATISTICS>(), out _))
            {
                Console.WriteLine($"[-] GetTokenInformation(TokenStatistics) failed: {Marshal.GetLastWin32Error()}");
                Marshal.FreeHGlobal(tokeninfo);
            }

            else
            {
                statistics = Marshal.PtrToStructure<TOKEN_STATISTICS>(tokeninfo);
                Console.Write($" [{statistics.AuthenticationId.HighPart},{statistics.AuthenticationId.LowPart}]");
                Marshal.FreeHGlobal(tokeninfo);

            }



            var sessionid = Marshal.AllocHGlobal(sizeof(uint));

            if (!GetTokenInformation(tokenHandle, TOKEN_INFORMATION_CLASS.TokenSessionId, sessionid, sizeof(uint), out _))
            {
                Console.WriteLine($"[-] GetTokenInformation(TokenSessionId) failed: {Marshal.GetLastWin32Error()}");
                Marshal.FreeHGlobal(sessionid);
            }

            else
            {
                Console.Write($" {(uint)Marshal.ReadInt32(sessionid)} ");
                Marshal.FreeHGlobal(sessionid);
            }


            var elevation_type = Marshal.AllocHGlobal(sizeof(int));
            if (!GetTokenInformation(tokenHandle, TOKEN_INFORMATION_CLASS.TokenElevationType, elevation_type, sizeof(int), out _))
            {
                Console.WriteLine($"[-] GetTokenInformation(TokenElevationType) failed: {Marshal.GetLastWin32Error()}");
                Marshal.FreeHGlobal(elevation_type);
            }

            else
            {
                elevation_type = Marshal.ReadInt32(elevation_type);
                string elevStr = (elevation_type == 1 ? "Default" : "") + (elevation_type == 2 ? "Full" : "") + (elevation_type == 3 ? "Limited" : "");

                Console.Write($"[{elevStr}]");
                Marshal.FreeHGlobal(elevation_type);
            }

            Console.Write($" {statistics.TokenId.LowPart} ");

            GetTokenInformation(tokenHandle, TOKEN_INFORMATION_CLASS.TokenUser, IntPtr.Zero, 0, out int usersize);
            var tokenuser = Marshal.AllocHGlobal(usersize);

            if (!GetTokenInformation(tokenHandle, TOKEN_INFORMATION_CLASS.TokenUser, tokenuser, usersize, out _))
            {
                Console.WriteLine($"[-] GetTokenInformation(TokenUser) failed: {Marshal.GetLastWin32Error()}");
                Marshal.FreeHGlobal(tokenuser);
            }

            else
            {
                TOKEN_USER user = Marshal.PtrToStructure<TOKEN_USER>(tokenuser);
                SecurityIdentifier sid = new SecurityIdentifier(user.User.Sid);
                string sidString = sid.Value;
                var account = (NTAccount)sid.Translate(typeof(NTAccount));
                string fullname = account.Value;
                Console.Write($" {fullname}  {sidString}");
                Marshal.FreeHGlobal(tokenuser);
            }

            Console.WriteLine($"  [{statistics.GroupCount}g,{statistics.PrivilegeCount}p]  {statistics.TokenType} ");

            if (full)
            {
                GetTokenInformation(tokenHandle, TOKEN_INFORMATION_CLASS.TokenGroupsAndPrivileges, IntPtr.Zero, 0, out int gsize);

                var groupriv = Marshal.AllocHGlobal(gsize);

                if (!GetTokenInformation(tokenHandle, TOKEN_INFORMATION_CLASS.TokenGroupsAndPrivileges, groupriv, gsize, out _))
                {
                    Console.WriteLine($"[-] GetTokenInformation(TokenGroupsAndPrivileges) failed: {Marshal.GetLastWin32Error()}");
                    Marshal.FreeHGlobal(groupriv);
                }
                else
                {
                    var tokengroupriv = Marshal.PtrToStructure<TOKEN_GROUPS_AND_PRIVILEGES>(groupriv);

                    f_display_sids('G', tokengroupriv.SidCount, tokengroupriv.Sids);
                    f_display_sids('R', tokengroupriv.RestrictedSidCount, tokengroupriv.RestrictedSids);

                    for (int i = 0; i < tokengroupriv.PrivilegeCount; i++)
                    {
                        IntPtr current = IntPtr.Add(tokengroupriv.Privileges, i * Marshal.SizeOf<LUID_AND_ATTRIBUTES>());
                        var luidAatr = Marshal.PtrToStructure<LUID_AND_ATTRIBUTES>(current);

                        string privflags = string.Concat(
                            (luidAatr.Attributes & 0x00000001) != 0 ? "D" : " ",             // SE_PRIVILEGE_ENABLED_BY_DEFAULT
                            (luidAatr.Attributes & 0x00000002) != 0 ? "E" : " ",             // SE_PRIVILEGE_ENABLED
                            (luidAatr.Attributes & 0x00000004) != 0 ? "R" : " ",             // SE_PRIVILEGE_REMOVED
                            (luidAatr.Attributes & 0x80000000) != 0 ? "A" : " "              // SE_PRIVILEGE_USED_FOR_ACCESS

                        );

                        Console.Write($"   P:[{privflags}]");
                        uint privsize = 0;
                        LookupPrivilegeName(null, ref luidAatr.Luid, null, ref privsize);
                        var privname = new StringBuilder((int)privsize);

                        if (!LookupPrivilegeName(null, ref luidAatr.Luid, privname, ref privsize))
                        {
                            Console.WriteLine($"[-] LookupPrivilegeName failed: {Marshal.GetLastWin32Error()}");

                        }
                        else
                        {
                            Console.WriteLine($"  {privname}");
                        }

                    }
                    Marshal.FreeHGlobal(groupriv);
                }
            }
        }
        public static bool f_token_get_luid(IntPtr tokenHandle, out ulong luid)
        {
            luid = 0;
            var tokeninfo = Marshal.AllocHGlobal(Marshal.SizeOf<TOKEN_STATISTICS>());
            TOKEN_STATISTICS statistics = new();
            if (!GetTokenInformation(tokenHandle, TOKEN_INFORMATION_CLASS.TokenStatistics, tokeninfo, Marshal.SizeOf<TOKEN_STATISTICS>(), out _))
            {
                Console.WriteLine($"[-] GetTokenInformation(TokenStatistics) failed: {Marshal.GetLastWin32Error()}");
                Marshal.FreeHGlobal(tokeninfo);
                return false;
            }

            else
            {
                statistics = Marshal.PtrToStructure<TOKEN_STATISTICS>(tokeninfo);
                
                luid = ((ulong)statistics.AuthenticationId.HighPart << 32) | statistics.AuthenticationId.LowPart;
                Marshal.FreeHGlobal(tokeninfo);
                return true;

            }
        }
        static void f_display_sids(char l, uint count, IntPtr sidArray)
        {
            for (int i = 0; i < count; i++)
            {
                IntPtr current = IntPtr.Add(sidArray, i * Marshal.SizeOf<SID_AND_ATTRIBUTES>());
                var sidAatr = Marshal.PtrToStructure<SID_AND_ATTRIBUTES>(current);


                string flags = string.Concat(
                    (sidAatr.Attributes & 0x00000001) != 0 ? "M" : " ",             // SE_GROUP_MANDATORY
                    (sidAatr.Attributes & 0x00000002) != 0 ? "D" : " ",             // SE_GROUP_ENABLED_BY_DEFAULT
                    (sidAatr.Attributes & 0x00000004) != 0 ? "E" : " ",             // SE_GROUP_ENABLED
                    (sidAatr.Attributes & 0x00000008) != 0 ? "O" : " ",             // SE_GROUP_OWNER
                    (sidAatr.Attributes & 0x00000010) != 0 ? "U" : " ",             // SE_GROUP_USE_FOR_DENY_ONLY
                    (sidAatr.Attributes & 0xC0000000) != 0 ? "L" : " ",             // SE_GROUP_LOGON_ID
                    (sidAatr.Attributes & 0x20000000) != 0 ? "R" : " "              // SE_GROUP_RESOURCE
                );

                Console.Write($"   {l}:[{flags}]");

                uint namelen = 0;
                uint domainlen = 0;

                LookupAccountSid(null, sidAatr.Sid, null, ref namelen, null, ref domainlen, out int peUse);

                var sidAatrName = new StringBuilder((int)namelen);
                var sidAatrDomain = new StringBuilder((int)domainlen);
                if (!LookupAccountSid(null, sidAatr.Sid, sidAatrName, ref namelen, sidAatrDomain, ref domainlen, out peUse))
                {
                    Console.WriteLine($"[-] LookupAccountSid({l}) failed: {Marshal.GetLastWin32Error()}");
                }
                else
                {
                    Console.WriteLine(@$"  {sidAatrDomain}\{sidAatrName}");
                }
            }
        }




    }
}

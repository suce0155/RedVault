using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using static RedVault.Modules.m_process;

namespace RedVault.Modules
{
    internal class f_process
    {
        public static ProcessEntry[] f_process_enum_processes()
        {
            var results = new List<ProcessEntry>();

            var result = f_process_NtQuerySystemInformation(SYSTEM_INFORMATION_CLASS.SystemProcessInformation, out IntPtr ntquery, out uint ntqsize);

            if (result != 0)
            {
                return results.ToArray();
            }

            IntPtr current = ntquery;

            while (true)
            {
                var entry = Marshal.PtrToStructure<SYSTEM_PROCESS_INFORMATION>(current);

                string name = entry.ImageName.Buffer != IntPtr.Zero
                    ? Marshal.PtrToStringUni(entry.ImageName.Buffer, entry.ImageName.Length / 2)
                    : "[Idle]";
                
                results.Add(new ProcessEntry((uint)entry.UniqueProcessId.ToInt64(), entry.NumberOfThreads, name));

                if (entry.NextEntryOffset == 0) break;
                current += (int)entry.NextEntryOffset;
            }

            Marshal.FreeHGlobal(ntquery);
            return results.ToArray();

        }

        public static IntPtr[] f_process_enum_handles(string handleType = "")
        {
            // "" returns all handle types

            var results = new List<IntPtr>();

            var result = f_process_NtQuerySystemInformation(SYSTEM_INFORMATION_CLASS.SystemHandleInformation, out IntPtr ntquery, out uint ntqsize);

            if (result != 0)
            {
                return results.ToArray();
            }

            var size = Marshal.ReadInt64(ntquery);
            ntquery = ntquery + IntPtr.Size;

            for (int i  = 0; i < size; i++ )
            {
                var current = IntPtr.Add(ntquery, i * Marshal.SizeOf<SYSTEM_HANDLE_TABLE_ENTRY_INFO>());
                var handle = Marshal.PtrToStructure<SYSTEM_HANDLE_TABLE_ENTRY_INFO>(current);
                

                uint procAccess = 0x0040; // PROCESS_DUP_HANDLE
                var hProcess = OpenProcess(procAccess, false, handle.UniqueProcessId);

                if (hProcess == IntPtr.Zero)
                {
                    continue;
                }

                const uint TOKEN_DUPLICATE = 0x0002;
                const uint TOKEN_QUERY = 0x0008;
                var dup = DuplicateHandle(hProcess, new nint(handle.HandleValue), GetCurrentProcess(), out IntPtr targetHandle,
                                                TOKEN_QUERY | TOKEN_DUPLICATE,  
                                                                        true,
                                                                            0);

                if (!dup)
                {
                    CloseHandle(hProcess);
                    continue;
                    
                }
                CloseHandle(hProcess);

                NtQueryObject(targetHandle, OBJECT_INFORMATION_CLASS.ObjectTypeInformation, IntPtr.Zero, 0, out int ntobjsize);
                var ntobject = Marshal.AllocHGlobal(ntobjsize);

                result = NtQueryObject(targetHandle, OBJECT_INFORMATION_CLASS.ObjectTypeInformation, ntobject, ntobjsize, out ntobjsize);
                if (result != 0)
                {
                    CloseHandle(targetHandle);
                    Marshal.FreeHGlobal(ntobject);
                    continue;
                }

                var typeinfo = Marshal.PtrToStructure<PUBLIC_OBJECT_TYPE_INFORMATION>(ntobject);

                var str = Marshal.PtrToStringUni(typeinfo.TypeName.Buffer, typeinfo.TypeName.Length / sizeof(char));

                if (handleType == "" || str == handleType)
                {
                    results.Add(targetHandle);
                }
                else
                {
                    CloseHandle(targetHandle);
                }

                Marshal.FreeHGlobal(ntobject);
                
            }

            Marshal.FreeHGlobal(ntquery);

            return results.ToArray();
        }


        public static int f_process_NtQuerySystemInformation(SYSTEM_INFORMATION_CLASS informationclass, out IntPtr buffer, out uint buffsize)
        {
            uint status = 0xC0000004; //STATUS_INFO_LENGTH_MISMATCH;

            buffer = IntPtr.Zero;
            buffsize = 0x1000;


            while (status == 0xC0000004)
            {
                buffer = Marshal.AllocHGlobal((int)buffsize);
                status = (uint)NtQuerySystemInformation(informationclass, buffer, buffsize, out _);
                
                if (status != 0)
                {
                    Marshal.FreeHGlobal(buffer);
                    buffer = IntPtr.Zero;
                    buffsize <<= 1;
                }


            }

            var result = NtQuerySystemInformation(informationclass, buffer, buffsize, out _);

            if (result != 0)
            {
                Marshal.FreeHGlobal(buffer);
                return result;
            }

            return result;

        }


    }
}

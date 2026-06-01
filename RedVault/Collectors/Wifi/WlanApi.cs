using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RedVault.Collectors.Wifi
{
    internal class WlanApi
    {
        [DllImport("wlanapi.dll",SetLastError = true)]
        internal static extern uint WlanOpenHandle(
            uint dwClientVersion,
            IntPtr pReserved,
            out uint pdwNegotiatedVersion,
            out IntPtr phClientHandle
            );

        [DllImport("wlanapi.dll", SetLastError = true)]
        internal static extern uint WlanEnumInterfaces(
            IntPtr hClientHandle,
            IntPtr pReserved,
            out IntPtr pInterfaceList
            );

        [DllImport("wlanapi.dll", SetLastError = true)]
        internal static extern uint WlanGetProfileList(
            IntPtr hClientHandle,
            ref Guid pInterfaceGuid,
            IntPtr pReserved,
            out IntPtr pProfileList
            );

        [DllImport("wlanapi.dll", SetLastError = true,CharSet = CharSet.Unicode)]
        internal static extern uint WlanGetProfile(
            IntPtr hClientHandle,
            ref Guid InterfaceGuid,
            string ProfileName,
            IntPtr pReserved,
            out IntPtr profileXml,
            ref uint flags,
            out uint grantedAccess
            );

        [DllImport("wlanapi.dll", SetLastError = true)]
        internal static extern uint WlanCloseHandle(
            IntPtr hClientHandle,
            IntPtr pReserved
            );

        [DllImport("wlanapi.dll", ExactSpelling = true, SetLastError = false)]
        internal static extern void WlanFreeMemory(IntPtr pMemory);

    }
}

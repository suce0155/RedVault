using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RedVault.Modules
{
    internal class m_wlan
    {
        // WLAN_INTERFACE_INFO_LIST
        [StructLayout(LayoutKind.Sequential)]
        internal struct WLAN_INTERFACE_INFO_LIST
        {
            internal uint NumberOfItems;
            internal uint Index;
            //internal WLAN_INTERFACE_INFO InterfaceInfo;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct WLAN_INTERFACE_INFO
        {
            internal Guid InterfaceGuid;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            internal string strInterfaceDescription;

            internal WLAN_INTERFACE_STATE isState;
        }

        internal enum WLAN_INTERFACE_STATE
        {
            wlan_interface_state_not_ready,
            wlan_interface_state_connected,
            wlan_interface_state_ad_hoc_network_formed,
            wlan_interface_state_disconnecting,
            wlan_interface_state_disconnected,
            wlan_interface_state_associating,
            wlan_interface_state_discovering,
            wlan_interface_state_authenticating
        }

        // WLAN_PROFILE_INFO_LIST
        [StructLayout(LayoutKind.Sequential)]
        internal struct WLAN_PROFILE_INFO_LIST
        {
            internal uint NumberOfItems;
            internal uint Index;
            //internal WLAN_PROFILE_INFO ProfileInfo;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct WLAN_PROFILE_INFO
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            internal string ProfileName;

            uint Flags;
        }



        [DllImport("wlanapi.dll", SetLastError = true)]
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

        [DllImport("wlanapi.dll", SetLastError = true, CharSet = CharSet.Unicode)]
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

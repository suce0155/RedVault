using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RedVault.Collectors.Wifi
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

    [StructLayout(LayoutKind.Sequential,CharSet = CharSet.Unicode)]
    internal struct WLAN_PROFILE_INFO
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        internal string ProfileName;

        uint Flags;
    }


}

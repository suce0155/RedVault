using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RedVault.Collectors.Vault
{
    [StructLayout(LayoutKind.Sequential)]
    public struct VAULT_GUID_STRING
    {
        public Guid guid;
        public string text;
    }

    public enum VAULT_ELEMENT_TYPE : int
    {
        Undefined = -1,
        Boolean = 0,
        Short = 1,
        UnsignedShort = 2,
        Int = 3,
        UnsignedInt = 4,
        Double = 5,
        Guid = 6,
        String = 7,
        ByteArray = 8,
        TimeStamp = 9,
        ProtectedArray = 10,
        Attribute = 11,
        Sid = 12,
        Last = 13
    }
    

    [StructLayout(LayoutKind.Sequential)]
    public struct VAULT_BYTE_ARRAY
    {
        public int Length;
        public IntPtr pData;
    }

    public enum VAULT_SCHEMA_ELEMENT_ID : int
    {
        Illegal = 0,
        Resource = 1,
        Identity = 2,
        Authenticator = 3,
        Tag = 4,
        PackageSid = 5,
        AppStart = 100,
        AppEnd = 10000
    }

    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
    public struct VAULT_ITEM_DATA
    {
        [FieldOffset(0)] public VAULT_SCHEMA_ELEMENT_ID SchemaElementId;
        [FieldOffset(8)] public VAULT_ELEMENT_TYPE Type;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VAULT_ITEM_7 
    {
        public Guid SchemaId;
        public IntPtr FriendlyName;
        public IntPtr Resource;        //VAULT_ITEM_DATA
        public IntPtr Identity;        //VAULT_ITEM_DATA
        public IntPtr Authenticator;   //VAULT_ITEM_DATA
    }

    
    [StructLayout(LayoutKind.Sequential, Pack = 8, CharSet = CharSet.Unicode)]
    public struct VAULT_ITEM_8
    {
        public Guid SchemaId;

        public IntPtr FriendlyName;

        public IntPtr Resource;         //VAULT_ITEM_DATA
        public IntPtr Identity;         //VAULT_ITEM_DATA
        public IntPtr Authenticator;    //VAULT_ITEM_DATA
        public IntPtr PackageSid;       //VAULT_ITEM_DATA

        public ulong LastWritten;
        public uint Flags;
        public uint cbProperties;

        public IntPtr Properties;       //VAULT_ITEM_DATA
    }
    
}

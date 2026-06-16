using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RedVault.Modules
{
    internal class m_vault
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

        [DllImport("vaultcli.dll", SetLastError = true)]
        internal static extern int VaultEnumerateVaults(
            uint unk0,
            out uint cbItems,
            out IntPtr items);

        [DllImport("vaultcli.dll", SetLastError = true)]
        internal static extern int VaultOpenVault(
            Guid vaultGUID,
            uint unk0,
            out IntPtr hVault);

        [DllImport("vaultcli.dll", SetLastError = true)]
        internal static extern int VaultEnumerateItems(
            IntPtr hVault,
            uint unk0,
            out uint cbItems,
            out IntPtr items);


        [DllImport("vaultcli.dll", EntryPoint = "VaultGetItem", SetLastError = true)]
        public static extern int VaultGetItem_WIN8(
            IntPtr vaultHandle,
            ref Guid schemaId,
            IntPtr pResourceElement,
            IntPtr pIdentityElement,
            IntPtr pPackageSid,
            IntPtr zero,
            int arg6,
            ref IntPtr passwordVaultPtr);


        [DllImport("vaultcli.dll", EntryPoint = "VaultGetItem", SetLastError = true)]
        public static extern int VaultGetItem_WIN7(
            IntPtr vaultHandle,
            ref Guid schemaId,
            IntPtr pResourceElement,
            IntPtr pIdentityElement,
            IntPtr zero, int arg5,
            ref IntPtr passwordVaultPtr);


        [DllImport("vaultcli.dll", SetLastError = true)]
        public static extern int VaultCloseVault(ref IntPtr vaultHandle);


        [DllImport("vaultcli.dll", SetLastError = true)]
        public static extern int VaultFree(IntPtr buffer);





    }
}

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RedVault.Collectors.Vault
{
    internal class VaultApi
    {
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


        [DllImport("vaultcli.dll",SetLastError = true)]
        public static extern int VaultFree(IntPtr buffer);

    }
}

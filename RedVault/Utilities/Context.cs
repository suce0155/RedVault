using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;

namespace RedVault.Utilities
{
    internal class Context
    {
        public static string GetLogonName()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            return identity.Name.ToString();

        }
        public static string GetOsVersion()
        {
            return RuntimeInformation.OSDescription;

        }

        public static bool IsAdmin()
        {
            var iden = WindowsIdentity.GetCurrent();
            var prin = new WindowsPrincipal(iden);
            return prin.IsInRole(WindowsBuiltInRole.Administrator);

        }

    }
}

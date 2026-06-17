using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using static RedVault.Modules.m_privileges;


namespace RedVault.Modules
{
    internal class f_privileges
    {
        public static int f_privileges_enable_priv(Privilege privilege)
        {
            int privId = (int)privilege;
            var result = RtlAdjustPrivilege(privId, true, false, out bool status);

            if (result != 0) 
            {
                Console.WriteLine($"RtlAdjustPrivilege failed: {result}");
                return result;
            }
            Console.WriteLine($"[+] Privilege '{privId}' Enabled");
            return result;

        }




    }
}

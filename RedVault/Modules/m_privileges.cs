using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RedVault.Modules
{
    internal class m_privileges
    {
        public enum Privilege : int
        {
            SeCreateToken = 2,
            SeAssignPrimaryToken = 3,
            SeLockMemory = 4,
            SeIncreaseQuota = 5,
            SeTcb = 7,
            SeSecurity = 8,
            SeTakeOwnership = 9,
            SeLoadDriver = 10,
            SeBackup = 17,
            SeRestore = 18,
            SeShutdown = 19,
            SeDebug = 20,
            SeAudit = 21,
            SeSystemEnvironment = 22,
            SeChangeNotify = 23,
            SeRemoteShutdown = 24,
            SeUndock = 25,
            SeSyncAgent = 26,
            SeEnableDelegation = 27,
            SeManageVolume = 28,
            SeImpersonate = 29,
            SeCreateGlobal = 30,
            SeTrustedCredManAccess = 31,
            SeRelabel = 32,
            SeIncreaseWorkingSet = 33,
            SeTimeZone = 34,
            SeCreateSymbolicLink = 35
        }





        [DllImport("ntdll.dll")]
        public static extern int RtlAdjustPrivilege(
            int Privilege,
            bool Enable,
            bool CurrentThread,
            out bool Enabled);
        








    }
}

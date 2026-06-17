using System;
using System.Collections.Generic;
using System.Text;
using static RedVault.Modules.f_privileges;
using static RedVault.Modules.m_privileges;

namespace RedVault.Collectors.Privileges
{
    internal class Privileges
    {
        public static void SeDebugPrivilege()
        {
            f_privileges_enable_priv(Privilege.SeDebug);

        }

        public static void SeLoadDriverPrivilege()
        {
            f_privileges_enable_priv(Privilege.SeLoadDriver);
        }
        public static void SeBackupPrivilege()
        {
            f_privileges_enable_priv(Privilege.SeBackup);
        }

        public static void SeRestorePrivilege()
        {
            f_privileges_enable_priv(Privilege.SeRestore);
        }

        public static void SeSecurityPrivilege()
        {
            f_privileges_enable_priv(Privilege.SeSecurity);
        }
        public static void SeSystemEnvironmentPrivilege()
        {
            f_privileges_enable_priv(Privilege.SeSystemEnvironment);
        }
        public static void SeTcbPrivilege()
        {
            f_privileges_enable_priv(Privilege.SeTcb);
        }

    }
}

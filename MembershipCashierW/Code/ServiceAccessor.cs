using System;

namespace MembershipCashierW.Code
{
    internal static class ServiceAccessor
    {
        private static object _lock = new object();
        private static SecurityDL.Access.ILowLevelAccess securityDb;
        private static MembershipCashierDL.Access.ILowLevelAccess db;

        internal static SecurityDL.Access.ILowLevelAccess SecurityDb
        {
            get
            {
                if (securityDb == null)
                {
                    lock (_lock)
                    {
                        if (securityDb == null)
                        {
                            securityDb = new SecurityDL.Access.LowLevelAccess();
                        }
                    }
                }
                    
                return securityDb;
            }
        }

        internal static MembershipCashierDL.Access.ILowLevelAccess Db
        {
            get
            {
                if (db == null)
                {
                    lock (_lock)
                    {
                        if (db == null)
                        {
                            db = new MembershipCashierDL.Access.LowLevelAccess();
                        }
                    }
                }

                return db;
            }
        }

        internal static void ResetSecurityDb()
        {
            lock (_lock)
            {
                if (securityDb is IDisposable)
                    (securityDb as IDisposable).Dispose();
                securityDb = null;
            }
        }

        internal static void ResetDb()
        {
            lock (_lock)
            {
                if (db is IDisposable)
                    (db as IDisposable).Dispose();
                db = null;
            }
        }

        internal static void Reset()
        {
            ResetDb();
            ResetSecurityDb();
        }
    }
}
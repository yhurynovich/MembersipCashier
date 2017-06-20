using SecurityDL.Access;
using SecurityDL.Authentication;
using SecurityUnified.Interfaces;
using System;

namespace MembershipCashierW.Code.Authentication
{
    public class XRoleProvider : ServiceDrivenRoleProvider, IDisposable
    {
        //private ILowLevelAccess db;

        public override ILowLevelAccess Db
        {
            get
            {
                return ServiceAccessor.SecurityDb;
            }
        }

        public override IHasUserId Requestor
        {
            get
            {
                return SessionGlobal.CurrentUser.Identity;
            }
        }

        public override string ApplicationName
        {
            get
            {
                return System.Reflection.Assembly.GetExecutingAssembly().FullName;
            }
            set
            {
            }
        }

        public void Dispose()
        {
            //if (db != null)
            //{
            //    if (db is IDisposable)
            //        (db as IDisposable).Dispose();
            //    db = null;
            //}
        }
    }
}
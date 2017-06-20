
namespace MembershipCashierDL.Tests
{
    public abstract class TestBase
    {
        private MPSvc.LowLevelForMembershipCashierDLClient db;

        public MPSvc.LowLevelForMembershipCashierDLClient Db
        {
            get
            {
                HostForServices.Instance.StartServices();

                if (db == null)
                    db = new MPSvc.LowLevelForMembershipCashierDLClient();
                return db;
            }
        }

        private SecuritySvc.LowLevelForSecurityDLClient securitydb;

        public SecuritySvc.LowLevelForSecurityDLClient SecurityDb
        {
            get
            {
                HostForServices.Instance.StartServices();

                if (securitydb == null)
                    securitydb = new SecuritySvc.LowLevelForSecurityDLClient();
                return securitydb;
            }
        }
    }
}

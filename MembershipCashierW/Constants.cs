
namespace MembershipCashierW
{
    public static class Constants
    {
        #region Role Definitions
        public const string GUEST = "guest";
        public const string TELLER = "teller";
        public const string OWNER = "owner";
        public const string MARKETING = "marketing";
        public const string ADMIN = "admin";
        public const string SUPER_ADMIN = "super_admin";
        public const string SYSTEM = "system";

        public const string USERS_WITH_PROFILE_ACCESS = "teller, owner, marketing, admin, super_admin, system";
        public const string ALL_ROLES = "guest, teller, owner, marketing, admin, super_admin, system";
        public const string ALL_AUTHENTICATED = "guest, teller, owner, marketing, admin, super_admin, system";
        public const string ALL_MARKETING = "marketing, admin, super_admin, system";
        public const string ALL_ADMINS = "admin, super_admin, system";
        public const string ALL_EMPLOYEES = "owner, teller, admin, super_admin, system";

        public static string[] ALL_MARKETING_AsArray = new string[] { "marketing", "admin", "super_admin", "system" };

        #endregion
    }
}
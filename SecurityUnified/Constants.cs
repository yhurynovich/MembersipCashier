namespace SecurityUnified
{
    public static class Constants
    {
        public const int LOGIN_RETRY_COUNT = 3;

        #region Role Definitions
        public const string SYSTEM = "system";
        public const string SUPER_ADMIN = "super_admin";
        public const string ALL_SUPER_ADMINS = "system, super_admin";
        public const string ADMIN = "admin";
        public const string ALL_ADMINS = "admin, super_admin";
        #endregion
    }
}

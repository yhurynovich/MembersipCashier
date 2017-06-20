namespace MembershipCashierUnified
{
    public static class Constants
    {
        #region Postal Code Validation
        public const string POSTAL_CODE_BR = @"^\d{8}$";
        public const string POSTAL_CODE_JP = @"^\d{7}$";
        public const string POSTAL_CODE_CN = @"^\d{6}$";
        public const string POSTAL_CODE_CS = @"^\d{5}$";
        public const string POSTAL_CODE_AU = @"^\d{4}$";
        public const string POSTAL_CODE_CA = @"^\w\d\w ?\d\w\d$";
        public const string POSTAL_CODE_USA = @"^\d{5}[-\s]?(\d{4})?$";
        public const string POSTAL_CODE_UK = @"^[a-zA-Z\d ]+$"; // @"^(([gG][iI][rR] {0,}0[aA]{2})|((([a-pr-uwyzA-PR-UWYZ][a-hk-yA-HK-Y]?[0-9][0-9]?)|(([a-pr-uwyzA-PR-UWYZ][0-9][a-hjkstuwA-HJKSTUW])|([a-pr-uwyzA-PR-UWYZ][a-hk-yA-HK-Y][0-9][abehmnprv-yABEHMNPRV-Y]))) {0,}[0-9][abd-hjlnp-uw-zABD-HJLNP-UW-Z]{2}))$";
        public const string POSTAL_CODE_FREEFORM = @"^.+$";
        #endregion

        public const string PO_BOX_REGEX = @"[^\w]?p[\.\s]?o\.?[\s]+box[\s]+[\d]+";
        public const string LANGUAGE_REGEX = @"^\w{2}(-\w{2})?$";
        public const string LANGUAGE_ISO2_REGEX = @"^\w{2}$";
    }
}

using MembershipCashierUnified.Interfaces;

namespace MembershipCashierUnified
{
    public static class Extentions
    {
        public static void Uppercase(this IIAddress a)
        {
            if (a == null)
                return;

            if (a.Address1 != null)
                a.Address1 = a.Address1.ToUpper();
            if (a.City != null)
                a.City = a.City.ToUpper();
            if (a.Country != null)
                a.Country = a.Country.ToUpper();
            if (a.PostalCode != null)
                a.PostalCode = a.PostalCode.ToUpper();
            if (a.Province != null)
                a.Province = a.Province.ToUpper();
        }
    }
}

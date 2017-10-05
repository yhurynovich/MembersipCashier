using htm.paymentProcessing.core.Interfaces;

namespace htm.paymentProcessing.core
{
    public static class Extentions_CopyTo
    {
        public static void CopyTo(this IAddress from, IAddress to, bool allowDefaultValues = true)
        {
            if (from == null || to == null)
                return;

            if (allowDefaultValues || !string.IsNullOrWhiteSpace(from.Address1))
                to.Address1 = from.Address1;
            if (allowDefaultValues || !string.IsNullOrWhiteSpace(from.City))
                to.City = from.City;
            if (allowDefaultValues || !string.IsNullOrWhiteSpace(from.Country))
                to.Country = from.Country;
            if (allowDefaultValues || !string.IsNullOrWhiteSpace(from.PostalCode))
                to.PostalCode = from.PostalCode;
            if (allowDefaultValues || !string.IsNullOrWhiteSpace(from.Province))
                to.Province = from.Province;
        }
    }
}

using MembershipCashierDL.DB.Repository.CustomRepository;
using MembershipCashierUnified.Interfaces;
using SecurityDL.DB.Repository;

namespace MembershipCashierDL.DB.Repository
{
    internal static class RepositoryFactory
    {
        public static AddressRepositoryEntitySet GetAddress()
        {
            return new AddressRepositoryEntitySet();
        }

        public static RepositoryEntitySet<CreditTransaction, ICreditTransaction> GetCreditTransaction()
        {
            return new RepositoryEntitySet<CreditTransaction, ICreditTransaction>();
        }

        public static LocationRepositoryEntitySet GetLocation()
        {
            return new LocationRepositoryEntitySet();
        }

        public static OwnerVsLocationRepositoryEntitySet GetOwnerVsLocation()
        {
            return new OwnerVsLocationRepositoryEntitySet();
        }

        public static OwnerRepositoryEntitySet GetOwner()
        {
            return new OwnerRepositoryEntitySet();
        }

        public static ProfileCreditRepositoryEntitySet GetProfileCredit()
        {
            return new ProfileCreditRepositoryEntitySet();
        }

        public static ProductRepositoryEntitySet GetProduct()
        {
            return new ProductRepositoryEntitySet();
        }

        public static ProductPriceHistoryEntitySet GetProductPriceHistory()
        {
            return new ProductPriceHistoryEntitySet();
        }

        public static UserProfileRepositoryEntitySet GetUserProfile()
        {
            return new UserProfileRepositoryEntitySet();
        }
    }
}

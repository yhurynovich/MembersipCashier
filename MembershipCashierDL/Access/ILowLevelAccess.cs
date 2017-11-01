using MembershipCashierDL.MixedContracts;
using MembershipCashierUnified.Contracts;
using SecurityUnified.Contracts;
using System.Collections.Generic;
using System.ServiceModel;

namespace MembershipCashierDL.Access
{
    [ServiceContract]
    public interface ILowLevelAccess
    {
        #region Find
        [OperationContract]
        AddressContract[] FindAddress(AddressDiscriminator d);
        [OperationContract]
        CreditTransactionContract[] FindCreditTransaction(CreditTransactionDiscriminator d);
        [OperationContract]
        LocationContract[] FindLocation(LocationDiscriminator d, OwnerVsLocationDiscriminator o, UserProfileDiscriminator u);
        [OperationContract]
        OwnerVsLocationContract[] FindOwnerVsLocation(OwnerVsLocationDiscriminator o, LocationDiscriminator l, UserProfileDiscriminator p);
        [OperationContract]
        OwnerContract[] FindOwner(OwnerDiscriminator d, LocationDiscriminator l);
        [OperationContract]
        ProductCreditContract[] FindProfileCredit(ProfileCreditDiscriminator d);
        [OperationContract]
        ProductCreditContract[] FindProfileCredit2(ProductDiscriminator p, ProfileCreditDiscriminator d, LocationDiscriminator l);
        /// <summary>
        /// Locates users by combining boths filters
        /// </summary>
        /// <param name="d"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        [OperationContract]
        UserProfile2Contract[] FindUserProfile(UserProfile2Discriminator d, UserProfileVsLocationDiscriminator d2, RoleDiscriminator r);
        [OperationContract]
        ProductContract[] FindProduct(ProductDiscriminator d, LocationDiscriminator l, ProfileCreditDiscriminator c, int userId);
        [OperationContract]
        ProductAndTransactionsContract[] FindProductLastTransactions(ProductDiscriminator d, LocationDiscriminator l, ProfileCreditDiscriminator c, int trnCntByProduct);
        [OperationContract]
        ProductPriceHistoryContract[] FindProductPriceHistory(ProductDiscriminator p, bool latestPriceOnly);
        [OperationContract]
        UserAndProductsContarct[] FindLastUsedProducts(UserProfile2Discriminator u, UserProfileVsLocationDiscriminator pvl);
        #endregion

        #region Update

        [OperationContract]
        void UpdateAddress(AddressContract[] d, bool allowDefaultValues = true);

        [OperationContract]
        void UpdateProduct(ProductContract[] d, bool allowDefaultValues = true);

        [OperationContract]
        void UpdateOwnerVsLocation(OwnerVsLocationContract[] d, bool allowDefaultValues = true);

        [OperationContract]
        void UpdateLocation(LocationContract[] d, bool allowDefaultValues = true);

        #endregion

        #region Insert
        [OperationContract]
        long[] InsertAddress(params AddressContract[] d);

        [OperationContract]
        void InsertProduct(Product_Location_Price_Contract[] d);

        [OperationContract]
        void InsertProductPriceHistory(ProductPriceHistoryContract[] d);

        [OperationContract]
        ProfileCreditContract[] InsertCreditTransaction(CreditTransactionContract[] d);

        [OperationContract]
        int[] InsertLocation(params LocationContract[] d);

        [OperationContract]
        void InsertOwner(params OwnerContract[] d);

        [OperationContract]
        void InsertOwnerVsLocation(params OwnerVsLocationContract[] d);

        [OperationContract]
        KeyValuePair<long, short>[] InsertPayment(params PaymentContract[] d);
        #endregion

        #region InsertOrUpdate

        [OperationContract]
        void InsertOrUpdateProfileCredit(ProfileCreditContract[] d, bool allowDefaultValues = true);

        #endregion
    }
}

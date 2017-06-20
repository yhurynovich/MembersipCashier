using MembershipCashierUnified.Contracts;
using MembershipCashierUnified.Interfaces;
using SecurityDL.DB.Repository;
using SecurityUnified.Contracts;
using System.Linq;

namespace MembershipCashierDL.DB.Repository.CustomRepository
{
    internal class ProductRepositoryEntitySet : RepositoryEntitySet<DB.Product, IProduct>
    {
        public ProfileCreditDiscriminator ProfileCreditFilter { get; set; }

        public UserProfileDiscriminator UserProfileFilter { get; set; }

        public LocationDiscriminator LocationFilter { get; set; }

        public bool ShowRecordsWithBallanceFirst { get; set; }

        public CreditTransactionDiscriminator CreditTransactionFilter
        {
            get; set;
        }

        public override IQueryable<IProduct> Result
        {
            get
            {
                if (result == null)
                {
                    IQueryable<DB.Product> newResult = GetTable<DB.Product>();

                    if (ProfileCreditFilter != null)
                    {
                        IQueryable<DB.ProfileCredit> profileCredits = GetTable<DB.ProfileCredit>();

                        if (ProfileCreditFilter.Filter != null)
                            profileCredits = profileCredits.Where(ProfileCreditFilter.Filter).OfType<DB.ProfileCredit>();

                        profileCredits = profileCredits.Skip(ProfileCreditFilter.Skip).Take(ProfileCreditFilter.Take == default(int) ? DEFAULT_MAX_RECORDS : ProfileCreditFilter.Take);

                        newResult = profileCredits.Join(
                            newResult,
                            profileCredit => profileCredit.ProductId,
                            product => product.ProductId,
                            (profileCredit, product) => product
                        );
                    }

                    if (LocationFilter != null)
                    {
                        IQueryable<DB.Location> locations = GetTable<DB.Location>();

                        if (LocationFilter.Filter != null)
                            locations = locations.Where(LocationFilter.Filter).OfType<DB.Location>();

                        locations = locations.Skip(LocationFilter.Skip).Take(LocationFilter.Take == default(int) ? DEFAULT_MAX_RECORDS : LocationFilter.Take);

                        var prodVsLocations = locations.SelectMany(l => l.ProductVsLocations);

                        newResult = prodVsLocations.Join(
                            newResult,
                            prodVsLocation => prodVsLocation.ProductId,
                            product => product.ProductId,
                            (prodVsLocation, product) => product
                        ).Distinct();
                    }

                    if (CreditTransactionFilter != null)
                    {
                        IQueryable<DB.CreditTransaction> creditTransactions = GetTable<DB.CreditTransaction>();

                        if (CreditTransactionFilter.Filter != null)
                            creditTransactions = creditTransactions.Where(CreditTransactionFilter.Filter).OfType<DB.CreditTransaction>();

                        creditTransactions = creditTransactions.Skip(CreditTransactionFilter.Skip).Take(CreditTransactionFilter.Take == default(int) ? DEFAULT_MAX_RECORDS : CreditTransactionFilter.Take);

                        newResult = creditTransactions.Join(
                            newResult,
                            creditTransaction => creditTransaction.ProductId,
                            product => product.ProductId,
                            (creditTransaction, product) => product
                        );
                    }

                    if (UserProfileFilter != null)
                    {
                        IQueryable<DB.UserProfile> userProfiles = GetTable<DB.UserProfile>();

                        if (UserProfileFilter.Filter != null)
                            userProfiles = userProfiles.Where(UserProfileFilter.Filter).OfType<DB.UserProfile>();

                        userProfiles = userProfiles.Skip(UserProfileFilter.Skip).Take(UserProfileFilter.Take == default(int) ? DEFAULT_MAX_RECORDS : UserProfileFilter.Take);

                        var profileCredits = userProfiles.Join(
                            GetTable<DB.ProfileCredit>(),
                            userProfile => userProfile.UserId,
                            profileCredit => profileCredit.UserId,
                            (userProfile, profileCredit) => profileCredit
                        );

                        newResult = profileCredits.Join(
                            newResult,
                            profileCredit => profileCredit.ProductId,
                            product => product.ProductId,
                            (profileCredit, product) => product
                        );
                    }

                    if (ShowRecordsWithBallanceFirst)
                    {
                        newResult = newResult.OrderByDescending(x=>x.ProfileCredits.Any(c=>c.HasBallance.Value));
                    }

                    if (discriminator != null)
                    {
                        if (discriminator.Filter != null)
                            newResult = newResult.Where(discriminator.Filter).OfType<DB.Product>();

                        if (discriminator.OrderBy != null)
                        {
                            foreach (var oby in discriminator.OrderBy)
                            {
                                newResult = OrderByHelper(newResult, oby.FieldName, oby.IsDescending);
                            }
                        }

                        newResult = newResult.Skip(discriminator.Skip).Take(discriminator.Take);
                    }
                    else
                    {
                        newResult = newResult.Take(DEFAULT_MAX_RECORDS);
                    }

                    result = newResult;
                }


                return result;
            }
        }
    }
}

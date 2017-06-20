using MembershipCashierUnified.Contracts;
using MembershipCashierUnified.Interfaces;
using SecurityDL.DB.Repository;
using SecurityUnified.Contracts;
using System.Linq;

namespace MembershipCashierDL.DB.Repository.CustomRepository
{
    internal class ProfileCreditRepositoryEntitySet : RepositoryEntitySet<DB.ProfileCredit, IProfileCredit>
    {
        public ProductDiscriminator ProductFilter { get; set; }

        public UserProfileDiscriminator UserProfileFilter { get; set; }

        public LocationDiscriminator LocationFilter { get; set; }

        public override IQueryable<IProfileCredit> Result
        {
            get
            {
                if (result == null)
                {
                    IQueryable<DB.ProfileCredit> newResult = GetTable<DB.ProfileCredit>();

                    if (ProductFilter != null)
                    {
                        IQueryable<DB.Product> products = GetTable<DB.Product>();

                        if (ProductFilter.Filter != null)
                            products = products.Where(ProductFilter.Filter).OfType<DB.Product>();

                        products = products.Skip(ProductFilter.Skip).Take(ProductFilter.Take == default(int) ? DEFAULT_MAX_RECORDS : ProductFilter.Take);

                        newResult = products.Join(
                            newResult,
                            profileCredit => profileCredit.ProductId,
                            product => product.ProductId,
                            (product, profileCredit) => profileCredit
                        );
                    }

                    if (UserProfileFilter != null)
                    {
                        IQueryable<DB.UserProfile> userProfiles = GetTable<DB.UserProfile>();

                        if (UserProfileFilter.Filter != null)
                            userProfiles = userProfiles.Where(UserProfileFilter.Filter).OfType<DB.UserProfile>();

                        userProfiles = userProfiles.Skip(UserProfileFilter.Skip).Take(UserProfileFilter.Take == default(int) ? DEFAULT_MAX_RECORDS : UserProfileFilter.Take);

                        newResult = userProfiles.Join(
                            newResult,
                            profileCredit => profileCredit.UserId,
                            userProfile => userProfile.UserId,
                            (userProfile, profileCredit) => profileCredit
                        );
                    }

                    if (LocationFilter != null)
                    {
                        IQueryable<DB.Location> locations = GetTable<DB.Location>();

                        if (LocationFilter.Filter != null)
                            locations = locations.Where(LocationFilter.Filter).OfType<DB.Location>();

                        locations = locations.Skip(LocationFilter.Skip).Take(LocationFilter.Take == default(int) ? DEFAULT_MAX_RECORDS : LocationFilter.Take);

                        newResult = locations.Join(
                            newResult,
                            location => location.LocationId,
                            profileCredit => profileCredit.LocationId,
                            (userProfile, profileCredit) => profileCredit
                        );
                    }

                    if (discriminator != null)
                    {
                        if (discriminator.Filter != null)
                            newResult = newResult.Where(discriminator.Filter).OfType<DB.ProfileCredit>();

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

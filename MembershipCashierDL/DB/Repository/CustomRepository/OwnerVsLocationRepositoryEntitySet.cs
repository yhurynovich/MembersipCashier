using MembershipCashierUnified.Contracts;
using MembershipCashierUnified.Interfaces;
using SecurityDL.DB.Repository;
using SecurityUnified.Contracts;
using System.Linq;

namespace MembershipCashierDL.DB.Repository.CustomRepository
{
    internal class OwnerVsLocationRepositoryEntitySet : RepositoryEntitySet<DB.OwnerVsLocation, IOwnerVsLocation>
    {
        public LocationDiscriminator LocationFilter { get; set; }

        public UserProfileDiscriminator UserProfileFilter { get; set; }

        public override IQueryable<IOwnerVsLocation> Result
        {
            get
            {
                if (result == null)
                {
                    IQueryable<DB.OwnerVsLocation> newResult = GetTable<DB.OwnerVsLocation>();

                    if (LocationFilter != null)
                    {
                        IQueryable<DB.Location> locations = GetTable<DB.Location>();

                        if (LocationFilter.Filter != null)
                            locations = locations.Where(LocationFilter.Filter).OfType<DB.Location>();

                        locations = locations.Skip(LocationFilter.Skip).Take(LocationFilter.Take == default(int) ? DEFAULT_MAX_RECORDS : LocationFilter.Take);

                        newResult = locations.Join(
                            newResult,
                            location => location.LocationId,
                            ownerVsLocation => ownerVsLocation.LocationId,
                            (location, ownerVsLocation) => ownerVsLocation
                        );
                    }

                    //if (UserProfileFilter != null)
                    //{
                    //    IQueryable<DB.UserProfile> userProfiles = GetTable<DB.UserProfile>();

                    //    if (UserProfileFilter.Filter != null)
                    //        userProfiles = userProfiles.Where(UserProfileFilter.Filter).OfType<DB.UserProfile>();

                    //    userProfiles = userProfiles.Skip(UserProfileFilter.Skip).Take(UserProfileFilter.Take == default(int) ? DEFAULT_MAX_RECORDS : UserProfileFilter.Take);

                    //    newResult = userProfiles.Join(
                    //        newResult,
                    //        userProfile => userProfile.UserId,
                    //        location => location.LocationId,
                    //        (userProfile, location) => location
                    //    );
                    //}

                    if (discriminator != null)
                    {
                        if (discriminator.Filter != null)
                            newResult = newResult.Where(discriminator.Filter).OfType<DB.OwnerVsLocation>();

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

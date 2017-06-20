using MembershipCashierUnified.Contracts;
using MembershipCashierUnified.Interfaces;
using SecurityDL.DB.Repository;
using SecurityUnified.Contracts;
using System.Linq;

namespace MembershipCashierDL.DB.Repository.CustomRepository
{
    internal class OwnerRepositoryEntitySet : RepositoryEntitySet<DB.Owner, IOwner>
    {
        public LocationDiscriminator LocationFilter { get; set; }

        public UserProfileDiscriminator UserProfileFilter { get; set; }

        public override IQueryable<IOwner> Result
        {
            get
            {
                if (result == null)
                {
                    IQueryable<DB.Owner> newResult = GetTable<DB.Owner>();

                    if (LocationFilter != null)
                    {
                        IQueryable<DB.Location> locations = GetTable<DB.Location>();

                        if (LocationFilter.Filter != null)
                            locations = locations.Where(LocationFilter.Filter).OfType<DB.Location>();

                        locations = locations.Skip(LocationFilter.Skip).Take(LocationFilter.Take == default(int) ? DEFAULT_MAX_RECORDS : LocationFilter.Take);

                        var ownerLocations = locations.Join(
                            GetTable<DB.OwnerVsLocation>(),
                            location => location.LocationId,
                            ownerVsLocation => ownerVsLocation.LocationId,
                            (location, ownerVsLocation) => ownerVsLocation
                        );

                        newResult = ownerLocations.Join(
                            newResult,
                            ownerVsLocation => ownerVsLocation.OwnerId,
                            owner => owner.OwnerId,
                            (location, owner) => owner
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
                            userProfile => userProfile.UserId,
                            owner => owner.OwnerUserId,
                            (userProfile, owner) => owner
                        );
                    }

                    if (discriminator != null)
                    {
                        if (discriminator.Filter != null)
                            newResult = newResult.Where(discriminator.Filter).OfType<DB.Owner>();

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

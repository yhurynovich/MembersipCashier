using MembershipCashierUnified.Contracts;
using MembershipCashierUnified.Interfaces;
using SecurityDL.DB.Repository;
using SecurityUnified.Contracts;
using System.Linq;

namespace MembershipCashierDL.DB.Repository.CustomRepository
{
    internal class LocationRepositoryEntitySet : RepositoryEntitySet<DB.Location, ILocation>
    {
        public OwnerVsLocationDiscriminator OwnerVsLocationFilter { get; set; }

        public UserProfileDiscriminator UserProfileFilter { get; set; }

        public override IQueryable<ILocation> Result
        {
            get
            {
                if (result == null)
                {
                    IQueryable<DB.Location> newResult = GetTable<DB.Location>();

                    if (OwnerVsLocationFilter != null)
                    {
                        IQueryable<DB.OwnerVsLocation> ownerVsLocations = GetTable<DB.OwnerVsLocation>();

                        if (OwnerVsLocationFilter.Filter != null)
                            ownerVsLocations = ownerVsLocations.Where(OwnerVsLocationFilter.Filter).OfType<DB.OwnerVsLocation>();

                        ownerVsLocations = ownerVsLocations.Skip(OwnerVsLocationFilter.Skip).Take(OwnerVsLocationFilter.Take == default(int) ? DEFAULT_MAX_RECORDS : OwnerVsLocationFilter.Take);

                        newResult = ownerVsLocations.Join(
                            newResult,
                            ownerVsLocation => ownerVsLocation.LocationId,
                            location => location.LocationId,
                            (ownerVsLocation, location) => location
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
                            location => location.LocationId,
                            (userProfile, location) => location
                        );
                    }

                    if (discriminator != null)
                    {
                        if (discriminator.Filter != null)
                            newResult = newResult.Where(discriminator.Filter).OfType<DB.Location>();

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

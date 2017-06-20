using MembershipCashierUnified.Contracts;
using MembershipCashierUnified.Interfaces;
using SecurityUnified.Contracts;
using SecurityUnified.Interfaces;
using System;
using System.Linq;

namespace MembershipCashierDL.DB.Repository.CustomRepository
{
    public class UserProfileRepositoryEntitySet : SecurityDL.DB.Repository.CustomRepository.UserProfileRepositoryEntitySet
    {
        public UserProfileVsLocationDiscriminator UserProfileVsLocationFilter { get; set; }

        public DataDiscriminator<IUserProfile2> Discriminator2 { get; set; }

        public override DataDiscriminator<IUserProfile> Discriminator
        {
            get
            {
                throw new NotImplementedException("Please use Discriminator2");
            }
            set
            {
                throw new NotImplementedException("Please use Discriminator2");
            }
        }

        public override IQueryable<IUserProfile> Result
        {
            get
            {
                if (result == null)
                {
                    IQueryable<DB.UserProfile> newResult = GetTable<DB.UserProfile>();

                    if (RoleFilter != null)
                    {
                        IQueryable<SecurityDL.DB.webpages_Role> roles = GetTable<SecurityDL.DB.webpages_Role>();
                        if (RoleFilter.Filter != null)
                        {
                            roles = roles.Where(RoleFilter.Filter).OfType<SecurityDL.DB.webpages_Role>();
                        }
                        roles = roles.Skip(RoleFilter.Skip).Take(RoleFilter.Take == default(int) ? DEFAULT_MAX_RECORDS : RoleFilter.Take);

                        var usersInRoles = roles.Join(GetTable<SecurityDL.DB.webpages_UsersInRole>(),
                                role => role.RoleId,
                                uir => uir.RoleId,
                                (role, uir) => uir
                            );

                        newResult = newResult.Join(
                            usersInRoles,
                            profile => profile.UserId,
                            uir => uir.UserId,
                            (profile, uir) => profile
                        );
                    }

                    if (UserProfileVsLocationFilter != null)
                    {
                        IQueryable<DB.UserProfileVsLocation> userProfileVsLocations = GetTable<DB.UserProfileVsLocation>();

                        if (UserProfileVsLocationFilter.Filter != null)
                            userProfileVsLocations = userProfileVsLocations.Where(UserProfileVsLocationFilter.Filter).OfType<DB.UserProfileVsLocation>();

                        userProfileVsLocations = userProfileVsLocations.Skip(UserProfileVsLocationFilter.Skip).Take(UserProfileVsLocationFilter.Take == default(int) ? DEFAULT_MAX_RECORDS : UserProfileVsLocationFilter.Take);

                        newResult = userProfileVsLocations.Join(
                            newResult,
                            userProfile => userProfile.UserId,
                            userProfileVsLocation => userProfileVsLocation.UserId,
                            (userProfileVsLocation, userProfile) => userProfile
                        );
                    }

                    if (Discriminator2 != null)
                    {
                        if (Discriminator2.Filter != null)
                            newResult = newResult.Where(Discriminator2.Filter).OfType<DB.UserProfile>();

                        result = newResult.Skip(Discriminator2.Skip).Take(Discriminator2.Take);
                    }
                    else
                        result = newResult.Take(DEFAULT_MAX_RECORDS);

                }

                return result;
            }
        }

        public IQueryable<IUserProfile> Result2 {
            get {
                return Result.OfType<IUserProfile2>();
            }
        }
    }
}

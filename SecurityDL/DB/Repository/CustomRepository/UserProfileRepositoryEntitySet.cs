using SecurityUnified.Contracts;
using SecurityUnified.Interfaces;
using System.Linq;

namespace SecurityDL.DB.Repository.CustomRepository
{
    public class UserProfileRepositoryEntitySet : RepositoryEntitySet<DB.UserProfile, IUserProfile>
    {
        public RoleDiscriminator RoleFilter
        {
            get; set;
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
                        IQueryable<DB.webpages_Role> roles = GetTable<DB.webpages_Role>();
                        if (RoleFilter.Filter != null)
                        {
                            roles = roles.Where(RoleFilter.Filter).OfType<DB.webpages_Role>();
                        }
                        roles = roles.Skip(RoleFilter.Skip).Take(RoleFilter.Take == default(int) ? DEFAULT_MAX_RECORDS : RoleFilter.Take);

                        var usersInRoles = roles.Join(GetTable<DB.webpages_UsersInRole>(),
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

                    if (discriminator != null)
                    {
                        if (discriminator.Filter != null)
                            newResult = newResult.Where(discriminator.Filter).OfType<DB.UserProfile>();

                        result = newResult.Skip(discriminator.Skip).Take(discriminator.Take);
                    }
                    else
                        result = newResult.Take(DEFAULT_MAX_RECORDS);

                }
                return result;
            }
        }
    }
}

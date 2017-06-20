using MembershipCashierUnified.Interfaces;
using SecurityDL.DB.Repository;
using System.Linq;

namespace MembershipCashierDL.DB.Repository.CustomRepository
{
    internal class AddressRepositoryEntitySet : RepositoryEntitySet<DB.Address, IAddress>
    {
        //public UserProfileDiscriminator UserProfileFilter
        //{
        //    get; set;
        //}

        public override IQueryable<IAddress> Result
        {
            get
            {
                if (result == null)
                {
                    IQueryable<DB.Address> newResult = GetTable<DB.Address>();

                    //if (UserProfileFilter != null)
                    //{
                    //    IQueryable<DB.UserProfile> profiles = GetTable<DB.UserProfile>();
                    //    if (UserProfileFilter.Filter != null)
                    //    {
                    //        profiles = profiles.Where(UserProfileFilter.Filter).OfType<DB.UserProfile>();
                    //    }
                    //    profiles = profiles.Skip(UserProfileFilter.Skip).Take(UserProfileFilter.Take == default(int) ? DEFAULT_MAX_RECORDS : UserProfileFilter.Take);

                    //    var userVsAddrs = profiles.Join(GetTable<DB.UserVsAddress>(),
                    //            profile => profile.UserId,
                    //            userVsAddr => userVsAddr.UserId,
                    //            (profile, userVsAddr) => userVsAddr
                    //        );

                    //    newResult = newResult.Join(
                    //        userVsAddrs,
                    //        address => address.AddressId,
                    //        userVsAddr => userVsAddr.AddressId,
                    //        (address, userVsAddr) => address
                    //    );
                    //}

                    if (discriminator != null)
                    {
                        if (discriminator.Filter != null)
                            newResult = newResult.Where(discriminator.Filter).OfType<DB.Address>();

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

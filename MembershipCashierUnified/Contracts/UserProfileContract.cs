using MembershipCashierUnified.Interfaces;
using SecurityUnified.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MembershipCashierUnified.Contracts
{
    [DataContract]
    public class UserProfileContract : IHasUserProfile
    {
        [DataMember(Name = "_userProfileContract")]
        private UserProfileImplementor _userProfileContract;

        public IUserProfile UserProfile
        {
            get { return (_userProfileContract as IUserProfile); }
            set { var x = new UserProfileImplementor(); value.CopyTo(x); _userProfileContract = x; }
        }
    }
}

using MembershipCashierUnified.Interfaces;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace MembershipCashierUnified.Contracts
{
    [DataContract]
    public class UserProfileWithLDAPContract : IHasUserProfileWithLDAP
    {
        [DataMember(Name = "UserProfileWithLDAP")]
        private UserProfileWithLDAPImplementor _userProfileWithLDAP;

        [JsonIgnore]
        public IUserProfileWithLDAP UserProfileWithLDAP
        {
            get { return (_userProfileWithLDAP as IUserProfileWithLDAP); }
            set { var x = new UserProfileWithLDAPImplementor(); value.CopyTo(x); _userProfileWithLDAP = x; }
        }
    }
}

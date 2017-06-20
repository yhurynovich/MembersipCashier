using MembershipCashierUnified.Interfaces;
using SecurityUnified.Contracts;
using System.Runtime.Serialization;

namespace MembershipCashierUnified.Contracts
{
    [DataContract]
    public class UserProfile2Implementor : UserProfileImplementor, IUserProfile2
    {
        [DataMember]
        public string LdapAccount { get; set; }

        [DataMember]
        public System.Data.Linq.Binary Photo { get; set; }
    }
}

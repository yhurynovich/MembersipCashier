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
        public string PersonalId { get; set; }

        [DataMember]
        public string Photo { get; set; }
    }
}

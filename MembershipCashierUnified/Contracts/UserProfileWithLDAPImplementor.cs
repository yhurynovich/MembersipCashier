using MembershipCashierUnified.Interfaces;
using SecurityUnified.Contracts;
using System;
using System.Runtime.Serialization;

namespace MembershipCashierUnified.Contracts
{
    [Serializable]
    [DataContract]
    public class UserProfileWithLDAPImplementor : UserProfileImplementor, IUserProfileWithLDAP
    {
        [DataMember]
        public string LdapAccount { get; set; }
    }
}

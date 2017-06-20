using MembershipCashierUnified.Interfaces;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace MembershipCashierUnified.Contracts
{
    [DataContract]
    public class ProfileCreditContract : IHasProfileCredit
    {
        [DataMember(Name = "ProfileCredit")]
        private ProfileCreditImplementor _profileCredit;

        [JsonIgnore]
        public IProfileCredit ProfileCredit
        {
            get { return (_profileCredit as IProfileCredit); }
            set { var x = new ProfileCreditImplementor(); value.CopyTo(x); _profileCredit = x; }
        }
    }
}

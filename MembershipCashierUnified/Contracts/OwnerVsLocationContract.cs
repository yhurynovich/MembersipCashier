using MembershipCashierUnified.Interfaces;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace MembershipCashierUnified.Contracts
{
    [DataContract]
    public class OwnerVsLocationContract : IHasOwnerVsLocation
    {
        [DataMember(Name = "OwnerVsLocation")]
        private OwnerVsLocationImplementor _ownerVsLocation;

        [JsonIgnore]
        public IOwnerVsLocation OwnerVsLocation
        {
            get { return (_ownerVsLocation as IOwnerVsLocation); }
            set { var x = new OwnerVsLocationImplementor(); value.CopyTo(x); _ownerVsLocation = x; }
        }
    }
}

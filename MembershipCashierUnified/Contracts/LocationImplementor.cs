using MembershipCashierUnified.Interfaces;
using System.Runtime.Serialization;

namespace MembershipCashierUnified.Contracts
{
    [DataContract]
    public class LocationImplementor : ILocation
    {
        [DataMember]
        public int LocationId { get; set; }
        [DataMember]
        public string LocationCode { get; set; }
        [DataMember]
        public long? AddressId { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string TimeZoneCode { get; set; }
        [DataMember]
        public bool IsCredeitReversed { get; set; }
    }
}

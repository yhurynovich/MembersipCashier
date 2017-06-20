using System.Runtime.Serialization;

namespace MembershipCashierUnified.Interfaces
{
    public interface ILocation : IHasLocationId, IMayHaveAddressId, IHasDescription
    {
        [DataMember]
        string LocationCode { get; set; }
        [DataMember]
        string TimeZoneCode { get; set; }
        [DataMember]
        bool IsCredeitReversed { get; set; }
    }
}

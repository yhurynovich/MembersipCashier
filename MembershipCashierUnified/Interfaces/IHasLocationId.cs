using System.Runtime.Serialization;

namespace MembershipCashierUnified.Interfaces
{
    public interface IHasLocationId
    {
        [DataMember]
        int LocationId { get; set; }
    }
}

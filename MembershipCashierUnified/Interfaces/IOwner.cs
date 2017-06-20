using SecurityUnified.Interfaces;
using System.Runtime.Serialization;

namespace MembershipCashierUnified.Interfaces
{
    public interface IOwner : IHasUserId, IHasOwnerId
    {
        [DataMember]
        int OwnerId { get; set; }
        [DataMember]
        int? OwnerUserId { get; set; }
    }
}

using System.Runtime.Serialization;

namespace MembershipCashierUnified.Interfaces
{
    public interface IHasOwnerId
    {
        [DataMember]
        int OwnerId { get; set; }
    }
}

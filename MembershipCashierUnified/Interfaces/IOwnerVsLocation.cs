using System.Runtime.Serialization;

namespace MembershipCashierUnified.Interfaces
{
    public interface IOwnerVsLocation : IHasOwnerId, IHasLocationId
    {
        bool IsCurrent { get; set; }
    }
}

using System.Runtime.Serialization;

namespace MembershipCashierUnified.Interfaces
{
    public interface IHasProductId
    {
        [DataMember]
        int ProductId { get; set; }
    }
}

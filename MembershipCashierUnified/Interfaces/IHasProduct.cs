using System.Runtime.Serialization;

namespace MembershipCashierUnified.Interfaces
{
    public interface IHasProduct
    {
        [DataMember]
        IProduct Product { get; set; }
    }
}

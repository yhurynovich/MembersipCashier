using MembershipCashierUnified.Interfaces;
using System.Runtime.Serialization;

namespace MembershipCashierUnified.Contracts
{
    [DataContract]
    public class ProductCoreImplementor : IProductCore
    {
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int ProductId { get; set; }
    }
}

using MembershipCashierUnified.Interfaces;
using System.Runtime.Serialization;

namespace MembershipCashierUnified.Contracts
{
    [DataContract]
    public class PaymentImplementor : IPayment
    {
        [DataMember]
        public decimal? Amount { get; set; }
        [DataMember]
        public string Currency { get; set; }
        [DataMember]
        public long PaymentId { get; set; }
        [DataMember]
        public char PaymentMethod { get; set; }
        [DataMember]
        public short Sequence { get; set; }
    }
}

using MembershipCashierUnified.Interfaces;
using System.Runtime.Serialization;

namespace MembershipCashierUnified.Contracts
{
    [DataContract]
    public class CreditTransactionImplementor : ICreditTransaction
    {
        [DataMember]
        public long CreditTransactionId { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public int ProductId { get; set; }
        [DataMember]
        public System.DateTime TransactionTime { get; set; }
        [DataMember]
        public int LocationId { get; set; }
        [DataMember]
        public decimal BallanceUnits { get; set; }
        [DataMember]
        public string Description { get; set; }
    }
}

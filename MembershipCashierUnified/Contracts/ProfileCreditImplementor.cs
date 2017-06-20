using MembershipCashierUnified.Interfaces;
using System.Runtime.Serialization;
using System;

namespace MembershipCashierUnified.Contracts
{
    [DataContract]
    public class ProfileCreditImplementor : IProfileCredit
    {
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public int ProductId { get; set; }
        [DataMember]
        public int LocationId { get; set; }
        [DataMember]
        public DateTime CalculatedTime { get; set; }
        [DataMember]
        public decimal Ballance { get; set; }
        [DataMember]
        public decimal BallanceUnits { get; set; }

        public bool? HasBallance
        {
            get
            {
                return Ballance != 0;
            }
        }
    }
}

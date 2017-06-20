using MembershipCashierUnified.Interfaces;
using System.Runtime.Serialization;
using System;

namespace MembershipCashierUnified.Contracts
{
    [DataContract]
    public class OwnerVsLocationImplementor : IOwnerVsLocation
    {
        [DataMember]
        public int OwnerId { get; set; }
        [DataMember]
        public int LocationId { get; set; }
        [DataMember]
        public bool IsCurrent { get; set; }
    }
}

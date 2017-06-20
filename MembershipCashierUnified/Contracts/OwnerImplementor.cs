using MembershipCashierUnified.Interfaces;
using System.Runtime.Serialization;

namespace MembershipCashierUnified.Contracts
{
    [DataContract]
    public class OwnerImplementor : IOwner
    {
        [DataMember]
        public int OwnerId { get; set; }

        /// <summary>
        /// Mimics UserId property
        /// </summary>
        public int? OwnerUserId { get; set; }

        /// <summary>
        /// Mimics OwnerUserId property
        /// </summary>
        public int UserId
        {
            get
            {
                return OwnerUserId ?? 0;
            }
            set
            {
                OwnerUserId = value;
            }
        }
    }
}

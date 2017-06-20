using MembershipCashierUnified.Interfaces;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace MembershipCashierUnified.Contracts
{
    public class OwnerContract : IHasOwner
    {
        [DataMember(Name = "Owner")]
        private OwnerImplementor _owner;

        [JsonIgnore]
        public IOwner Owner
        {
            get { return (_owner as IOwner); }
            set { var x = new OwnerImplementor(); value.CopyTo(x); _owner = x; }
        }
    }
}

using MembershipCashierUnified.Interfaces;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace MembershipCashierUnified.Contracts
{
    [KnownType(typeof(AddressImplementor))]
    [DataContract]
    public class AddressContract : IHasAddress
    {
        [DataMember(Name = "Address")]
        private AddressImplementor _address;

        [JsonIgnore]
        public IAddress Address
        {
            get { return (_address as IAddress); }
            set { var x = new AddressImplementor(); value.CopyTo(x); _address = x; }
        }
    }
}

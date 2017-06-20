using MembershipCashierUnified.Interfaces;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace MembershipCashierUnified.Contracts
{
    [DataContract]
    public class LocationContract : IHasLocation
    {
        [DataMember(Name = "Location")]
        private LocationImplementor _location;

        [JsonIgnore]
        public ILocation Location
        {
            get { return (_location as ILocation); }
            set { var x = new LocationImplementor(); value.CopyTo(x); _location = x; }
        }
    }
}

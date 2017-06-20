using MembershipCashierUnified;
using MembershipCashierUnified.Contracts;
using MembershipCashierUnified.Interfaces;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace MembershipCashierDL.MixedContracts
{
    [DataContract]
    public class Product_Location_Price_Contract : ProductContract, IHasLocation, IHasPrice
    {
        [DataMember(Name = "Location")]
        private LocationImplementor _location;

        [JsonIgnore]
        public ILocation Location
        {
            get { return (_location as ILocation); }
            set { var x = new LocationImplementor(); value.CopyTo(x); _location = x; }
        }

        [DataMember]
        public decimal Price { get; set; }
    }
}

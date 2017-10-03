using htm.paymentProcessing.core.Interfaces;
using Newtonsoft.Json;
using System;
using square = Square.Connect.Model;

namespace htm.paymentProcessing.core.Square.DataContracts
{
    public class SquareAddress : square.Address, core.Interfaces.IAddress
    {
        [JsonIgnore]
        public string Address1
        {
            get
            {
                return base.AddressLine1;
            }
            set
            {
                base.AddressLine1 = value;
            }
        }
        [JsonIgnore]
        public string City
        {
            get
            {
                return base.Locality;
            }
            set
            {
                base.Locality = value;
            }
        }
        [JsonIgnore]
        public string Province
        {
            get
            {
                return base.AdministrativeDistrictLevel1;
            }
            set
            {
                base.AdministrativeDistrictLevel1 = value;
            }
        }
        [JsonIgnore]
        string IAddress.Country
        {
            get
            {
                return base.Country.ToString();
            }
            set
            {
                base.Country = (CountryEnum)Enum.Parse(typeof(CountryEnum), value);
            }
        }
    }
}

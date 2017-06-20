using MembershipCashierUnified;
using MembershipCashierUnified.Contracts;
using MembershipCashierUnified.Interfaces;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace MembershipCashierDL.MixedContracts
{
    [DataContract]
    public class ProductCreditContract : IHasProduct, IHasProfileCredit
    {
        [DataMember(Name = "ProfileCredit")]
        private ProfileCreditImplementor _profileCredit;
        [DataMember(Name = "Product")]
        private ProductImplementor _product;

        [JsonIgnore]
        public IProfileCredit ProfileCredit
        {
            get { return (_profileCredit as IProfileCredit); }
            set { var x = new ProfileCreditImplementor(); value.CopyTo(x); _profileCredit = x; }
        }

        [JsonIgnore]
        public IProduct Product
        {
            get { return (_product as IProduct); }
            set { var x = new ProductImplementor(); value.CopyTo(x); _product = x; }
        }
    }
}

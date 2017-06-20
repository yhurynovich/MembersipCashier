using System;
using MembershipCashierUnified.Interfaces;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace MembershipCashierUnified.Contracts
{
    [DataContract]
    public class ProductContract : IHasProduct
    {
        [DataMember(Name = "Product")]
        private ProductImplementor _product;

        [JsonIgnore]
        public IProduct Product
        {
            get { return (_product as IProduct); }
            set { var x = new ProductImplementor(); value.CopyTo(x); _product = x; }
        }
    }
}

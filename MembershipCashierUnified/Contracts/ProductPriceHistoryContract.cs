using System;
using MembershipCashierUnified.Interfaces;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace MembershipCashierUnified.Contracts
{
    [DataContract]
    public class ProductPriceHistoryContract : IHasProductPriceHistory
    {
        [DataMember(Name = "ProductPriceHistory")]
        private ProductPriceHistoryImplementor _productPriceHistory;

        [JsonIgnore]
        public IProductPriceHistory ProductPriceHistory
        {
            get { return (_productPriceHistory as IProductPriceHistory); }
            set { var x = new ProductPriceHistoryImplementor(); value.CopyTo(x); _productPriceHistory = x; }
        }
    }
}

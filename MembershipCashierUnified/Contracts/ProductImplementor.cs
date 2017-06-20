using MembershipCashierUnified.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace MembershipCashierUnified.Contracts
{
    [DataContract]
    public class ProductImplementor : ProductCoreImplementor, IProduct
    {
        public IEnumerable<IProductPriceHistory> ProductPriceHistories { get; set; }

        decimal? IProduct.GetCurrentPrice()
        {
            if (ProductPriceHistories == null || !ProductPriceHistories.Any())
                return null;

            return ProductPriceHistories.OrderByDescending(x => x.ChangeDate).First().Price;
        }
    }
}

using System.Collections.Generic;

namespace MembershipCashierUnified.Interfaces
{
    public interface IHasProductPriceHistories
    {
        IEnumerable<IProductPriceHistory> ProductPriceHistories { get; set; }
    }
}

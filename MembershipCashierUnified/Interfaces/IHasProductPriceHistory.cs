using System.Collections.Generic;

namespace MembershipCashierUnified.Interfaces
{
    public interface IHasProductPriceHistory
    {
        IProductPriceHistory ProductPriceHistory { get; set; }
    }
}

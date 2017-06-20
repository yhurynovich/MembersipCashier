using System;

namespace MembershipCashierUnified.Interfaces
{
    public interface IProductPriceHistory : IHasProductId, IHasPrice, ICanCopyTo<IProductPriceHistory>
    {
        DateTime ChangeDate { get; set; }
    }
}

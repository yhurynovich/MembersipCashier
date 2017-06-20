using MembershipCashierUnified.Interfaces;
using MembershipCashierUnified;
using System;

namespace MembershipCashierUnified.Contracts
{
    public class ProductPriceHistoryImplementor : IProductPriceHistory, IHasPrice
    {
        public DateTime ChangeDate { get; set; }

        public decimal Price { get; set; }

        public int ProductId { get; set; }

        public void CopyTo(IProductPriceHistory target)
        {
            this.CopyTo(target);
        }
    }
}

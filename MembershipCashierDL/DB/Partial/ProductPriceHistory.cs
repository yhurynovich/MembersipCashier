using MembershipCashierUnified.Interfaces;

namespace MembershipCashierDL.DB
{
    public partial class ProductPriceHistory : IProductPriceHistory
    {
        public void CopyTo(IProductPriceHistory target)
        {
            MembershipCashierUnified.Extentions_CopyTo.CopyTo(this, target);
        }
    }
}

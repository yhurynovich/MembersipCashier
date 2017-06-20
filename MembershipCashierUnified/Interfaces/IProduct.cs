
namespace MembershipCashierUnified.Interfaces
{
    public interface IProduct : IProductCore, IHasProductPriceHistories
    {
        decimal? GetCurrentPrice();
    }
}

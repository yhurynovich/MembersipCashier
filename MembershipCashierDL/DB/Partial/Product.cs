using MembershipCashierDL.Properties;
using MembershipCashierUnified.Interfaces;
using System.Linq;

namespace MembershipCashierDL.DB
{
    public partial class Product : IProduct
    {
        private int numberOfPriceHistoryItemsToShow = Settings.Default.NumberOfPriceHistoryItemsToShow;

        public void SetNumberOfPriceHistoryItemsToShow(int numberOfPriceHistoryItemsToShow)
        {
            this.numberOfPriceHistoryItemsToShow = numberOfPriceHistoryItemsToShow;
        }

        public decimal? GetCurrentPrice()
        {
            if (ProductPriceHistories == null || !ProductPriceHistories.Any())
                return null;

            return ProductPriceHistories.OrderByDescending(x => x.ChangeDate).First().Price;
        }

        System.Collections.Generic.IEnumerable<IProductPriceHistory> IHasProductPriceHistories.ProductPriceHistories
        {
            get
            {
                return this.ProductPriceHistories.OrderByDescending(x=>x.ChangeDate).Take(numberOfPriceHistoryItemsToShow);
            }
            set
            {
                this.ProductPriceHistories.Clear();
                //TODO: make smart update/insert/delete instead of clear;
                this.ProductPriceHistories.AddRange( value.Select(x => new DB.ProductPriceHistory() { ChangeDate = x.ChangeDate, Price = x.Price, ProductId = x.ProductId }));
            }
        }
    }
}

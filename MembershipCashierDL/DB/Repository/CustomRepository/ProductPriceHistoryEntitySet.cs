using MembershipCashierUnified.Contracts;
using MembershipCashierUnified.Interfaces;
using SecurityDL.DB.Repository;
using SecurityUnified.Contracts;
using System.Linq;

namespace MembershipCashierDL.DB.Repository.CustomRepository
{
    internal class ProductPriceHistoryEntitySet : RepositoryEntitySet<DB.ProductPriceHistory, IProductPriceHistory>
    {
        public ProductDiscriminator ProductFilter { get; set; }

        public bool LatestPriceOnly { get; set; }

        public override IQueryable<IProductPriceHistory> Result
        {
            get
            {
                if (result == null)
                {
                    IQueryable<DB.ProductPriceHistory> newResult = GetTable<DB.ProductPriceHistory>();

                    if (ProductFilter != null)
                    {
                        IQueryable<DB.Product> products = GetTable<DB.Product>();

                        if (ProductFilter.Filter != null)
                            products = products.Where(ProductFilter.Filter).OfType<DB.Product>();

                        products = products.Skip(ProductFilter.Skip).Take(ProductFilter.Take == default(int) ? DEFAULT_MAX_RECORDS : ProductFilter.Take);

                        newResult = products.Join(
                            newResult,
                            product => product.ProductId,
                            hist => hist.ProductId,
                            (product, hist) => hist
                        );
                    }

                    if (discriminator != null)
                    {
                        if (discriminator.Filter != null)
                            newResult = newResult.Where(discriminator.Filter).OfType<DB.ProductPriceHistory>();

                        if (discriminator.OrderBy != null)
                        {
                            foreach (var oby in discriminator.OrderBy)
                            {
                                newResult = OrderByHelper(newResult, oby.FieldName, oby.IsDescending);
                            }
                        }

                        newResult = newResult.Skip(discriminator.Skip).Take(discriminator.Take);
                    }
                    else
                    {
                        newResult = newResult.Take(DEFAULT_MAX_RECORDS);
                    }

                    if (LatestPriceOnly)
                    {
                        newResult = newResult.OrderByDescending(x => x.ChangeDate).GroupBy(g => g.ProductId).Select(g => g.First());
                    }

                    result = newResult;
                }


                return result;
            }
        }
    }
}

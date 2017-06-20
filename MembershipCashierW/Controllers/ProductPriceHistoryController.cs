using MembershipCashierUnified.Contracts;
using MembershipCashierW.Controllers.Authorization;
using MembershipCashierW.Controllers.ControllerBase;
using System.Linq;
using System.Web.Http;

namespace MembershipCashierW.Controllers
{
    /// <summary>
    /// Sets new price on a product effective today
    /// </summary>
    [RoleAuthorize(Roles = Constants.ALL_EMPLOYEES)]
    public class ProductPriceHistoryController : WebApiControllerBase
    {
        /// <summary>
        /// Sets new price on a product effective today
        /// </summary>
        /// <param name="data">[IProductPriceHistory]</param>
        /// <returns>Ok</returns>
        [HttpPut]
        public IHttpActionResult Put([FromBody] ProductPriceHistoryImplementor[] data)
        {
            return Execute<IHttpActionResult>(delegate
            {
                Db.InsertProductPriceHistory(data.Select(x => new ProductPriceHistoryContract() { ProductPriceHistory = x }).ToArray());
                return base.Ok();
            });
        }
    }
}
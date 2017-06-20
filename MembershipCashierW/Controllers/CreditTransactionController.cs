using MembershipCashierUnified.Contracts;
using MembershipCashierW.Controllers.ControllerBase;
using System.Linq;
using System.Web.Http;

namespace MembershipCashierW.Controllers
{
    /// <summary>
    /// Credit Transaction Controller
    /// </summary>
    public class CreditTransactionController : WebApiControllerBase
    {
        /// <summary>
        /// Inserts new Credit transaction and recalculates ballance
        /// </summary>
        /// <param name="data">[ICreditTransaction]</param>
        /// <returns>[IProfileCredit]</returns>
        [HttpPut]
        public IHttpActionResult Put([FromBody] CreditTransactionImplementor[] data)
        {
            return Execute<IHttpActionResult>(delegate
            {
                var newBallance = Db.InsertCreditTransaction(data.Select(d => new CreditTransactionContract()
                {
                    CreditTransaction = d
                }).ToArray());

                return base.Ok(newBallance.Select(x=>x.ProfileCredit));
            });
        }
    }
}
using MembershipCashierUnified.Contracts;
using MembershipCashierUnified.Interfaces;
using MembershipCashierW.Controllers.ControllerBase;
using SecurityUnified.Serialization.Expressions;
using System.Linq;
using System.Web.Http;

namespace MembershipCashierW.Controllers
{
    /// <summary>
    /// Credit Transaction Controller
    /// </summary>
    public class CreditTransactionController : CreditTransactionControllerBase
    {
        public IHttpActionResult Get(string lambda, int? skip, int? take)
        {
            return Execute<IHttpActionResult>(delegate
            {
                if (!base.ValidateLambdaSring(lambda))
                    return base.BadRequest("incorrect lambda");

                var filter = ExpressionParser.CompileBolleanFunc<ICreditTransaction>(lambda);
                filter = EnhanceFilterByAuthorizedLocations(filter);

                var discriminator = new CreditTransactionDiscriminator() { Filter = filter };
                if (skip.HasValue)
                    discriminator.Skip = skip.Value;
                if (take.HasValue)
                    discriminator.Take = take.Value;

                return base.Ok(Db.FindCreditTransaction(discriminator));
            });
        }

        

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
using MembershipCashierUnified.Contracts;
using MembershipCashierUnified.Interfaces;
using MembershipCashierW.Controllers.Authorization;
using MembershipCashierW.Controllers.ControllerBase;
using SecurityUnified.Exceptions;
using SecurityUnified.Serialization.Expressions;
using System.Web.Http;

namespace MembershipCashierW.Controllers
{
    [RoleAuthorize(Roles = Constants.USERS_WITH_PROFILE_ACCESS)]
    public class ClientLastUsedProductsController : BlobSearchUserProfileControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="blob">text to search by</param>
        /// <param name="skip">skip result records</param>
        /// <param name="take">number of records to take</param>
        /// <returns></returns>
        public IHttpActionResult Get(string blob, int? skip, int? take)
        {
            return Execute<IHttpActionResult>(delegate
            {
                if (string.IsNullOrWhiteSpace(blob))
                    return base.BadRequest("blob is not supplied");

                var discriminator = new UserProfile2Discriminator() { Filter = base.GetBlobFilter(blob) };
                if (skip.HasValue)
                    discriminator.Skip = skip.Value;
                if (take.HasValue)
                    discriminator.Take = take.Value;

                return base.Ok(Db.FindLastUsedProducts(discriminator, AuthorizedLocationFilter));
            });
        }

        [HttpPost]
        public IHttpActionResult Post(string lambda, int? skip, int? take)
        {
            return Execute<IHttpActionResult>(delegate
            {
                if (!ValidateLambdaSring(lambda))
                    throw new Xxception(Errors.InvalidLambdaParameter);

                var filter = ExpressionParser.CompileBolleanFunc<IUserProfile2>(lambda);
                var discriminator = new UserProfile2Discriminator() { Filter = filter };
                if (skip.HasValue)
                    discriminator.Skip = skip.Value;
                if (take.HasValue)
                    discriminator.Take = take.Value;

                return base.Ok(Db.FindLastUsedProducts(discriminator, AuthorizedLocationFilter));
            });
        }
    }
}
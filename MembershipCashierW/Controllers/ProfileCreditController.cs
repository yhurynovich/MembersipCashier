using MembershipCashierUnified.Contracts;
using MembershipCashierUnified.Interfaces;
using MembershipCashierW.Code;
using MembershipCashierW.Controllers.Authorization;
using MembershipCashierW.Controllers.ControllerBase;
using SecurityUnified.Serialization.Expressions;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Web.Http;

namespace MembershipCashierW.Controllers
{
    [RoleAuthorize(Roles = Constants.USERS_WITH_PROFILE_ACCESS)]
    public class ProfileCreditController : WebApiControllerBase
    {
        [DataContract]
        public class ProfileCreditRequest
        {
            [DataMember]
            public int? UserId { get; set; }
            [DataMember]
            public string LocationLambda { get; set; }
            [DataMember]
            public string ProductLambda { get; set; }
        }

        /// <summary>
        /// Retreaves last ballance
        /// </summary>
        /// <param name="request">ProfileCreditRequest</param>
        /// <returns>[IProfileCredit]</returns>
        [HttpGet]
        public IHttpActionResult Get([FromUri] ProfileCreditRequest request)
        {
            return Execute<IHttpActionResult>(delegate
            {
                bool isPoweUser = SessionGlobal.CurrentUser.RoleNames.Any(a =>
                    a == Constants.OWNER
                    || a == Constants.TELLER
                    || Constants.ALL_MARKETING_AsArray.Any(b => a == b));

                Expression<Func<IProfileCredit, bool>> filter = x=>true;

                if (!isPoweUser)
                {
                    if (isPoweUser || request.UserId.Value == SessionGlobal.CurrentUser.Identity.UserId)
                    {
                        int userId = request.UserId.Value;
                        filter = x => x.UserId == userId;
                    }
                    else
                        throw new UnauthorizedAccessException("Attempt to request records from other profile");
                }

                LocationDiscriminator locationFilter = null;
                if (!string.IsNullOrEmpty(request.LocationLambda))
                {
                    locationFilter = new LocationDiscriminator() { Filter = ExpressionParser.CompileBolleanFunc<ILocation>(request.LocationLambda) };
                }

                ProductDiscriminator productFilter = null;
                if (!string.IsNullOrEmpty(request.ProductLambda))
                {
                    productFilter = new ProductDiscriminator() { Filter = ExpressionParser.CompileBolleanFunc<IProduct>(request.ProductLambda) };
                }

                return base.Ok( Db.FindProfileCredit2(
                    productFilter, 
                    new ProfileCreditDiscriminator() { Filter = filter },
                     locationFilter
                    ) 
                );
            });
        }

        /// <summary>
        /// Updates current ballance. Only use this method if correction (reconciliation) needs to be done. Usually balance will be automatically calculated when new transaction is placed.
        /// </summary>
        /// <param name="data">[IProfileCredit]</param>
        [RoleAuthorize(Roles = Constants.ALL_MARKETING)]
        public void Post([FromBody] ProfileCreditImplementor[] data)
        {
            try
            {
                if (data == null || !data.Any())
                    throw new ArgumentException("No credit change data submitted");

                Db.InsertOrUpdateProfileCredit(data.Select(d =>
                    new ProfileCreditContract() { ProfileCredit = d }).ToArray()
               , false);
            }
            catch (Exception ex)
            {
                this.HandleError(ex);
                throw ex;
            }
        }
    }
}

using MembershipCashierDL.MixedContracts;
using MembershipCashierUnified.Contracts;
using MembershipCashierUnified.Interfaces;
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
    /// <summary>
    /// All product entities are stored in the same place in persistance. ProductController provides CRUD for them.
    /// </summary>
    [RoleAuthorize(Roles = Constants.ALL_AUTHENTICATED)]
    public class ProductLastTransactionsController : UserProfileControllerBase
    {
        /// <summary>
        /// Find product request
        /// </summary>
        [DataContract]
        public struct ProductRequest
        {
            /// <summary>
            /// lambda string against IProduct
            /// </summary>
            [DataMember]
            public string ProductLambda { get; set; }
            /// <summary>
            /// lambda string against ILocation
            /// </summary>
            [DataMember]
            public string LocationLambda { get; set; }
            /// <summary>
            /// Max count of transactions to select by each product
            /// </summary>
            [DataMember]
            public int trnCntByProduct { get; set; }
        }

        /// <summary>
        /// Retreaves products
        /// </summary>
        /// <param name="request">ProductRequest with product and location filters</param>
        /// <returns>[IProduct]</returns>
        public IHttpActionResult Get([FromUri] ProductRequest request)
        {
            return Execute<IHttpActionResult>(delegate
            {
                Expression<Func<ILocation, bool>> locationFilter = null;
                Expression<Func<IProduct, bool>> productFilter = null;

                if (ValidateLambdaSring(request.LocationLambda))
                {
                    locationFilter = ExpressionParser.CompileBolleanFunc<ILocation>(request.LocationLambda);
                }
                locationFilter = EnhanceFilterByAuthorizedLocations(locationFilter);

                if (ValidateLambdaSring(request.ProductLambda))
                    productFilter = ExpressionParser.CompileBolleanFunc<IProduct>(request.ProductLambda);

                if (locationFilter == null && productFilter == null)
                    return base.BadRequest();

                return base.Ok(Db.FindProductLastTransactions(
                    productFilter != null ? new ProductDiscriminator() { Filter = productFilter } : null,
                    locationFilter != null ? new LocationDiscriminator() { Filter = locationFilter } : null,
                    null,
                    request.trnCntByProduct
                ));
            });
        }
    }
}
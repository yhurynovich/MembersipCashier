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
using MembershipCashierUnified.Contracts;

namespace MembershipCashierW.Controllers
{
    /// <summary>
    /// All product entities are stored in the same place in persistance. ProductController provides CRUD for them.
    /// </summary>
    [RoleAuthorize(Roles = Constants.ALL_AUTHENTICATED)]
    public class ProductController : UserProfileControllerBase
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
            /// Exclude products that current user already have in history
            /// </summary>
            [DataMember]
            public bool NotInCurrentUserHistory { get; set; }
        }

        [DataContract]
        public class NewProductRequest : ProductImplementor, IHasPrice
        {
            [DataMember]
            public decimal Price { get; set; }
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

                int currentUserId = default(int);
                if (request.NotInCurrentUserHistory)
                    currentUserId = SessionGlobal.CurrentUser.Identity.UserId;

                return base.Ok(Db.FindProduct(
                    productFilter != null ? new ProductDiscriminator() { Filter = productFilter } : null,
                    locationFilter != null ? new LocationDiscriminator() { Filter = locationFilter } : null,
                    null,
                    currentUserId
                ).Select(x => x.Product));
            });
        }

        /// <summary>
        /// Updates products that are matched by ProductId
        /// </summary>
        /// <param name="data">[IProduct]</param>
        /// <returns>IHttpActionResult</returns>
        [HttpPost]
        public IHttpActionResult Post([FromBody] ProductImplementor[] data)
        {
            return Execute<IHttpActionResult>(delegate
            {
                Db.UpdateProduct(data.Select(d =>new ProductContract() { Product = d }).ToArray(), false);
                return base.Ok();
            });
        }

        /// <summary>
        /// Inserts new products and associates them with current location
        /// </summary>
        /// <param name="data">[IProduct]</param>
        /// <returns>IHttpActionResult</returns>
        [HttpPut]
        public IHttpActionResult Put([FromBody] NewProductRequest[] data)
        {
            return Execute<IHttpActionResult>(delegate
            {
                Db.InsertProduct(data.Select(d => new Product_Location_Price_Contract()
                {
                    Product = d,
                    Location = SessionGlobal.CurrentLocation,
                    Price = d.Price
                }).ToArray());
                return base.Ok();
            });
        }
    }
}
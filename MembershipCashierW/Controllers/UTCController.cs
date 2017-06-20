using MembershipCashierW.Controllers.Authorization;
using MembershipCashierW.Controllers.ControllerBase;
using System;
using System.Web.Http;

namespace MembershipCashierW.Controllers
{
    /// <summary>
    /// UTCController provides server time in UTC and UTC conversion
    /// </summary>
    [RoleAuthorize(Roles = Constants.ALL_ROLES)]
    [System.Web.Mvc.ValidateAntiForgeryToken]
    public class UTCController : WebApiControllerBase
    {
        /// <summary>
        /// Returns current server time in UTC format
        /// </summary>
        /// <returns>DateTime</returns>
        public IHttpActionResult Get()
        {
            return Execute<IHttpActionResult>(delegate
            {
                return base.Ok(DateTime.UtcNow);
            });
        }

        /// <summary>
        /// Translates a date into UTC format
        /// </summary>
        /// <param name="date"></param>
        /// <returns>DateTime</returns>
        public IHttpActionResult Get(DateTime date)
        {
            return Execute<IHttpActionResult>(delegate
            {
                return base.Ok(date.ToUniversalTime());
            });
        }
    }
}
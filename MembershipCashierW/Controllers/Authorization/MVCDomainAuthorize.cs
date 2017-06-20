using System;
using System.Linq;
using System.Web.Mvc;

namespace MembershipCashierW.Controllers.Authorization
{
    public class MvcDomainAuthorize : FilterAttribute, IAuthorizationFilter
    {
        private string redirectUrl = null;
        private string[] allowedHosts = new string[] { };

        public MvcDomainAuthorize()
            : base()
        {
        }

        public MvcDomainAuthorize(string[] allowedHosts, string redirectUrl)
            : this(allowedHosts)
        {
            this.redirectUrl = redirectUrl;
        }

        public MvcDomainAuthorize(string[] allowedHosts)
            : this()
        {
            this.allowedHosts = allowedHosts.Select(x => x.ToLower()).ToArray();
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!allowedHosts.Contains(filterContext.HttpContext.Request.Url.Host))
            {
                string authUrl = this.redirectUrl; //passed from attribute

                if (String.IsNullOrEmpty(authUrl))
                    authUrl = "/Breeze/Login";

                if (!String.IsNullOrEmpty(authUrl))
                {
                    filterContext.Result = new RedirectResult(authUrl, true);
                    return;
                }
            }
        }
    }
}
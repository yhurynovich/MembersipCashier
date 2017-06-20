using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace MembershipCashierW.Controllers.Authorization
{
    public class DomainAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private string redirectUrl = null;
        private string[] allowedHosts = new string[] { };

        public DomainAuthorizeAttribute()
            : base()
        {
        }

        public DomainAuthorizeAttribute(string[] allowedHosts, string redirectUrl)
            : this(allowedHosts)
        {
            this.redirectUrl = redirectUrl;
        }

        public DomainAuthorizeAttribute(string[] allowedHosts)
            : this()
        {
            this.allowedHosts = allowedHosts.Select(x=>x.ToLower()).ToArray();
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            if (allowedHosts.Contains(actionContext.Request.RequestUri.Host))
            {
                string authUrl = this.redirectUrl; //passed from attribute

                if (String.IsNullOrEmpty(authUrl))
                    authUrl = "/Root/Login";

                if (!String.IsNullOrEmpty(authUrl))
                {
                    actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Redirect);
                    actionContext.Response.Headers.Location = new Uri(actionContext.Request.RequestUri, authUrl);
                    return;
                }
            }

            base.HandleUnauthorizedRequest(actionContext);
        }
    }
}
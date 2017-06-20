using System;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace MembershipCashierW.Controllers.Authorization
{
    public class RoleAuthorizeAttribute : AuthorizeAttribute
    {
        private string redirectUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["RolesAuthRedirectUrl"];
        public RoleAuthorizeAttribute()
            : base()
        {
        }

        //public RoleAuthorizeAttribute(string redirectUrl)
        //    : base()
        //{
        //    this.redirectUrl = redirectUrl;
        //}

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            AuthorizationWorker(actionContext);

            base.OnAuthorization(actionContext);
        }

        public override System.Threading.Tasks.Task OnAuthorizationAsync(HttpActionContext actionContext, System.Threading.CancellationToken cancellationToken)
        {
            AuthorizationWorker(actionContext);

            return base.OnAuthorizationAsync(actionContext, cancellationToken);
        }

        private void AuthorizationWorker(HttpActionContext actionContext)
        {
            var currentUser = SessionGlobal.CurrentUser;

            if (!actionContext.RequestContext.Principal.Identity.IsAuthenticated)
            {
                if (currentUser != null && currentUser.Identity != null)
                    SetPrincipal(currentUser);
            }
        }

        private void SetPrincipal(IPrincipal principal)
        {
            System.Threading.Thread.CurrentPrincipal = principal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
        }


        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            //if (actionContext.HttpContext.Request.IsAuthenticated)
            if (!actionContext.RequestContext.Principal.Identity.IsAuthenticated)
            {
                string authUrl = this.redirectUrl; //passed from attribute

                //if null, get it from config
                if (String.IsNullOrEmpty(authUrl))
                    authUrl = "/Login";

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

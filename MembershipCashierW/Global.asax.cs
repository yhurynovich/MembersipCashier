using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;

namespace MembershipCashierW
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        public const string ANTIFORGERY_TOKEN_HEADER_NAME = "antiforgery";
        public const string UI_CULTURE_HEADER_NAME = "ui_lang";

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        void Session_Start(object sender, EventArgs e)
        {
            if (HttpContext.Current != null)
            {
                //HttpContext.Current.Session["Language"] = System.Globalization.CultureInfo.InvariantCulture.IetfLanguageTag;

                if (HttpContext.Current.Request[UI_CULTURE_HEADER_NAME] != null)
                    SessionGlobal.Language = HttpContext.Current.Request[UI_CULTURE_HEADER_NAME];
                else
                    if (HttpContext.Current.Request.UserLanguages != null)
                    {
                        SessionGlobal.Language = Request.UserLanguages[0];
                    }
            }
        }
         
        protected void Application_PostAuthorizeRequest()
        {
            HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            string lang = SessionGlobal.Language;
            var cultur_multur = CultureInfo.CreateSpecificCulture(lang);
            Thread.CurrentThread.CurrentUICulture = cultur_multur;
            HttpContext.Current.Response.Headers.Add(UI_CULTURE_HEADER_NAME, lang);

            if (IsWebApiRequest())
            {
                string newToken = SessionGlobal.RegisterServiceAntiforgeryToken(
                    Convert.ToBase64String(
                        ProtectedData.Protect(
                            System.Text.Encoding.Unicode.GetBytes(
                                string.Concat(Session.SessionID, "_", DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss"))
                            ), null, DataProtectionScope.CurrentUser
                        )
                    )
                );

                HttpContext.Current.Response.Headers.Remove(ANTIFORGERY_TOKEN_HEADER_NAME);
                HttpContext.Current.Response.Headers.Add(ANTIFORGERY_TOKEN_HEADER_NAME, newToken);
            }
        }

        private bool IsWebApiRequest()
        {
            return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.StartsWith(WebApiConfig.UrlPrefixRelative);
        }


    }
}
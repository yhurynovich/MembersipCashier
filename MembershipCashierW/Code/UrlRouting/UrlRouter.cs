using System;
using System.Linq;
using System.Web.Mvc;

namespace MembershipCashierW.Code.UrlRouting
{
    public class UrlRouter
    {
        private Routs.ApplicationOptions DefaultApplication
        {
            get {
                return Routs.ApplicationOptions.MembershipCashierW;
            }
        }

        public Uri GetEntryPage()
        {
            try
            {
                string key = "login";
                var principal = SessionGlobal.CurrentUser;

                if (principal != null)
                {
                    if (principal.RoleNames.Any())
                    {
                        if (principal.RoleNames.Contains(Constants.SUPER_ADMIN))
                            key = "superAdminConsole";
                        else if (principal.RoleNames.Contains(Constants.ADMIN))
                            key = "adminConsole";
                        else if (principal.RoleNames.Contains(Constants.MARKETING))
                            key = Constants.MARKETING;
                        else
                            key = "dashboard";
                    }
                }

                return new Routs() { PrefferedApplication = DefaultApplication }.GetEntryPage(key);
            }
            catch (Exception ex)
            {
                Utils.WriteToEventLog(ex);
                return new Uri("/#routingException");
            }
        }

        public ActionResult GetEntryAction()
        {
            return new RedirectResult(GetEntryPage().ToString());
        }
    }
}
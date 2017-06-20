using MembershipCashierW.Properties;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace MembershipCashierW.Controllers.Authorization.Antiforgery.Services
{
    public static class ServiceAntiforgery
    {
        static string[] excludedTargets = { "/api/forgotpassword" };

        public static void Assert()
        {
            if (Settings.Default.ServiceAntiforgeryEnabled)
            {
                var request = HttpContext.Current.Request;
                if (excludedTargets.Contains(request.Url.LocalPath.ToLower()))
                    return;

                string receivedToken = request.Headers["RequestVerificationToken"];
                if (!string.IsNullOrEmpty(receivedToken))
                {
                    if (!SessionGlobal.GetServiceAntiforgeryTokens().Contains(receivedToken))
                    {
                        //try do decrypt time
                        var origStr = System.Text.Encoding.Unicode.GetString(
                            ProtectedData.Unprotect(
                                Convert.FromBase64String(receivedToken)
                            , null, DataProtectionScope.CurrentUser));

                        var parts = origStr.Split('_');
                        if (parts.Length == 2 && parts[0] == HttpContext.Current.Session.SessionID)
                        {
                            DateTime tokenTime;
                            DateTime.TryParseExact(parts[1], "MM/dd/yyyy HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out tokenTime);
                            if ((DateTime.UtcNow - tokenTime).TotalSeconds < 11)
                                return;
                        }

                        //AuthenticationFactory.Logout();
                        throw new System.Web.Mvc.HttpAntiForgeryException();
                    }
                }
            }
        }
    }
}
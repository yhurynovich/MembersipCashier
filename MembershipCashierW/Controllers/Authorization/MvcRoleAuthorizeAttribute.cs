using System.Linq;
using System.Web.Mvc;

namespace MembershipCashierW.Controllers.Authorization
{
    public class MvcRoleAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (Roles == Constants.ALL_Roles 
                || filterContext.ActionDescriptor.ActionName == "Login"
                || filterContext.ActionDescriptor.ActionName == "ForgotPassword"
                || filterContext.ActionDescriptor.ActionName == "FirstLogin"
                || filterContext.ActionDescriptor.ActionName == "ManageAccounts"
                || filterContext.ActionDescriptor.ActionName == "EditProfile"
                || filterContext.ActionDescriptor.ActionName == "Pwd"
                || filterContext.ActionDescriptor.ActionName == "UpdateProfile"
            )
            {
                base.OnAuthorization(filterContext);
            }
            else
            {
                var currentUser = SessionGlobal.CurrentPrincipal;

                if (currentUser != null && currentUser.RoleNames.Any(r => Constants.ALL_MARKETING_as_Array.Contains(r)))
                    base.OnAuthorization(filterContext);
                else
                {
                    var currentUserLocations = SessionGlobal.CurrentUserLocations;
                    if (currentUserLocations != null && currentUserLocations.Any())
                    {
                        var currentLocation = SessionGlobal.CurrentUserLocations.FirstOrDefault(l => l.IsCurrentLocation);

                        if(currentUser.RoleNames.Any(r => r==Constants.SUPERVISING_USER))
                            base.OnAuthorization(filterContext);
                        else if (currentLocation != null && !currentLocation.ScanEnabled)
                            filterContext.Result = new HttpUnauthorizedResult();
                        else
                            base.OnAuthorization(filterContext);
                    }
                    else
                        base.OnAuthorization(filterContext);
                }
            }
        }
    }
}
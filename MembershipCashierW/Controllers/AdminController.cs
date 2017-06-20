using MembershipCashierW.Controllers.ControllerBase;
using System.Web.Mvc;


namespace MembershipCashierW.Controllers
{
    public class AdminController : MVCControllersBase
    {
        [Authorize(Roles = Constants.ALL_ADMINS)]
        public ActionResult AdminConsole()
        {
            return RedirectToAction("Index", "Root");
        }

        [Authorize(Roles = Constants.SUPER_ADMIN)]
        public ActionResult SuperAdminConsole()
        {
            return RedirectToAction("Index", "Root");
        }
    }
}
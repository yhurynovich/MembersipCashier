using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MembershipCashierW.Controllers.ControllerBase
{
    [HandleError(ExceptionType = typeof(HttpAntiForgeryException), View = "ErrorMessages/NoCookieSupport")]
    [HandleError(View = "ErrorMessages/DefaultError")]
    //[InitializeSimpleMembership]
    public class MVCControllersBase : Controller
    {
        const bool unknownUserCountryProblem = false; //TODO:

        public virtual ActionResult ViewSwitch()
        {
            if (unknownUserCountryProblem)
                return RedirectToAction("SelectCountry", "Shared", new { c = this.ControllerContext.RouteData.Values["controller"], a = this.ControllerContext.RouteData.Values["action"] });
            else if (base.Request["X-Requested-With"] == "XMLHttpRequest") // Ajax
                return base.PartialView();
            else
                return base.View();
        }

        public virtual ActionResult ViewSwitch(object model)
        {
            if (unknownUserCountryProblem)
                return RedirectToAction("SelectCountry", "Shared", new { c = this.ControllerContext.RouteData.Values["controller"], a = this.ControllerContext.RouteData.Values["action"] });
            if (base.Request["X-Requested-With"] == "XMLHttpRequest") // Ajax
                return base.PartialView(model);
            else
                return base.View(model);
        }
    }
}
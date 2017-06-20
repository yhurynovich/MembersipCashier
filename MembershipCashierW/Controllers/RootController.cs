using MembershipCashierW.Controllers.ControllerBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MembershipCashierW.Controllers
{
    public class RootController : MVCControllersBase
    {
        [AllowAnonymous]
        public ActionResult Login()
        {
            return ViewSwitch();
        }

        [Authorize(Roles = Constants.ALL_AUTHENTICATED)]
        public ActionResult Index()
        {
            return ViewSwitch();
        }
    }
}
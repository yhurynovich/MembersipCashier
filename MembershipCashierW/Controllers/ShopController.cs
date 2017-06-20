using MembershipCashierW.Controllers.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MembershipCashierW.Controllers
{
    [Authorize(Roles = Constants.USERS_WITH_PROFILE_ACCESS)]
    public class ShopController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Default()
        {
            return View();
        }

        // GET: Shop

        public ActionResult FindMember()
        {
            return View();
        }
    }
}
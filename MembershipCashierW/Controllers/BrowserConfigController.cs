using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MembershipCashierW.Controllers
{
    public class BrowserConfigController : Controller
    {
        // GET: BrowserConfig
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult BrowserConfig()
        {
            Response.ContentType = "text/xml"; 
            return View();
        }
    }
}
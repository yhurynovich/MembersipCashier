using SecurityUnified.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MembershipCashierW.Code.Menu
{
    public static class MenuHelper
    {
        internal static readonly string _MenuItem = "<li><a href='{0}' ng-translate='{1}'></a></li>";
        internal static readonly string _MenuItemToggle = "<li><a href='#' data-toggle='modal' data-target='{0}' ng-translate='{1}'></a></li>";

        public static MvcHtmlString GetMenu()
        {
            //var culture = SessionGlobal.Language;

            StringBuilder menu = new StringBuilder();

            if (SessionGlobal.CurrentUser == null)
            {
                menu.AppendLine(CreateMenuItemWithToggle("#login-modal", "Login"));
                menu.AppendLine(CreateMenuItem("/Products", "Products"));
                return new MvcHtmlString(menu.ToString());
            }

            ////ADMIN/SUPER ADMIN
            //if (SessionGlobal.CurrentUser.RoleNames.Any(r => Constants.SUPER_ADMIN.Split(',').Select(s => s.Trim()).Contains(r)))
            //{
            //    menu.AppendLine(CreateMenuItem("/breeze/admin", "Admin Dashboard"));
            //    menu.AppendLine(CreateMenuItem("/Roles/SuperAdminConsole", "Manage Admins"));
            //    menu.AppendLine(CreateMenuItem("/Roles/ProcessMonitor", "Process Monitor"));
            //    menu.AppendLine(CreateMenuItem("/Reports/PivotReport", "Reports"));
            //    //menu.AppendLine(CreateMenuItem("/mc/managecustomers", "Manage Customers")); //does not work for some reason
            //    return new MvcHtmlString(menu.ToString());
            //}

            ////User is in marketing
            //if (SessionGlobal.CurrentUser.RoleNames.Any(r => Constants.ALL_MARKETING.Split(',').Select(s => s.Trim()).Contains(r)))
            //{
            //    //< li >< a href = "#" > Manage Customers </ a ></ li >
            //    menu.AppendLine(CreateMenuItem("/mc/managecustomers", "Manage Customers"));
            //}

            //Customer Users
            //if (SessionGlobal.CurrentUser.RoleNames.Any(r => Constants.ALL_AUTHENTICATED.Split(',').Select(s => s.Trim()).Contains(r)))
            //{
            //    var location = SessionGlobal.CurrentUser.FirstOrDefault(l => l.IsCurrentLocation);

            //    if (location == null)
            //        throw new Xxception("User location has not been configured.");

            //    menu.AppendLine(CreateMenuItem("/breeze", "Dashboard"));

            //    if (location.RetailShipEnabled)
            //    {
            //        menu.AppendLine(CreateMenuItem("/breeze/shipping", "Shipping"));
            //        menu.AppendLine(CreateMenuItem("/breeze/shipmentqueue", "Pending Shipments"));
            //        menu.AppendLine(CreateMenuItem("/breeze/shipmenthistory", "Shipment History"));
            //    }
            //}

            return new MvcHtmlString(menu.ToString());
        }

        private static string CreateMenuItem(string href, string title)
        {
            return string.Format(_MenuItem, href, title);
        }

        private static string CreateMenuItemWithToggle(string target, string title)
        {
            return string.Format(_MenuItemToggle, target, title);
        }
    }
}
using System;
using System.Collections.Generic;

namespace MembershipCashierW.Code.UrlRouting.AppSpecificRotes
{
    public class MembershipCashierWRouts : Dictionary<string,Uri>
    {
        public MembershipCashierWRouts()
        {
            base.Add("login", new Uri(HtmlExtentions.ResolveUrl("~/root/login")));
            base.Add("dashboard", new Uri(HtmlExtentions.ResolveUrl("~/root")));
            base.Add("adminConsole", new Uri(HtmlExtentions.ResolveUrl("~/admin/AdminConsole")));
            base.Add("superAdminConsole", new Uri(HtmlExtentions.ResolveUrl("~/admin/SuperAdminConsole")));
        }
    }
}
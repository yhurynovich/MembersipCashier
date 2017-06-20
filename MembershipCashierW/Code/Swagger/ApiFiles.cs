using System;

namespace MembershipCashierW.Code.Swagger
{
    public static class ApiFiles
    {
        public static string GetXmlCommentsPath()
        {
            return String.Format(@"{0}\bin\MembershipCashierW.XML", AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}
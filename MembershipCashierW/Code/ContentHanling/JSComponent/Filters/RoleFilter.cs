using System;
using System.Linq;

namespace MembershipCashierW.Code.ContentHanling.JSComponent.Filters
{
    internal class RoleFilter : IJSComponentFilter
    {
        private static RoleFilter instance;

        internal static IJSComponentFilter Instance
        {
            get { return instance; }
        }

        public Func<JsFileDesc, bool> Filter
        {
            get
            {
                //var nodes = Permissions.DocumentElement.ChildNodes.OfType<XmlNode>();
                var currentUserRoles = SessionGlobal.CurrentUser.RoleNames;

                //Func<KeyValuePair<string, string>, bool> ret = z =>
                //        nodes.Where(p => p.Attributes.OfType<XmlAttribute>().Any(a => a.Name == "file" && a.Value == z.Key)) //matching filename
                //        .Any(x =>
                //            x.Attributes.OfType<XmlAttribute>().Any(a => a.Name == "type" && a.Value == "directive")
                //            || x.ChildNodes.OfType<XmlNode>().Where(n => n.Name == "allow").Any(a => a.ChildNodes.OfType<XmlNode>().Any(f => f.Name == "role" && currentUserRoles.Contains(f.InnerText))));

                Func<JsFileDesc, bool> ret = z =>
                    CSComponentFilterUtil.DeclaredScripts.Any(p => p.File == z.File && (p.Type == "directive" || currentUserRoles.Any(r => p.Roles.Contains(r))));

                return ret;
            }
        }

        static RoleFilter()
        {
            lock (typeof(ConfigurationFilter))
            {
                instance = new RoleFilter();
            }
        }
    }
}
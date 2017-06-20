using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

namespace MembershipCashierW.Code.ContentHanling.JSComponent.Filters
{
    internal abstract class CSComponentFilterUtil
    {
        private static XmlDocument declaredScripts;
        private static JsFileDescComparer comparer = new JsFileDescComparer();
        private const string PERMISSIONS_XML_PATH = "~/Code/ContentHanling/JSComponent/Filters/JSComponentPremissions.xml";

        internal static IEnumerable<JsFileDesc> DeclaredScripts
        {
            get
            {
                if (declaredScripts == null)
                {
                    try
                    {
                        declaredScripts = new XmlDocument();
                        declaredScripts.LoadXml(File.ReadAllText(HttpContext.Current.Server.MapPath(PERMISSIONS_XML_PATH)));
                    }
                    catch (Exception ex)
                    {
                        Utils.WriteToEventLog(ex);
                        declaredScripts = null;
                    }
                }
                return ToJsFileDesc(declaredScripts.DocumentElement.ChildNodes.OfType<XmlNode>());
            }
        }

        internal static IEnumerable<JsFileDesc> IncludeDependents(IEnumerable<JsFileDesc> selection)
        {
            IEnumerable<JsFileDesc> ret = selection;

            var innerDpdt = selection.SelectMany(x =>
                x.Dependencies
            );
            ret = ret.Union(innerDpdt);

            while (innerDpdt.Any(i => i.Dependencies.Any()))
            {
                innerDpdt = innerDpdt.SelectMany(x =>
                    x.Dependencies
                );
                ret = ret.Union(innerDpdt);
            }

            return ret.Distinct(comparer);
        }

        protected static IEnumerable<JsFileDesc> ToJsFileDesc(IEnumerable<XmlNode> selection)
        {
            return selection.Select(x => new JsFileDesc() { Source = x });
        }
    }
}
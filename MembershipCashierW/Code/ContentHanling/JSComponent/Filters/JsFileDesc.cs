using System.Collections.Generic;
using System.Linq;
using System.Web.Hosting;
using System.Xml;

namespace MembershipCashierW.Code.ContentHanling.JSComponent.Filters
{
    public struct JsFileDesc
    {
        public string FullPath
        {
            get
            {
                switch(Type)
                {
                    case "component":
                        return HostingEnvironment.MapPath(
                               string.Format(@"~/ScriptControls/Components/{0}/{1}", Folder, File));
                    case "controller":
                        return HostingEnvironment.MapPath(
                               string.Format(@"~/ScriptControls/Controllers/{0}/{1}", Folder, File));
                    case "factory":
                        return HostingEnvironment.MapPath(
                               string.Format(@"~/ScriptControls/Factories/{0}/{1}", Folder, File));
                    case "directive":
                        return HostingEnvironment.MapPath(
                               string.Format(@"~/ScriptControls/Directives/{0}/{1}", Folder, File));
					case "service":
						return HostingEnvironment.MapPath(
							   string.Format(@"~/ScriptControls/Services/{0}/{1}", Folder, File));
				}
                return null;
            }
        }
        public string File
        {
            get
            {
                return Source.Attributes["file"].Value;
            }
        }
        public string Type
        {
            get
            {
                return Source.Attributes["type"].Value;
            }
        }
        public string Folder
        {
            get
            {
                return Source.Attributes["folder"] == null ? null : Source.Attributes["folder"].Value;
            }
        }
        public IEnumerable<string> Scenarious
        {
            get
            {
                return Source.ChildNodes.OfType<XmlNode>()
                    .Where(n => n.Name == "allow")
                    .SelectMany(x => x.ChildNodes.OfType<XmlNode>().Where(n => n.Name == "scenario").Select(s => s.InnerText));
            }
        }
        public IEnumerable<string> Roles
        {
            get
            {
                return Source.ChildNodes.OfType<XmlNode>()
                    .Where(n => n.Name == "allow")
                    .SelectMany(x => x.ChildNodes.OfType<XmlNode>().Where(n=>n.Name == "role").Select(s => s.InnerText));
            }
        }
        public IEnumerable<JsFileDesc> Dependencies
        {
            get
            {
                var declaredDependancies = Source.ChildNodes.OfType<XmlNode>()
                    .Where(n => n.Name == "dependency")
                    .SelectMany(x => x.ChildNodes.OfType<XmlNode>().Select(d => new JsFileDesc() { Source = d }));

                //Replace with the root elements
                List<JsFileDesc> ret = new List<JsFileDesc>(declaredDependancies.Count());
                foreach (var d in declaredDependancies)
                {
                    ret.Add(CSComponentFilterUtil.DeclaredScripts.FirstOrDefault(
                            x=>x.File == d.File && x.Folder==d.Folder && x.Type==d.Type
                        ));
                }

                return ret;
            }
        }
        public XmlNode Source { get; set; }
    }
}
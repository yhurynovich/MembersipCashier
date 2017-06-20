using MembershipCashierW.Code;
using MembershipCashierW.Code.ContentHanling.JSComponent.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Hosting;
using System.Web.Mvc;

namespace MembershipCashierW.Controllers
{
    public class ComponentController : Controller
    {
        [HttpGet]
        public ContentResult Login()
        {
            try
            {
                return GetScriptContent(HostingEnvironment.MapPath(@"~/ScriptControls/Components/core/login.controller.js"));
            }
            catch (Exception ex)
            {
                Utils.WriteToEventLog(ex);
                throw;
            }
        }

        [HttpGet]
        public ContentResult GetPermittedScripts(string scenario)
        {
            var scenarioScripts = CSComponentFilterUtil.DeclaredScripts.Where(j => j.Scenarious.Any(s => s == scenario));
            scenarioScripts = scenarioScripts.Where(RoleFilter.Instance.Filter);
            //scenarioScripts = scenarioScripts.Where(ConfigurationFilter.Instance.Filter);
            scenarioScripts = CSComponentFilterUtil.IncludeDependents(scenarioScripts);

            return GetScriptContent(scenarioScripts.Select(c => c.FullPath));
        }

        private ContentResult GetScriptContent(string absolutePath)
        {
            return new ContentResult
            {
                ContentType = "text/javascript",
                Content = System.Text.Encoding.UTF8.GetString(System.IO.File.ReadAllBytes(absolutePath)),
                ContentEncoding = System.Text.Encoding.UTF8
            };
        }

        private ContentResult GetScriptContent(IEnumerable<string> absolutePath)
        {
            try
            {
                var sb = new StringBuilder();

                //TODO: Apply permition filters
                foreach (var path in absolutePath)
                {
                    sb.AppendLine(System.Text.Encoding.UTF8.GetString(System.IO.File.ReadAllBytes(path)));
                }

                return new ContentResult
                {
                    ContentType = "text/javascript",
                    Content = sb.ToString(),
                    ContentEncoding = System.Text.Encoding.UTF8
                };
            }
            catch (Exception ex)
            {
                Utils.WriteToEventLog(ex);
                throw;
            }
        }
    }
}
using Swashbuckle.Swagger;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;

namespace MembershipCashierW.Code.Swagger
{
    public class CustomDocumentFilter : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
        {
            var thisAssemblyTypes = Assembly.GetExecutingAssembly().GetTypes().ToList();

            var odatacontrollers = thisAssemblyTypes.Where(t => t.IsSubclassOf(typeof(ODataController))).ToList();
            //var odatacontrollers = thisAssemblyTypes.Where(t => t.IsSubclassOf(typeof(ODataController)) || t.Name.EndsWith("Controller")).ToList();
            var odataRoutes = GlobalConfiguration.Configuration.Routes.Where(a => a.GetType() == typeof(ODataRoute)).ToList();

            if (!odataRoutes.Any() || !odatacontrollers.Any()) return;
            var odatamethods = new[] { "Get", "Put", "Post", "Delete" };

            var route = odataRoutes.FirstOrDefault() as ODataRoute;

            foreach (var odataContoller in odatacontrollers)  // this is all of the OData controllers in your API
            {
                var methods = odataContoller.GetMethods().Where(a => odatamethods.Contains(a.Name)).ToList();
                if (!methods.Any())
                    continue; // next controller -- this one doesn't have any applicable methods

                foreach (var method in methods)  // this is all of the methods for a SINGLE controller (e.g. GET, POST, PUT, etc)
                {
                    var path = "/" + route.RoutePrefix + "/" + odataContoller.Name.Replace("Controller", "");
                    var odataPathItem = new PathItem();
                    var op = new Operation();

                    // This is assuming that all of the odata methods will be listed under a heading called OData in the swagger doc
                    op.tags = new List<string> { "OData" };
                    op.operationId = "OData_" + odataContoller.Name.Replace("Controller", "");

                    // This should probably be retrieved from XML code comments....
                    op.summary = "Summary for your method / data";
                    op.description = "Here is where we go deep into the description and options for the call.";

                    op.consumes = new List<string>();
                    op.produces = new List<string> { "application/atom+xml", "application/json", "text/json", "application/xml", "text/xml" };
                    op.deprecated = false;

                    var response = new Response() { description = "OK" };
                    response.schema = new Schema { type = "array", items = schemaRegistry.GetOrRegister(method.ReturnType) };
                    op.responses = new Dictionary<string, Response> { { "200", response } };

                    var security = GetSecurityForOperation(odataContoller);
                    if (security != null)
                        op.security = new List<IDictionary<string, IEnumerable<string>>> { security };

                    odataPathItem.get = op;   // this needs to be a switch based on the method name
                    swaggerDoc.paths.Add(path, odataPathItem);
                }
            }
        }

        private Dictionary<string, IEnumerable<string>> GetSecurityForOperation(MemberInfo odataContoller)
        {
            Dictionary<string, IEnumerable<string>> securityEntries = null;
            if (odataContoller.GetCustomAttribute(typeof(AuthorizeAttribute)) != null)
            {
                securityEntries = new Dictionary<string, IEnumerable<string>> { { "oauth2", new[] { "actioncenter" } } };
            }
            return securityEntries;
        }
    }
}
using MembershipCashierW.Properties;
using Swashbuckle.Swagger;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Description;

namespace MembershipCashierW.Code.Swagger
{
    public class AddAuthenticationTokenHeaderParameter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.parameters == null)
                operation.parameters = new List<Parameter>();

            var allAfTokens = SessionGlobal.GetServiceAntiforgeryTokens();

            string afToken = allAfTokens.Skip(Settings.Default.PersistantAntiforgeryTokens.Count).FirstOrDefault();
            if (string.IsNullOrEmpty(afToken))
                afToken = allAfTokens.LastOrDefault();

            operation.parameters.Add(new Parameter
            {
                name = "RequestVerificationToken",
                @in = "header",
                type = "string",
                required = true,
                @default = afToken
            });
        }
    }
}
using MembershipCashierW.Code.Documents.Generators;
using MembershipCashierW.Controllers.ControllerBase;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;

namespace MembershipCashierW.Controllers
{
    public class LocationDocumentController : WebApiControllerBase
    {
        public enum DocumentType : byte
        {
            TRANSACTIONS = 0,
            ARREARS = 1,
            USER_BALANCES = 2
        }

        public HttpResponseMessage Get([FromUri] DateTime from, [FromUri] DateTime to, [FromUri] DocumentType[] docTypes)
        {
            return Execute(delegate
            {
                var sb = new StringBuilder();
                foreach (var docType in docTypes)
                {
                    switch (docType)
                    {
                        case DocumentType.TRANSACTIONS:
                            var generator = new LocationTransactionsReportGenerator(Db, SecurityDb);
                            generator.LocationIds = new int[] { SessionGlobal.CurrentLocation.LocationId };
                            sb.Append(generator.GetReportHTML(from, to));
                            break;
                        case DocumentType.ARREARS:
                            var generatorA = new CustomerArrearsByLocationReportGenerator(Db, CustomerArrearsByLocationReportGenerator.ModeOptions.ARREARS);
                            generatorA.LocationIds = new int[] { SessionGlobal.CurrentLocation.LocationId };
                            sb.Append(generatorA.GetReportHTML(from, to));
                            break;
                        case DocumentType.USER_BALANCES:
                            var generatorB = new CustomerArrearsByLocationReportGenerator(Db, CustomerArrearsByLocationReportGenerator.ModeOptions.BALANCES);
                            generatorB.LocationIds = new int[] { SessionGlobal.CurrentLocation.LocationId };
                            sb.Append(generatorB.GetReportHTML(from, to));
                            break;
                    }
                }

                var response = new HttpResponseMessage { Content = new StringContent(sb.ToString()) };
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
                return response;
            });
        }
    }
}
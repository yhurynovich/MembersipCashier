using MembershipCashierW.Code.Documents.Generators;
using MembershipCashierW.Controllers.ControllerBase;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;

namespace MembershipCashierW.Controllers
{
    public class UserDocumentController : UserProfileControllerBase
    {
        public enum DocumentType : byte
        {
            INVOICE = 0,
            RECEIPT = 1
        }

        public HttpResponseMessage Get([FromUri] int userId, [FromUri] DocumentType[] docTypes)
        {
            return Execute(delegate
            {
                var sb = new StringBuilder();
                foreach (var docType in docTypes)
                {
                    switch (docType)
                    {
                        case DocumentType.INVOICE:
                            var generator = new UserInvoiceGenerator(Db);
                            generator.UserId = userId;
                            generator.LocationIds = new int[] { SessionGlobal.CurrentLocation.LocationId };
                            sb.Append(generator.GetReportHTML());
                            break;
                        case DocumentType.RECEIPT:
                            var generatorR = new UserReceiptGenerator(Db);
                            generatorR.UserId = userId;
                            generatorR.LocationIds = new int[] { SessionGlobal.CurrentLocation.LocationId };
                            sb.Append(generatorR.GetReportHTML());
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
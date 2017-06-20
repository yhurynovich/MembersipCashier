using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SecurityUnified.AllowedApplications
{
    public class WCFCustomAuthorizationManager : ServiceAuthorizationManager
    {
        private string[] allowedAppIdHashStrings;
        private IEnumerable<string> allowedAppIds;
        private SHA256 shaM = new SHA256Managed();

        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            string clientAppIdHash = WCFAApMessageInspector.GetReceivedClientAppIdHash( WebOperationContext.Current.IncomingRequest.Headers);

            if (clientAppIdHash != null && allowedAppIdHashStrings != null && !allowedAppIdHashStrings.Contains(clientAppIdHash))
            {
                 return true;
            }
            else
            {
                WebOperationContext.Current.OutgoingResponse.Headers.Add("WWW-Authenticate: Basic realm=\"WCFCustomAuthorizationManager\"");
                throw new WebFaultException(HttpStatusCode.Unauthorized);
            }
        }

        public WCFCustomAuthorizationManager() : this(new string[] { "СтарухаОткройДверь" })
        {
        }

        public WCFCustomAuthorizationManager(IEnumerable<string> allowedAppIds)
        {
            this.allowedAppIds = allowedAppIds;

            allowedAppIdHashStrings = allowedAppIds.Select(a => Convert.ToBase64String(shaM.ComputeHash(Encoding.UTF8.GetBytes(a)))).ToArray();
        }
    }
}

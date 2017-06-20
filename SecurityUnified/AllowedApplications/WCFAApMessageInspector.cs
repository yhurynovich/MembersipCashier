using SecurityUnified.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Net;

namespace SecurityUnified.AllowedApplications
{
    public class WCFAApMessageInspector : IClientMessageInspector, IDispatchMessageInspector, IDisposable
    {
        public const string CLIENT_APP_ID = "CLIENT_APP_ID";

        private string clientAppId;
        private IEnumerable<string> allowedAppIds;
        private string[] allowedAppIdHashStrings;
        private SHA256 shaM = new SHA256Managed();

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            HttpRequestMessageProperty httpRequestMessage;
            object httpRequestMessageObject;

                if (request.Properties.TryGetValue(HttpRequestMessageProperty.Name, out httpRequestMessageObject))
                {
                    httpRequestMessage = httpRequestMessageObject as HttpRequestMessageProperty;
                    if (string.IsNullOrEmpty(httpRequestMessage.Headers[CLIENT_APP_ID]))
                    {
                        httpRequestMessage.Headers[CLIENT_APP_ID] = Convert.ToBase64String( shaM.ComputeHash(Encoding.UTF8.GetBytes(clientAppId)) );
                    }
                }
                else
                {
                    httpRequestMessage = new HttpRequestMessageProperty();
                    httpRequestMessage.Headers.Add(CLIENT_APP_ID, Convert.ToBase64String(shaM.ComputeHash(Encoding.UTF8.GetBytes(clientAppId))));
                    request.Properties.Add(HttpRequestMessageProperty.Name, httpRequestMessage);
                }

            return null;
        }

        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            object httpRequestMessageObject;
            HttpRequestMessageProperty httpRequestMessage;
            string clientAppIdHash = null;

            if (request.Properties.TryGetValue(HttpRequestMessageProperty.Name, out httpRequestMessageObject))
            {
                httpRequestMessage = httpRequestMessageObject as HttpRequestMessageProperty;
                clientAppIdHash = GetReceivedClientAppIdHash(httpRequestMessage.Headers);
            }

            if (!allowedAppIdHashStrings.Contains(clientAppIdHash))
                throw new Xxception("Client application is not allowed to make calls to this WCF service");

            return httpRequestMessageObject;
        }

        internal static string GetReceivedClientAppIdHash(WebHeaderCollection headers)
        {
            return headers[CLIENT_APP_ID];
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            
        }


        public WCFAApMessageInspector(string clientAppId)
        {
            this.clientAppId = clientAppId;
        }

        public WCFAApMessageInspector(IEnumerable<string> allowedAppIds)
        {
            this.allowedAppIds = allowedAppIds;

            allowedAppIdHashStrings = allowedAppIds.Select(a => Convert.ToBase64String(shaM.ComputeHash(Encoding.UTF8.GetBytes(a)))).ToArray();
        }

        public void Dispose()
        {
            if (shaM != null)
            {
                shaM.Dispose();
                shaM = null;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace SecurityUnified.AllowedApplications
{
    public class WCFAApBehavior : BehaviorExtensionElement, IEndpointBehavior
    {
        private string clientAppId;
        private IEnumerable<string> allowedAppIds;

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
            //endpoint.Behaviors.Add(new MyBehavior());
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            WCFAApMessageInspector inspector = new WCFAApMessageInspector(clientAppId);
            clientRuntime.MessageInspectors.Add(inspector);
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            WCFAApMessageInspector inspector = new WCFAApMessageInspector(allowedAppIds);
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(inspector);
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }

        protected override object CreateBehavior()
        {
            return new WCFAApBehavior(clientAppId, allowedAppIds);
        }

        public override Type BehaviorType
        {
            get
            {
                return this.GetType();
            }
        }

        public WCFAApBehavior(string clientAppId, IEnumerable<string> allowedAppIds)
        {
            this.clientAppId = clientAppId;
            this.allowedAppIds = allowedAppIds;
        }
    }
}

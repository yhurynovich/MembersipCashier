using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace MembershipCashierDL.Tests
{
    public class HostForServices : IDisposable
    {
        private static HostForServices instance = new HostForServices();

        public static HostForServices Instance
        {
            get { return HostForServices.instance; }
        }

        private ServiceHost host;
        private ServiceHost host2;

        public void StartServices()
        {
            if (host == null || host.State == CommunicationState.Faulted || host.State == CommunicationState.Closing || host.State == CommunicationState.Closed)
            {
                
                host = new ServiceHost(typeof(MembershipCashierDL.Access.LowLevelAccess), new Uri("http://localhost:9812"));
                // Enable metadata publishing.
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                host.Description.Behaviors.Add(smb);

                #region MembershipCashierDL
                host.AddServiceEndpoint(typeof(MembershipCashierDL.Access.ILowLevelAccess), new BasicHttpBinding(), "LowLevelAccess");
                host.Description.Endpoints[0].Name = "LowLevelForMembershipCashierDL";
                host.Description.Endpoints[0].Contract.Name = "LowLevelForMembershipCashierDL";
                #endregion

                 // Open the ServiceHost to start listening for messages. Since
                // no endpoints are explicitly configured, the runtime will create
                // one endpoint per base address for each service contract implemented
                // by the service.
                host.Open();           
            }

            if (host2 == null || host2.State == CommunicationState.Faulted || host2.State == CommunicationState.Closing || host2.State == CommunicationState.Closed)
            {
                host2 = new ServiceHost(typeof(SecurityDL.Access.LowLevelAccess), new Uri("http://localhost:9811"));
                // Enable metadata publishing.
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                host2.Description.Behaviors.Add(smb);

                #region SecurityDL
                host2.AddServiceEndpoint(typeof(SecurityDL.Access.ILowLevelAccess), new BasicHttpBinding(), "SecurityLowLevelAccess");
                host2.Description.Endpoints[0].Name = "LowLevelForSecurityDL";
                host2.Description.Endpoints[0].Contract.Name = "LowLevelForSecurityDL";
                #endregion

                host2.Open(); 
            }
        }

        public void Dispose()
        {
            if (host != null)
                host.Close();
            if (host2 != null)
                host2.Close();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel.Configuration;

namespace SecurityUnified.AllowedApplications
{
    /*
     This extension must then be added to the service model’s configuration section for extensions:

<system.serviceModel>
…
  <extensions>
    <behaviorExtensions>
      <add name=“httpUserAgent“ type=“SecurityUnified.AllowedApplications.WCFAApHttpUserAgentBehaviorExtensionElement, SecurityUnified, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null“ />
    </behaviorExtensions>
  </extensions>
…
</system.serviceModel>
Note: We must use the full assembly qualified name here, otherwise it won’t work.

Now we need to apply our behavior extension to a behavior configuration:

<system.serviceModel>
…
  <behaviors>
    <endpointBehaviors>
      <behavior name=“LegacyServiceEndpointBehavior“>
        <httpUserAgent ClientAppId=“secret“ AllowedAppIds="secret" />
      </behavior>
    </endpointBehaviors>
  </behaviors>
…
</system.serviceModel>
    */
    public class WCFAApHttpUserAgentBehaviorExtensionElement : BehaviorExtensionElement
    {
        public override Type BehaviorType
        {
            get
            {
                return typeof(WCFAApBehavior);
            }
        }

        protected override object CreateBehavior()
        {
            return new WCFAApBehavior(ClientAppId, ParseAllowedAppIds());
        }

        [ConfigurationProperty("ClientAppId", IsRequired = false)]
        public string ClientAppId
        {
            get { return (string)base["ClientAppId"]; }
            set { base["ClientAppId"] = value; }
        }

        [ConfigurationProperty("AllowedAppIds", IsRequired = false)]
        public string AllowedAppIds
        {
            get { return (string)base["AllowedAppIds"]; }
            set { base["AllowedAppIds"] = value; }
        }

        public IEnumerable<string> ParseAllowedAppIds()
        {
            if (string.IsNullOrWhiteSpace(AllowedAppIds))
                return null;

            return AllowedAppIds.Split(',');
        }
    }
}

using Newtonsoft.Json;
using SecurityUnified.Interfaces;
using System.Runtime.Serialization;

namespace SecurityUnified.Contracts
{
    [DataContract]
    [KnownType(typeof(WebPagesMembershipImplementor))]
    public class WebPagesMembershipContract : IHasWebPagesMembership
    {
        [DataMember(Name = "WebPagesMembership")]
        private WebPagesMembershipImplementor _webPagesMembership;

        [JsonIgnore]
        public IWebPagesMembership WebPagesMembership
        {
            get { return (_webPagesMembership as IWebPagesMembership); }
            set { var x = new WebPagesMembershipImplementor(); value.CopyTo(x); _webPagesMembership = x; }
        }
    }
}

using Newtonsoft.Json;
using SecurityUnified.Interfaces;
using System.Runtime.Serialization;

namespace SecurityUnified.Contracts
{
    [DataContract]
    [KnownType(typeof(WebpagesUsersInRolesImplementor))]
    public class WebpagesUsersInRolesContract : IHasWebpagesUsersInRoles
    {
        [DataMember(Name = "WebpagesUsersInRoles")]
        private WebpagesUsersInRolesImplementor _webpagesUsersInRoles;

        [JsonIgnore]
        public IWebpagesUsersInRoles WebpagesUsersInRoles
        {
            get { return (_webpagesUsersInRoles as IWebpagesUsersInRoles); }
            set { var x = new WebpagesUsersInRolesImplementor(); value.CopyTo(x); _webpagesUsersInRoles = x; }
        }
    }
}

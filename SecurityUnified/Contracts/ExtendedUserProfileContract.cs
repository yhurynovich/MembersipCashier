using SecurityUnified.Interfaces;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using Newtonsoft.Json;

namespace SecurityUnified.Contracts
{
    [DataContract]
    public class ExtendedUserProfileContract : UserProfileContract, IHasUserProfile
    {
        [DataMember(Name = "WebPagesMembership")]
        private WebPagesMembershipImplementor _webPagesMembership;
        [DataMember(Name = "UserRoles")]
        private IEnumerable<RoleImplementor> _user_roles;

        [JsonIgnore]
        public IWebPagesMembership WebPagesMembership
        {
            get { return (_webPagesMembership as IWebPagesMembership); }
            set { var x = new WebPagesMembershipImplementor(); value.CopyTo(x); _webPagesMembership = x; }
        }

        [JsonIgnore]
        public IEnumerable<IRole> UserRoles
        {
            get { return (_user_roles as IEnumerable<IRole>); }
            set {
                var ret = new List<RoleImplementor>(value.Count());
                var exectedType = typeof(RoleImplementor);
                foreach (var role in value)
                {
                    if (exectedType.IsInstanceOfType(role))
                        ret.Add((RoleImplementor)role);
                    else
                    {
                        var x = new RoleImplementor();
                        role.CopyTo(x);
                        ret.Add(x);
                    }
                }
                _user_roles = ret;
            }
        }
    }
}
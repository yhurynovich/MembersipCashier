using MembershipCashierUnified.Interfaces;
using Newtonsoft.Json;
using SecurityUnified.Contracts;
using SecurityUnified.Interfaces;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using SecurityUnified;

namespace MembershipCashierUnified.Contracts
{
    public class UserProfile2Contract : IHasUserProfile2
    {
        [DataMember(Name = "UserProfile2")]
        private UserProfile2Implementor _userProfile2;

        [DataMember(Name = "WebPagesMembership")]
        private WebPagesMembershipImplementor _webPagesMembershipContract;

        [DataMember(Name = "UserRoles")]
        private IEnumerable<RoleImplementor> _user_roles;

        [JsonIgnore]
        public IWebPagesMembership WebPagesMembership
        {
            get { return (_webPagesMembershipContract as IWebPagesMembership); }
            set { var x = new WebPagesMembershipImplementor(); value.CopyTo(x); _webPagesMembershipContract = x; }
        }

        [JsonIgnore]
        public IEnumerable<IRole> UserRoles
        {
            get { return (_user_roles as IEnumerable<IRole>); }
            set
            {
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

        [JsonIgnore]
        public IUserProfile2 UserProfile2
        {
            get { return (_userProfile2 as IUserProfile2); }
            set { var x = new UserProfile2Implementor(); value.CopyTo(x); _userProfile2 = x; }
        }
    }
}

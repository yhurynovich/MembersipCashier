using Newtonsoft.Json;
using SecurityUnified.Interfaces;
using System.Runtime.Serialization;

namespace SecurityUnified.Contracts
{
    [DataContract]
    public class UserProfileAuditContract : IHasUserProfileAudit
    {
        [DataMember(Name = "UserProfileAudit")]
        private UserProfileAuditImplementor _userProfileAudit;

        [JsonIgnore]
        public IUserProfileAudit UserProfileAudit
        {
            get { return (_userProfileAudit as IUserProfileAudit); }
            set { var x = new UserProfileAuditImplementor(); value.CopyTo(x); _userProfileAudit = x; }
        }
    }
}

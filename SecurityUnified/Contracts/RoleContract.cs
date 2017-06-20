using Newtonsoft.Json;
using SecurityUnified.Interfaces;
using System.Runtime.Serialization;

namespace SecurityUnified.Contracts
{
    [DataContract]
    [KnownType(typeof(RoleImplementor))]
    public class RoleContract : IHasRole
    {
        [DataMember(Name = "Role")]
        private RoleImplementor _role;

        [JsonIgnore]
        public IRole Role
        {
            get { return (_role as IRole); }
            set { var x = new RoleImplementor(); value.CopyTo(x); _role = x; }
        }
    }
}

using SecurityUnified.Interfaces;
using System.Runtime.Serialization;

namespace SecurityUnified.Contracts
{
    [DataContract]
    [KnownType(typeof(WebpagesUsersInRolesContract))]
    public class WebpagesUsersInRolesImplementor : IWebpagesUsersInRoles
    {
        [DataMember]
        public int RoleId { get; set; }
        [DataMember]
        public int UserId { get; set; }
    }
}

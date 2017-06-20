using SecurityUnified.Interfaces;
using System.Runtime.Serialization;

namespace SecurityUnified.Contracts
{
    [KnownType(typeof(RoleContract))]
    public class RoleImplementor : IRole
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; }
    }
}

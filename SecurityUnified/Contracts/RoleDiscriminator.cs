using SecurityUnified.Interfaces;
using System.Runtime.Serialization;

namespace SecurityUnified.Contracts
{
    [KnownType(typeof(RoleImplementor))]
    [KnownType(typeof(RoleContract))]
    public class RoleDiscriminator : DataDiscriminator<IRole>
    {
    }
}

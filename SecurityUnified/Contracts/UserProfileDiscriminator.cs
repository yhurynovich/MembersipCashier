using SecurityUnified.Interfaces;
using System.Runtime.Serialization;

namespace SecurityUnified.Contracts
{
    [KnownType(typeof(UserProfileImplementor))]
    [KnownType(typeof(UserProfileContract))]
    public class UserProfileDiscriminator : DataDiscriminator<IUserProfile>
    {
    }
}

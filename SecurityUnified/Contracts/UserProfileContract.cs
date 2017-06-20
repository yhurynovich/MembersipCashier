using Newtonsoft.Json;
using SecurityUnified.Interfaces;
using System.Runtime.Serialization;

namespace SecurityUnified.Contracts
{
    [DataContract]
    [KnownType(typeof(UserProfileImplementor))]
    public class UserProfileContract : IHasUserProfile
    {
        [DataMember(Name = "UserProfile")]
        private UserProfileImplementor _userProfile;

        [JsonIgnore]
        public IUserProfile UserProfile
        {
            get { return (_userProfile as IUserProfile); }
            set { var x = new UserProfileImplementor(); value.CopyTo(x); _userProfile = x; }
        }
    }
}

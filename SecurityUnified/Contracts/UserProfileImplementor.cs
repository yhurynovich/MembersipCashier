using SecurityUnified.Interfaces;
using System.Runtime.Serialization;

namespace SecurityUnified.Contracts
{
    [DataContract]
    [KnownType(typeof(UserProfileContract))]
    public class UserProfileImplementor : IUserProfile
    {
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public byte UserStatusId { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string EmailId { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string Phone { get; set; }
    }
}

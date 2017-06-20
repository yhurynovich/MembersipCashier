using SecurityUnified.Interfaces;
using System;
using System.Runtime.Serialization;

namespace SecurityUnified.Contracts
{
    [DataContract]
    public class UserProfileAuditImplementor : IUserProfileAudit
    {
        [DataMember]
        public int AuditId { get; set; }
        [DataMember]
        public int ModifiedBy { get; set; }
        [DataMember]
        public DateTime ModificationTime { get; set; }
        [DataMember]
        public byte Action { get; set; }
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

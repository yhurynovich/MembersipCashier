
using System.Runtime.Serialization;
namespace SecurityUnified.Interfaces
{
    public interface IUserProfileAudit : IUserProfile
    {
        [DataMember]
         int AuditId { get; set; }
        [DataMember]
         int ModifiedBy { get; set; }
        [DataMember]
         System.DateTime ModificationTime { get; set; }
        [DataMember]
         byte Action { get; set; }
         //int UserId { get; set; }
         //byte UserStatusId { get; set; }
         //string UserName { get; set; }
         //string EmailId { get; set; }
         //string FirstName { get; set; }
         //string LastName { get; set; }
    }
}

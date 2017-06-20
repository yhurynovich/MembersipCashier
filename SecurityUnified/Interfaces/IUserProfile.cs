using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace SecurityUnified.Interfaces
{
    public interface IUserProfile : IHasUserIdAndUserName, IHasFirstName, IHasLastName, IHasPhone
    {
        //[DataMember]
        //int UserId { get; set; }
        [DataMember]
        byte UserStatusId { get; set; }
        //[Required]
        //[DataMember]
        //string UserName { get; set; }
        [DataMember]
        string EmailId { get; set; }
        //[DataMember]
        //string FirstName { get; set; }
        //[DataMember]
        //string LastName { get; set; }
    }
}

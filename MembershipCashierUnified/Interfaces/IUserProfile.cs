using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MembershipCashierUnified.Interfaces
{
    public interface IUserProfile
    {
        [DataMember]
        int UserId { get; set; }
        [DataMember]
        byte UserStatusId { get; set; }
        [DataMember]
        string UserName { get; set; }
        [DataMember]
        string EmailId { get; set; }
        [DataMember]
        string FirstName { get; set; }
        [DataMember]
        string LastName { get; set; }
    }
}

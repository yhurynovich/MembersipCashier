using System.Runtime.Serialization;

namespace MembershipCashierUnified.Interfaces
{
    public interface IUserProfile2 : SecurityUnified.Interfaces.IUserProfile, IUserProfileWithLDAP
    {
        [DataMember]
        System.Data.Linq.Binary Photo { get; set; }
    }
}

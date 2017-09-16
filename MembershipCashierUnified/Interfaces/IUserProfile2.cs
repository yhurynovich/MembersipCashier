using System.Runtime.Serialization;

namespace MembershipCashierUnified.Interfaces
{
    public interface IUserProfile2 : SecurityUnified.Interfaces.IUserProfile, IUserProfileWithLDAP
    {
        [DataMember]
        string Photo { get; set; }
        [DataMember]
        string PersonalId { get; set; }
    }
}

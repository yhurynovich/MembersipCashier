using SecurityUnified.Interfaces;

namespace MembershipCashierUnified.Interfaces
{
    public interface IUserProfileWithLDAP : IUserProfile
    {
        string LdapAccount { get; set; }
    }
}

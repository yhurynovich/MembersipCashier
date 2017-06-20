
using SecurityUnified.Interfaces;
namespace MembershipCashierUnified.Interfaces
{
    public interface IHasUserProfile
    {
        IUserProfile UserProfile { get; set; }
    }
}

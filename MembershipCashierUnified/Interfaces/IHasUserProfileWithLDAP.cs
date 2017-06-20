namespace MembershipCashierUnified.Interfaces
{
    public interface IHasUserProfileWithLDAP
    {
        IUserProfileWithLDAP UserProfileWithLDAP { get; set; }
    }
}

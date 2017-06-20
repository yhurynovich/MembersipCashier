
namespace SecurityUnified.Interfaces
{
    public interface IRole : IHasRoleId
    {
        string RoleName { get; set; }
    }
}

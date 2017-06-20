using SecurityUnified.Contracts;
using SecurityUnified.Interfaces;
using System.ServiceModel;

namespace SecurityDL.Access
{
    [ServiceContract]
    public interface ILowLevelAccess
    {
        [OperationContract]
        RoleContract[] GetUserRoles(IHasUserId user);
        [OperationContract]
        void AddRoleToUser(IHasUserId user, RoleContract[] roles, int modifiedByUserId);
        [OperationContract]
        UserProfileContract[] GetUsersInRole(RoleDiscriminator d);
        [OperationContract]
        RoleContract[] GetRolesForUser(UserProfileDiscriminator d);

        #region Find
        [OperationContract]
        UserProfileContract[] FindUserProfile(UserProfileDiscriminator d);
        [OperationContract]
        ExtendedUserProfileContract[] FindExtendedUserProfile(UserProfileDiscriminator d);
        [OperationContract]
        RoleContract[] FindRole(RoleDiscriminator d);
        [OperationContract]
        WebPagesMembershipContract[] FindWebPagesMembership(WebPagesMembershipDiscriminator d);
        #endregion

        #region Update
        [OperationContract]
        void UpdateUserProfile(UserProfileContract[] d, int modifiedByUserId, bool allowDefaultValues = true);
        [OperationContract]
        void UpdateWebPagesMembership(WebPagesMembershipContract[] d);
        #endregion

        #region Insert
        [OperationContract]
        void InsertUserProfile(ExtendedUserProfileContract[] d);

        [OperationContract]
        void InsertWebpagesUsersInRole(WebpagesUsersInRolesContract[] d);
        #endregion

        [OperationContract]
        void AddUsersToRoles(string[] usernames, string[] roleNames);
    }
}

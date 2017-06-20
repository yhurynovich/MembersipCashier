using MembershipCashierDL.Access;
using MembershipCashierW.Code;
using MembershipCashierW.Controllers.Authorization;
using MembershipCashierW.Models;
using SecurityUnified;
using SecurityUnified.Contracts;
using SecurityUnified.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using WebMatrix.WebData;

namespace MembershipCashierW.App_Start
{
    public static class RoleConfig
    {
        public static void InitRoles()
        {
            var definedRoles = Roles.Provider.GetAllRoles();
            if (!definedRoles.Contains(SecurityUnified.Constants.SUPER_ADMIN))
                Roles.Provider.CreateRole(SecurityUnified.Constants.SUPER_ADMIN);

            if (!definedRoles.Contains(SecurityUnified.Constants.ADMIN))
                Roles.Provider.CreateRole(SecurityUnified.Constants.ADMIN);

            //if (!definedRoles.Contains(Constants.SUPERVISING_USER))
            //    Roles.Provider.CreateRole(Constants.SUPERVISING_USER);

            //if (!definedRoles.Contains(Constants.USER_WITH_REPORTS))
            //    Roles.Provider.CreateRole(Constants.USER_WITH_REPORTS);

            //if (!definedRoles.Contains(Constants.REGULAR_USER))
            //    Roles.Provider.CreateRole(Constants.REGULAR_USER);

            InitSystemAccount();
        }

        //[RoleAuthorize(Roles = Constants.SUPER_ADMIN)]
        //[ValidateAntiForgeryToken]
        internal static void InitSystemAccount()
        {
            if (Membership.Provider.GetUser(SecurityUnified.Constants.SYSTEM, false) == null)
                WebSecurity.CreateUserAndAccount(SecurityUnified.Constants.SYSTEM, System.Guid.NewGuid().ToString(), new { UserStatusId = 0, FirstName = SecurityUnified.Constants.SYSTEM, LastName = SecurityUnified.Constants.SYSTEM });

            if (!Roles.Provider.GetRolesForUser(SecurityUnified.Constants.SYSTEM).Any(r => r == SecurityUnified.Constants.SUPER_ADMIN))
            {
                Roles.Provider.AddUsersToRoles(new string[] { SecurityUnified.Constants.SYSTEM }, new string[] { SecurityUnified.Constants.SUPER_ADMIN });
            }
        }

        [RoleAuthorize(Roles = SecurityUnified.Constants.ALL_SUPER_ADMINS)]
        internal static void InitSuperAdmin(string userName)
        {
            if (userName == SecurityUnified.Constants.SYSTEM)
                throw new Exception("System role is handled separately");

            if (userName.Contains("admin"))
            {
                //foreach (string role in AppGlobal.SuperAdminRoles.Where(r => r != Constants.SYSTEM))
                //{
                    if (!string.IsNullOrWhiteSpace(userName) &&
                         Roles.Provider.GetUsersInRole(SecurityUnified.Constants.SUPER_ADMIN).Count() < 2) //First admin is the system
                    {
                        var profile = ServiceAccessor.SecurityDb.FindUserProfile(new UserProfileDiscriminator() { Filter = p => p.UserName == userName }).Single();
                        IHasUserId updatedBy = null;
                        if(SessionGlobal.CurrentUser!=null)
                            updatedBy = SessionGlobal.CurrentUser.Identity;
                        if(updatedBy == null)
                        {
                            updatedBy = new UserProfileImplementor();
                            ServiceAccessor.SecurityDb.FindUserProfile(new UserProfileDiscriminator() { Filter = p => p.UserName == SecurityUnified.Constants.SYSTEM }).Single().UserProfile.CopyTo(updatedBy);
                        }

                        ServiceAccessor.SecurityDb.AddRoleToUser(profile.UserProfile, new RoleContract[] { }, updatedBy.UserId);
                    }
                //}
            }
        }

        //static RoleConfig()
        //{
        //    try
        //    {
        //        new InitializeSimpleMembershipAttribute().OnActionExecuting(null);
        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.WriteToEventLog(ex);
        //    }
        //}
    }
}
using SecurityDL.Access;
using SecurityUnified.Contracts;
using SecurityUnified.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using WebMatrix.WebData;

namespace SecurityDL.Authentication
{
    public abstract class ServiceDrivenRoleProvider : SimpleRoleProvider
    {
        public abstract ILowLevelAccess Db { get; }

        public abstract IHasUserId Requestor { get; }

        public override string[] GetAllRoles()
        {
            return Db.FindRole(null).Select(r => r.Role.RoleName).ToArray();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            return Db.GetUsersInRole(new RoleDiscriminator() { Filter = x => x.RoleName == roleName }).Select(p => p.UserProfile.UserName).ToArray();
        }

        #region lastUserCache
        private KeyValuePair<string, string[]> lastUserCache;
        private object lastUserCache_lock = new object();
        #endregion

        public override string[] GetRolesForUser(string username)
        {
            string[] roles;
            var cached = lastUserCache;

            if (cached.Key == username)
                return cached.Value;

            roles = Db.GetRolesForUser(new UserProfileDiscriminator() { Filter = x => x.UserName.ToLower() == username.ToLower() }).Select(p => p.Role.RoleName).ToArray();
            lock (lastUserCache_lock)
            {
                lastUserCache = new KeyValuePair<string, string[]>(username, roles);
            }
            return roles;
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            var cached = lastUserCache;
            if (cached.Key == username)
                return cached.Value.Any(r => r == roleName);
            else
                return Db.GetRolesForUser(new UserProfileDiscriminator() { Filter = x => x.UserName.ToLower() == username.ToLower() }).Any(p => p.Role.RoleName == roleName);
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException(); // Unable to implement
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            Db.AddUsersToRoles(usernames, roleNames);
            lock (lastUserCache_lock)
            {
                lastUserCache = new KeyValuePair<string, string[]>();
            }
        }

        public override bool RoleExists(string roleName)
        {
            return Db.FindRole(null).Any(r => r.Role.RoleName == roleName);
        }
    }
}

using SecurityUnified.Interfaces;
using System;

namespace SecurityUnified
{
    public static class Extentions_CopyTo
    {
        public static void CopyTo(this IHasUserId from, IHasUserId to, bool allowDefaultValues = true)
        {
            to.UserId = from.UserId;
        }

        public static void CopyTo(this IHasFirstName from, IHasFirstName to, bool allowDefaultValues = true)
        {
            to.FirstName = from.FirstName;
        }

        public static void CopyTo(this IHasLastName from, IHasLastName to, bool allowDefaultValues = true)
        {
            to.LastName = from.LastName;
        }

        public static void CopyTo(this IHasUserName from, IHasUserName to, bool allowDefaultValues = true)
        {
            to.UserName = from.UserName;
        }

        public static void CopyTo(this IHasUserIdAndUserName from, IHasUserIdAndUserName to, bool allowDefaultValues = true)
        {
            (from as IHasUserId).CopyTo(to, allowDefaultValues);
            (from as IHasUserName).CopyTo(to, allowDefaultValues);
        }

        public static void CopyTo(this IHasPhone from, IHasPhone to, bool allowDefaultValues = true)
        {
            to.Phone = from.Phone;
        }

        public static void CopyTo(this IUserProfile from, IUserProfile to, bool allowDefaultValues = true)
        {
            (from as IHasUserIdAndUserName).CopyTo(to, allowDefaultValues);
            (from as IHasFirstName).CopyTo(to, allowDefaultValues);
            (from as IHasLastName).CopyTo(to, allowDefaultValues);
            (from as IHasPhone).CopyTo(to, allowDefaultValues);

            if (allowDefaultValues || !string.IsNullOrEmpty(from.EmailId))
                to.EmailId = from.EmailId;
            //if (allowDefaultValues || !string.IsNullOrEmpty(from.FirstName))
            //    to.FirstName = from.FirstName;
            //if (allowDefaultValues || !string.IsNullOrEmpty(from.LastName))
            //    to.LastName = from.LastName;
            if (allowDefaultValues || from.UserStatusId != byte.MinValue)
                to.UserStatusId = from.UserStatusId;
        }

        public static void CopyTo(this IUserProfileAudit from, IUserProfileAudit to, bool allowDefaultValues = true)
        {
            (from as IUserProfile).CopyTo(to, allowDefaultValues);
            to.AuditId = from.AuditId;
            if (allowDefaultValues || from.Action != byte.MinValue)
                to.Action = from.Action;
            if (allowDefaultValues || from.ModificationTime != DateTime.MinValue)
                to.ModificationTime = from.ModificationTime;
            if (allowDefaultValues || from.ModifiedBy != 0)
                to.ModifiedBy = from.ModifiedBy;
        }

        public static void CopyTo(this IWebPagesMembership from, IWebPagesMembership to, bool allowDefaultValues = true)
        {
            (from as IHasUserId).CopyTo(to, allowDefaultValues);
            if (allowDefaultValues || !string.IsNullOrEmpty(from.ConfirmationToken))
                to.ConfirmationToken = from.ConfirmationToken;
            if (allowDefaultValues || from.CreateDate.HasValue)
                to.CreateDate = from.CreateDate;
            if (allowDefaultValues || from.IsConfirmed.HasValue)
                to.IsConfirmed = from.IsConfirmed;
            if (allowDefaultValues || from.LastPasswordFailureDate.HasValue)
                to.LastPasswordFailureDate = from.LastPasswordFailureDate;
            if (allowDefaultValues || !string.IsNullOrEmpty(from.Password))
                to.Password = from.Password;
            if (allowDefaultValues || from.PasswordChangedDate.HasValue)
                to.PasswordChangedDate = from.PasswordChangedDate;
            if (allowDefaultValues || from.PasswordFailuresSinceLastSuccess != 0)
                to.PasswordFailuresSinceLastSuccess = from.PasswordFailuresSinceLastSuccess;
            if (allowDefaultValues || !string.IsNullOrEmpty(from.PasswordSalt))
                to.PasswordSalt = from.PasswordSalt;
            if (allowDefaultValues || !string.IsNullOrEmpty(from.PasswordVerificationToken))
                to.PasswordVerificationToken = from.PasswordVerificationToken;
            if (allowDefaultValues || from.PasswordVerificationTokenExpirationDate.HasValue)
                to.PasswordVerificationTokenExpirationDate = from.PasswordVerificationTokenExpirationDate;
            if (allowDefaultValues || from.PasswordChangedBy.HasValue)
                to.PasswordChangedBy = from.PasswordChangedBy;
        }

        public static void CopyTo(this IHasRoleId from, IHasRoleId to, bool allowDefaultValues = true)
        {
            to.RoleId = from.RoleId;
        }

        public static void CopyTo(this IRole from, IRole to, bool allowDefaultValues = true)
        {
            (from as IHasRoleId).CopyTo(to, allowDefaultValues);
            if (allowDefaultValues || !string.IsNullOrEmpty(from.RoleName))
                to.RoleName = from.RoleName;
        }

        public static void CopyTo(this IWebpagesUsersInRoles from, IWebpagesUsersInRoles to, bool allowDefaultValues = true)
        {
            (from as IHasUserId).CopyTo(to);
            (from as IHasRoleId).CopyTo(to);
        }
    }
}

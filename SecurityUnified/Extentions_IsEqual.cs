using SecurityUnified.Interfaces;
using System;

namespace SecurityUnified
{
    public static class Extentions_IsEqual
    {
        public static bool IsEqual(this IHasUserId from, IHasUserId to)
        {
            return to.UserId == from.UserId;
        }

        public static bool IsEqual(this IHasFirstName from, IHasFirstName to)
        {
            return to.FirstName == from.FirstName;
        }

        public static bool IsEqual(this IHasLastName from, IHasLastName to)
        {
            return to.LastName == from.LastName;
        }

        public static bool IsEqual(this IHasUserName from, IHasUserName to)
        {
            return to.UserName == from.UserName;
        }

        public static bool IsEqual(this IHasUserIdAndUserName from, IHasUserIdAndUserName to)
        {
            return (from as IHasUserId).IsEqual(to)
                    && (from as IHasUserName).IsEqual(to);
        }

        public static bool IsEqual(this IHasPhone from, IHasPhone to)
        {
            return to.Phone == from.Phone;
        }

        public static bool IsEqual(this IUserProfile from, IUserProfile to)
        {
            return (from as IHasUserIdAndUserName).IsEqual(to)
                && (from as IHasFirstName).IsEqual(to)
                && (from as IHasLastName).IsEqual(to)
                && (from as IHasPhone).IsEqual(to)
            && to.EmailId == from.EmailId
            && to.UserStatusId == from.UserStatusId;
        }

        public static bool IsEqual(this IUserProfileAudit from, IUserProfileAudit to)
        {
            return (from as IUserProfile).IsEqual(to)
            && to.AuditId == from.AuditId
            && to.Action == from.Action
            && to.ModificationTime == from.ModificationTime
            && to.ModifiedBy == from.ModifiedBy;
        }

        public static bool IsEqual(this IWebPagesMembership from, IWebPagesMembership to)
        {
            return (from as IHasUserId).IsEqual(to)
            && to.ConfirmationToken == from.ConfirmationToken
            && to.CreateDate == from.CreateDate
            && to.IsConfirmed == from.IsConfirmed
            && to.LastPasswordFailureDate == from.LastPasswordFailureDate
            && to.Password == from.Password
            && to.PasswordChangedDate == from.PasswordChangedDate
            && to.PasswordFailuresSinceLastSuccess == from.PasswordFailuresSinceLastSuccess
            && to.PasswordSalt == from.PasswordSalt
            && to.PasswordVerificationToken == from.PasswordVerificationToken
            && to.PasswordVerificationTokenExpirationDate == from.PasswordVerificationTokenExpirationDate
            && to.PasswordChangedBy == from.PasswordChangedBy;
        }

        public static bool IsEqual(this IRole from, IRole to)
        {
            return to.RoleId == from.RoleId
                && to.RoleName == from.RoleName;
        }
    }
}

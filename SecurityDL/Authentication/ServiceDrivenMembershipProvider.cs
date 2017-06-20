using SecurityDL.Access;
using SecurityUnified.Contracts;
using SecurityUnified.Enumerations;
using SecurityUnified.Interfaces;
using System;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Web.Security;
using System.Linq;

namespace SecurityDL.Authentication
{
    public abstract class ServiceDrivenMembershipProvider : MembershipProvider
    {
        public abstract ILowLevelAccess Db { get; }
        public abstract IHasUserId Requestor { get; }

        [Obsolete]
        public override MembershipUser CreateUser(
            string username,
            string password,
            string email,
            string passwordQuestion, //
            string passwordAnswer, //
            bool isApproved,
            object providerUserKey,
            out MembershipCreateStatus status)
        {

            username = username.ToLower();

            var args = new ValidatePasswordEventArgs(username, password, true);
            OnValidatingPassword(args);

            if (args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            if (RequiresUniqueEmail && GetUserNameByEmail(email) != string.Empty)
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

            var existingUser = GetUser(username, true);

            if (existingUser == null)
            {
                var profile = new ExtendedUserProfileContract()
                {
                    UserProfile = new UserProfileImplementor()
                    {
                        EmailId = email,
                        //FirstName = 
                        //LastName = 
                        //UserId = providerUserKey
                        UserName = username,
                        UserStatusId = isApproved ? Convert.ToByte(UserStatusOptions.Approved) : Convert.ToByte(UserStatusOptions.New)
                    }
                    ,
                    WebPagesMembership = new WebPagesMembershipImplementor()
                    {
                        //ConfirmationToken =
                        CreateDate = DateTime.UtcNow,
                        IsConfirmed = isApproved,
                        //LastPasswordFailureDate =
                        Password = password,
                        PasswordChangedBy = Requestor.UserId,
                        PasswordChangedDate = DateTime.UtcNow,
                        PasswordSalt = String.Empty,
                        //PasswordFailuresSinceLastSuccess
                        //PasswordVerificationToken = 
                        // PasswordVerificationTokenExpirationDate =
                        //UserId =  = providerUserKey
                    }
                };
                Db.InsertUserProfile(new ExtendedUserProfileContract[] { profile });
                status = MembershipCreateStatus.Success;

                return GetUser(username, true);
            }
            status = MembershipCreateStatus.DuplicateUserName;

            return null;
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            var user = Db.FindExtendedUserProfile(new UserProfileDiscriminator() { Filter = x => x.UserName == username }).FirstOrDefault();
            if (user != null)
            {
                var memUser = new MembershipUser(
                    "ServiceDrivenMembershipProvider"
                    , username
                    , user.UserProfile.UserId
                    , user.UserProfile.EmailId
                    , null
                    , null
                    , user.UserProfile.UserStatusId >= Convert.ToByte(UserStatusOptions.Approved)
                    , user.UserProfile.UserStatusId == Convert.ToByte(UserStatusOptions.Disabled)
                    , user.WebPagesMembership.CreateDate.HasValue ? user.WebPagesMembership.CreateDate.Value : DateTime.MinValue
                    , DateTime.MinValue
                    , DateTime.MinValue
                    , user.WebPagesMembership.PasswordChangedDate.HasValue ? user.WebPagesMembership.PasswordChangedDate.Value : DateTime.MinValue
                    , DateTime.UtcNow);
                return memUser;
            }
            return null;
        }

        public override int MinRequiredPasswordLength
        {
            get { return 6; }
        }

        public override bool ValidateUser(string username, string password)
        {
            username = username.ToLower();
            var md5Hash = GetMd5Hash(password);
            var requiredUser = Db.FindExtendedUserProfile(new UserProfileDiscriminator() { Filter = x => x.UserName == username }).FirstOrDefault(x => x.WebPagesMembership.Password == md5Hash);
            return requiredUser != null;
        }

        public static string GetMd5Hash(string value)
        {
            var md5Hasher = MD5.Create();
            var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(value));
            var sBuilder = new StringBuilder();
            for (var i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        #region NotImplemented

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }

        public override System.Web.Security.MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override System.Web.Security.MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override System.Web.Security.MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override System.Web.Security.MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override System.Web.Security.MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(System.Web.Security.MembershipUser user)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

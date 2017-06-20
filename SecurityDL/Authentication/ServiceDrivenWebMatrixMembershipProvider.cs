using SecurityDL.Access;
using SecurityDL.Properties;
using SecurityUnified.Contracts;
using SecurityUnified.Enumerations;
using SecurityUnified.Exceptions;
using SecurityUnified.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web.Helpers;
using System.Web.Security;
using WebMatrix.WebData;

namespace SecurityDL.Authentication
{
    public abstract class ServiceDrivenWebMatrixMembershipProvider : SimpleMembershipProvider
    {
        public abstract ILowLevelAccess Db { get; }

        public abstract IHasUserId Requestor { get; }

        private MembershipProvider PreviousProvider
        {
            get
            {
                return null;//base.PreviousProvider; //TODO: read private property
            }
        }

        public override System.Web.Security.MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            username = username.ToLower();
            status = MembershipCreateStatus.UserRejected;
            var args = new Dictionary<string, object>();
            args.Add("UserStatusId", status);
            args.Add("EmailId", email);
            //args.Add("FirstName", status);
            //args.Add("LastName", status);

            string un = CreateUserAndAccount(username, password, false, args);
            if (string.IsNullOrEmpty(un))
                return null;

            status = MembershipCreateStatus.Success;
            return GetUser(un, true);
        }

        //public override string CreateUserAndAccount(string userName, string password)
        //{
        //    return base.CreateUserAndAccount(userName, password);
        //}

        //public override string CreateUserAndAccount(string userName, string password, bool requireConfirmation)
        //{
        //    return base.CreateUserAndAccount(userName, password, requireConfirmation);
        //}

        public override string CreateUserAndAccount(string username, string password, bool requireConfirmation, IDictionary<string, object> values)
        {
            username = username.ToLower();
            var email = values.FirstOrDefault(x => x.Key == "EmailId");
            var firstName = values.FirstOrDefault(x => x.Key == "FirstName");
            var lastName = values.FirstOrDefault(x => x.Key == "LastName");
            var userStatusId = values.FirstOrDefault(x => x.Key == "UserStatusId");

            MembershipCreateStatus status;

            var args = new ValidatePasswordEventArgs(username, password, true);
            OnValidatingPassword(args);

            if (args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            if (RequiresUniqueEmail && GetUserNameByEmail(email.Value as string) != string.Empty)
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
                        EmailId = email.Value as string,
                        FirstName = firstName.Value as string,
                        LastName = lastName.Value as string,
                        //UserId = providerUserKey
                        UserName = args.UserName,
                        UserStatusId = Convert.ToByte(userStatusId.Value)
                    }
                    ,
                    WebPagesMembership = new WebPagesMembershipImplementor()
                    {
                        //ConfirmationToken =
                        CreateDate = DateTime.UtcNow,
                        IsConfirmed = !requireConfirmation || (Convert.ToByte(userStatusId.Value) != Convert.ToByte(UserStatusOptions.New)),
                        //LastPasswordFailureDate =
                        Password = Crypto.HashPassword( args.Password ),
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

                return username;
            }
            status = MembershipCreateStatus.DuplicateUserName;

            return null;
        }

        //public override string CreateUserAndAccount(string userName, string password, IDictionary<string, object> values)
        //{
        //    return base.CreateUserAndAccount(userName, password, values);
        //}

        public override MembershipUser GetUser(string userName, bool userIsOnline)
        {
            if (string.IsNullOrEmpty(userName))
                return null;

            userName = userName.ToLower();
            var user = Db.FindExtendedUserProfile(new UserProfileDiscriminator() { Filter = x => x.UserName.ToLower() == userName }).FirstOrDefault();

            if (user != null)
            {
                var memUser = new MembershipUser(
                    "ServiceDrivenWebMatrixMembershipProvider"
                    , user.UserProfile.UserName
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

        public MembershipUser GetUser(int userId, bool userIsOnline)
        {
            var user = Db.FindExtendedUserProfile(new UserProfileDiscriminator() { Filter = x => x.UserId == userId }).FirstOrDefault();

            if (user != null)
            {
                var memUser = new MembershipUser(
                    "ServiceDrivenWebMatrixMembershipProvider"
                    , user.UserProfile.UserName
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

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            if (typeof(int).IsInstanceOfType(providerUserKey))
            {
                int userId = Convert.ToInt32(providerUserKey);
                return GetUser(userId, userIsOnline);
            }
            else
            {
                string userName = Convert.ToString(providerUserKey);
                return GetUser(userName, userIsOnline);
            }
        }

        public override string GetUserNameFromId(int userId)
        {
            return GetUser(userId, false).UserName;
        }

        public override int GetUserIdFromOAuth(string provider, string providerUserId)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            return Db.FindExtendedUserProfile(new UserProfileDiscriminator() { Filter = x => x.EmailId == email }).Single().UserProfile.UserName;
        }

        public override int MinRequiredPasswordLength
        {
            get { return 6; }
        }

        public override bool ValidateUser(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException(Resources.Argument_Cannot_Be_Null_Or_Empty, "username");
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException(Resources.Argument_Cannot_Be_Null_Or_Empty, "password");
            }

            username = username.ToLower();
            var requiredUser = Db.FindExtendedUserProfile(new UserProfileDiscriminator() { Filter = x => x.UserName.ToLower() == username }).FirstOrDefault();
            if (requiredUser != null)
            {
                return Crypto.VerifyHashedPassword(requiredUser.WebPagesMembership.Password, password);
            }

            if (this.PreviousProvider != null)
                return this.PreviousProvider.ValidateUser(username, password);

            return false;
        }

        public override string GeneratePasswordResetToken(string userName, int tokenExpirationInMinutesFromNow)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("UserName can not be empty", "userName");
            }
            else
            {
                userName = userName.ToLower();
                var profiles = Db.FindExtendedUserProfile(new UserProfileDiscriminator() { Filter = x => x.UserName.ToLower() == userName });
                if (!profiles.Any())
                    throw new Xxception(string.Format("UserName was not located {0}",userName));
                if (profiles.Count()>1)
                    throw new Xxception(string.Format("Multiple users found that match given UserName {0}", userName));
                var p = profiles.First();
                if (p.WebPagesMembership == null)
                    throw new Xxception("Corrupt profile - missing membership"); 

                if(p.WebPagesMembership.PasswordVerificationTokenExpirationDate == null || p.WebPagesMembership.PasswordVerificationTokenExpirationDate < DateTime.UtcNow)
                {
                    var str = GenerateToken();

                    p.WebPagesMembership.PasswordVerificationToken = str;
                    if (tokenExpirationInMinutesFromNow < 1440)
                        tokenExpirationInMinutesFromNow = 1440;

                    p.WebPagesMembership.PasswordVerificationTokenExpirationDate = DateTime.UtcNow.AddMinutes((double)tokenExpirationInMinutesFromNow);
                    Db.UpdateWebPagesMembership(new WebPagesMembershipContract[] { new WebPagesMembershipContract() { WebPagesMembership = p.WebPagesMembership } });
                }
                return p.WebPagesMembership.PasswordVerificationToken;
            }
        }

        private static string GenerateToken()
        {
            using (RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider())
            {
                return GenerateToken(provider);
            }
        }

        internal static string GenerateToken(RandomNumberGenerator generator)
        {
            byte[] data = new byte[0x10];
            generator.GetBytes(data);
            return System.Web.HttpServerUtility.UrlTokenEncode(data);
        }

        public override string ResetPassword(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("UserName can not be empty", "userName");
            }
            else
            {
                userName = userName.ToLower();
                var p = Db.FindExtendedUserProfile(new UserProfileDiscriminator() { Filter = x => x.UserName.ToLower() == userName }).Single();
                if (p.WebPagesMembership == null)
                    throw new Xxception("Corrupt profile - missing membership");

                p.WebPagesMembership.PasswordChangedDate = DateTime.UtcNow;
                p.WebPagesMembership.PasswordChangedBy = null;
                p.WebPagesMembership.Password = Crypto.HashPassword(password);

                Db.UpdateWebPagesMembership(new WebPagesMembershipContract[] { new WebPagesMembershipContract() { WebPagesMembership = p.WebPagesMembership } });

                return password;
            }
        }

        public override bool ResetPasswordWithToken(string token, string newPassword)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("PasswordVerificationToken can not be empty", "userName");
            }
            else
            {
                var p = Db.FindWebPagesMembership(new WebPagesMembershipDiscriminator() { Filter = x => x.PasswordVerificationToken == token && x.PasswordVerificationTokenExpirationDate >= DateTime.UtcNow }).FirstOrDefault();
                if (p == null)
                    return false;

                p.WebPagesMembership.PasswordChangedDate = DateTime.UtcNow;
                p.WebPagesMembership.PasswordChangedBy = null;
                p.WebPagesMembership.Password = Crypto.HashPassword(newPassword);

                p.WebPagesMembership.PasswordVerificationToken = null;
                p.WebPagesMembership.PasswordVerificationTokenExpirationDate = DateTime.UtcNow.AddDays(7);
                p.WebPagesMembership.PasswordFailuresSinceLastSuccess = 0;
                p.WebPagesMembership.LastPasswordFailureDate = null;

                Db.UpdateWebPagesMembership(new WebPagesMembershipContract[] { new WebPagesMembershipContract() { WebPagesMembership = p.WebPagesMembership } });

                return true;
            }
        }

        public override int GetUserIdFromPasswordResetToken(string token)
        {
            var membership = Db.FindWebPagesMembership(new WebPagesMembershipDiscriminator() { Filter = x=>x.PasswordVerificationToken == token && x.PasswordVerificationTokenExpirationDate >= DateTime.UtcNow }).FirstOrDefault();
            if (membership == null || membership.WebPagesMembership == null)
                return default(int);

            return membership.WebPagesMembership.UserId;
        }

        public ServiceDrivenWebMatrixMembershipProvider()
            : base()
        {
            var field = this.GetType().GetProperty("InitializeCalled", System.Reflection.BindingFlags.FlattenHierarchy | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            field.SetValue(this, true);
        }
    }
}


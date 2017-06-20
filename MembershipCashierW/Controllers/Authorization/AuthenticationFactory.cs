using MembershipCashierDL.MixedContracts;
using MembershipCashierUnified.Contracts;
using MembershipCashierW.App_Start;
using MembershipCashierW.Code.Captcha;
using MembershipCashierW.Models;
using SecurityUnified.Enumerations;
using SecurityUnified.Exceptions;
using System;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Security.Principal;
using System.Web;
using WebMatrix.WebData;
using MembershipCashierUnified;

namespace MembershipCashierW.Controllers.Authorization
{
    public class AuthenticationFactory
    {
        #region Constructor

        #endregion

        #region Properties & Fields

        private static SecurityDL.Access.LowLevelAccess securitydb;
        private static MembershipCashierDL.Access.LowLevelAccess db;

        public SecurityDL.Access.LowLevelAccess SecurityDb
        {
            get
            {
                if (securitydb == null)
                    securitydb = new SecurityDL.Access.LowLevelAccess();
                return securitydb;
            }
        }

        public MembershipCashierDL.Access.LowLevelAccess Db
        {
            get
            {
                if (db == null)
                    db = new MembershipCashierDL.Access.LowLevelAccess();
                return db;
            }
        }

        #endregion

        #region Factory

        public bool Login(LoginModel model)
        {
            var sessionRetryCount = Convert.ToInt32(HttpContext.Current.Session["RetryCount"]);

            if (sessionRetryCount > model.RetryCount)
                model.RetryCount = sessionRetryCount;

            model.RetryCount++;

            HttpContext.Current.Session["RetryCount"] = model.RetryCount;

            string captcha = HttpContext.Current.Session["CaptchaImageText"] as string;

            if (model.RetryCount <= SecurityUnified.Constants.LOGIN_RETRY_COUNT || (!string.IsNullOrEmpty(captcha) && captcha == model.Captcha))
            {
                if (!WebSecurity.Initialized)
                    SecurityDL.Extentions.InitializeWebSecurity();

                if (!string.IsNullOrWhiteSpace(model.LdapAccount))
                {
                    if (LdapLogin(model))
                    {
                        return true;
                    }
                }

                if (WebFormLogin(model))
                {
                    return true;
                }

                return false;
            }

            //// If we got this far, something failed, redisplay form
            throw new UnauthorizedException(model.RetryCount > SecurityUnified.Constants.LOGIN_RETRY_COUNT);
        }

        private bool LdapLogin(LoginModel model)
        {
            model.LdapAccount = model.LdapAccount.Trim();

            var record = Db.FindUserProfile(new UserProfile2Discriminator() { Filter = x => x.LdapAccount == model.LdapAccount }).FirstOrDefault();

            if (record != null && ValidateLDAPAccess())
            {
                if (model.UserName != record.UserProfile2.UserName)
                {
                    model.UserName = record.UserProfile2.UserName;
                    model.Password = null;
                }
                System.Web.Security.FormsAuthentication.SetAuthCookie(model.UserName, false);

                PrepareSession(model, record);
                return true;
            }
            return false;
        }

        private bool ValidateLDAPAccess()
        {
            if (HttpContext.Current != null && HttpContext.Current.Request!=null)
            {
               var requestorDomain = Dns.GetHostEntry(Code.Utils.DetermineIP(HttpContext.Current.Request));
                if (requestorDomain.HostName.EndsWith("hitechmaster.net"))
                    return true;
            }

            //TODO: implement LDAP password validation here
            return false;
        }

        private bool WebFormLogin(LoginModel model)
        {
            model.UserName = model.UserName.Trim().ToLower();
            if (WebSecurity.Login(model.UserName, model.Password.Trim(), persistCookie: model.RememberMe))
            {
                var profile = Db.FindUserProfile(new UserProfile2Discriminator() { Filter = p => p.UserName.ToLower() == model.UserName }).Single();

                PrepareSession(model, profile);
                return true;
            }

            return false;
        }

        private void PrepareSession(LoginModel model, UserProfile2Contract profile)
        {
            bool isAccountDisabled = profile.UserProfile2.UserStatusId == Convert.ToByte(UserStatusOptions.Disabled);

            var roles = profile.UserRoles.Select(x => x.RoleName); //Roles.GetRolesForUser(model.UserName);
            if (roles.Contains(SecurityUnified.Constants.SUPER_ADMIN))
            {
                RoleConfig.InitSuperAdmin(model.UserName);
            }
            else if (roles.Contains(SecurityUnified.Constants.ADMIN)) // || roles.Contains(Constants.MARKETING)
            {
                if (isAccountDisabled)
                {
                    HttpContext.Current.Session.Clear();
                    WebSecurity.Logout();
                    throw new UnauthorizedException(model.RetryCount > SecurityUnified.Constants.LOGIN_RETRY_COUNT, Errors.Account_is_disabled);
                }
            }
            else
            {   //Accounts for average mortals
                if (isAccountDisabled)
                {
                    HttpContext.Current.Session.Clear();
                    WebSecurity.Logout();
                    throw new UnauthorizedException(model.RetryCount > SecurityUnified.Constants.LOGIN_RETRY_COUNT, Errors.Account_is_disabled);
                }

                //DateTime passwordChangedDate = DateTime.MinValue;
                if (profile.WebPagesMembership != null)
                {
                    if (profile != null
                        && profile.WebPagesMembership.PasswordChangedBy != profile.UserProfile2.UserId)
                    {
                        if (profile.WebPagesMembership.PasswordVerificationTokenExpirationDate.HasValue)
                        {
                            var tokenValidTill = profile.WebPagesMembership.PasswordVerificationTokenExpirationDate.Value;
                            if (tokenValidTill < DateTime.UtcNow)
                            {
                                HttpContext.Current.Session.Clear();
                                WebSecurity.Logout();
                                throw new UnauthorizedException(model.RetryCount > SecurityUnified.Constants.LOGIN_RETRY_COUNT, Errors.Temporary_password_expired);
                            }
                        }
                        else
                        {
                            HttpContext.Current.Session.Clear();
                            WebSecurity.Logout();
                            throw new UnauthorizedException(model.RetryCount > SecurityUnified.Constants.LOGIN_RETRY_COUNT, Errors.Temporary_password_expired);
                        }
                    }
                }
                else
                    throw new UnauthorizedException(model.RetryCount > SecurityUnified.Constants.LOGIN_RETRY_COUNT, Errors.Membership_Record_Corrupt);
            }

            if (profile.UserProfile2.UserStatusId != Convert.ToByte(UserStatusOptions.Disabled)
                && profile.UserProfile2.UserStatusId != Convert.ToByte(UserStatusOptions.Active))
            {
                profile.UserProfile2.UserStatusId = Convert.ToByte(UserStatusOptions.Active);
                Db.UpdateUserProfile(new UserProfile2Contract[] { profile }, profile.UserProfile2.UserId, false);
            }

            HttpContext.Current.Session["RetryCount"] = model.RetryCount = 0;
            var principal = new CompletePrincipal(new GenericIdentity(model.UserName), profile.UserProfile2);
            SetPrincipal(principal);

            //Set current location for owners
            int userId = profile.UserProfile2.UserId;
            var profileFilter = new SecurityUnified.Contracts.UserProfileDiscriminator() { Filter = x => x.UserId == userId };
            var ownerVsLocations = Db.FindOwnerVsLocation(null, null, profileFilter);
            if (ownerVsLocations.Any())
            {
                if (ownerVsLocations.Count() == 1)
                {
                    var ownerVsLoc = ownerVsLocations.First();
                    if (!ownerVsLoc.OwnerVsLocation.IsCurrent)
                    {
                        ownerVsLoc.OwnerVsLocation.IsCurrent = true;
                        Db.UpdateOwnerVsLocation(new OwnerVsLocationContract[] { ownerVsLoc });
                    }

                    var locationId = ownerVsLoc.OwnerVsLocation.LocationId;
                    var curLoc = Db.FindLocation(new LocationDiscriminator() { Filter = x => x.LocationId == locationId }, null, null).FirstOrDefault();
                    if (curLoc == null)
                        SessionGlobal.CurrentLocation = null;
                    else
                    {
                        SessionGlobal.CurrentLocation = new LocationModel();
                        curLoc.Location.CopyTo(SessionGlobal.CurrentLocation);
                    }
                }
                //NOTE: if owner has multiple locations we will ask to confirm location on the next screen
            }
        }

        public bool Logout()
        {
            WebSecurity.Logout();
            var principle = new GenericPrincipal(new GenericIdentity(string.Empty), null);
            SetPrincipal(principle);

            HttpContext.Current.Session.Clear();
            //HttpContext.Current.Response.Cookies.Clear();

            //SessionGlobal.CurrentDevice = null;

            return true;
        }

        #endregion

        #region Helpers

        private void SetPrincipal(IPrincipal principal)
        {
            System.Threading.Thread.CurrentPrincipal = principal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
            HttpContext.Current.Session["CurrentPrincipal"] = principal;
        }

        public string GenerateCaptcha()
        {
            HttpContext.Current.Session["CaptchaImageText"] = GenerateRandomCode();
            return GetCaptchaBase64ImageStr(HttpContext.Current.Session["CaptchaImageText"].ToString());
        }

        internal string GenerateRandomCode()
        {
            Random r = new Random();
            string s = "";
            for (int j = 0; j < 5; j++)
            {
                int i = r.Next(3);
                int ch;
                switch (i)
                {
                    case 1:
                        ch = r.Next(0, 9);
                        s = s + ch.ToString();
                        break;
                    case 2:
                        ch = r.Next(65, 90);
                        s = s + Convert.ToChar(ch).ToString();
                        break;
                    case 3:
                        ch = r.Next(97, 122);
                        s = s + Convert.ToChar(ch).ToString();
                        break;
                    default:
                        ch = r.Next(97, 122);
                        s = s + Convert.ToChar(ch).ToString();
                        break;
                }
                r.NextDouble();
                r.Next(100, 1999);
            }
            return s;
        }

        internal string GetCaptchaBase64ImageStr(string txt)
        {
            // Create a CAPTCHA image using the text stored in the Session object.
            RandomImage ci = new RandomImage(txt, 300, 75);
            using (MemoryStream stream = new MemoryStream())
            {
                ci.Image.Save(stream, ImageFormat.Jpeg);
                stream.Position = 0;
                BinaryReader br = new BinaryReader(stream);
                byte[] bytes = br.ReadBytes((Int32)stream.Length);
                return string.Concat("data:image/jpeg;base64,", Convert.ToBase64String(bytes, 0, bytes.Length));
            }
        }

        #endregion

        #region Unauthorized Exception

        [DataContract]
        public class UnauthorizedException : Xxception
        {
            [DataMember]
            public bool GenerateCaptcha { get; private set; }

            public UnauthorizedException() 
                : base()
            {
            }
            
            public UnauthorizedException(bool generateCaptcha = false, string message = null)
                : base(message)
            {
                this.GenerateCaptcha = generateCaptcha;
            }
        }

        #endregion
    }
}
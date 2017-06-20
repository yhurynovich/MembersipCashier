using SecurityDL.DB.Repository;
using SecurityUnified;
using SecurityUnified.Contracts;
using SecurityUnified.Enumerations;
using SecurityUnified.Exceptions;
using SecurityUnified.Interfaces;
using System;
using System.Data.Linq;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.SessionState;
using System.Xml.Linq;

namespace SecurityDL.Access
{
    //TODO: Xxception handling
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)] 
    public class LowLevelAccess : ILowLevelAccess
    {
        private DB.SecurityEntitiesDataContext mdb;
        //private object _sync_lock = new object();
        HttpContext context = null;

        public virtual HttpContext Context
        {
            get { return context; }
        }

        public HttpSessionState Session
        {
            get
            {
                if (Context == null)
                    return null;
                return Context.Session;
            }
        }

        private void RegisterInSession<T>(T obj)
        {
            if (Session == null)
                return;

            Session[typeof(T).Name] = obj;
        }

        private T GetFromSession<T>()
        {
            if (Session == null)
                return default(T);

            return (T)Session[typeof(T).Name];
        }

        /// <summary>
        /// Please avoid using this property in favour of RepositoryEntitySet
        /// RepositoryEntitySet is lightweight ObjectRelationProvider for a single table
        /// </summary>
        DB.SecurityEntitiesDataContext MDB
        {
            get
            {
                if (mdb == null)
                {
                    lock(this)
                    {
                        if (mdb == null)
                        {
                            mdb = GetFromSession<DB.SecurityEntitiesDataContext>();
                            if (mdb == null)
                            {
                                mdb = new DB.SecurityEntitiesDataContext();
                                RegisterInSession(mdb);
                            }
                        }
                    }
                }

                return mdb;
            }
        }

        public RoleContract[] GetUserRoles(IHasUserId user)
        {
            try
            {
                using (var records = RepositoryFactory.GetUserProfile())
                {
                    records.Discriminator = new UserProfileDiscriminator() { Filter = p => p.UserId == user.UserId };
                    var profile = records.FirstOrDefault() as DB.UserProfile;
                    if (profile == null) throw new Xxception("User was not found by id") { Tag = new XDocument(new XElement("UserId", user.UserId)) };
                    var ret = profile.webpages_UsersInRoles;
                    return ret.Select(r => new RoleContract() { Role = new RoleImplementor() { RoleId = r.RoleId, RoleName = r.webpages_Role.RoleName } }).ToArray();
                }
            }
            catch (Exception ex)
            {
                HandleMyException(ex);
                return null;
            }
        }

        public void AddRoleToUser(IHasUserId user, RoleContract[] roles, int modifiedByUserId)
        {
            try
            {
                lock(MDB)
                {
                    var profile = MDB.UserProfiles.FirstOrDefault(p => p.UserId == user.UserId);
                    if (profile == null) throw new Xxception("User was not found by id") { Tag = new XDocument(new XElement("UserId", user.UserId)) }; 
                    foreach (var newRole in roles)
                    {
                        if (!profile.webpages_UsersInRoles.Any(r => r.RoleId == newRole.Role.RoleId || r.webpages_Role.RoleName == newRole.Role.RoleName))
                        {
                            var dbRole = MDB.webpages_Roles.FirstOrDefault(r => r.RoleId == newRole.Role.RoleId || r.RoleName == newRole.Role.RoleName);
                            if (dbRole == null)
                            {
                                dbRole = new DB.webpages_Role() { RoleId = newRole.Role.RoleId, RoleName = newRole.Role.RoleName };
                                MDB.webpages_Roles.InsertOnSubmit(dbRole);
                            }

                            var dbUserInRole = new DB.webpages_UsersInRole() { RoleId = newRole.Role.RoleId, webpages_Role = dbRole };
                            profile.webpages_UsersInRoles.Add(dbUserInRole);

                            MDB.webpages_UsersInRolesAudits.InsertOnSubmit(new DB.webpages_UsersInRolesAudit()
                            { 
                                Action = Convert.ToByte(AddRemoveOptions.Add),
                                ModificationTime = DateTime.UtcNow,
                                ModifiedBy = modifiedByUserId,
                                RoleId = newRole.Role.RoleId,
                                UserId = user.UserId
                            });
                        }
                    }
                    MDB.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                HandleMyException(ex);
            }
        }

        public UserProfileContract[] GetUsersInRole(RoleDiscriminator d)
        {
            try
            {
                using (var repository = RepositoryFactory.GetUserProfile())
                {
                    repository.RoleFilter = d;
                    return repository.Select(uir => new UserProfileContract() { UserProfile = uir }).ToArray();
                }
            }
            catch (Exception ex)
            {
                HandleMyException(ex);
                return null;
            }
        }


        public RoleContract[] GetRolesForUser(UserProfileDiscriminator d)
        {
            try
            {
                using (var repository = RepositoryFactory.GetUserProfile())
                {
                    repository.Discriminator = d;

                    var users_in_roles = repository.OfType<DB.UserProfile>().SelectMany(p => p.webpages_UsersInRoles);

                    return users_in_roles.Select(uir => uir.webpages_Role).Distinct().Select(r => new RoleContract() { Role = r }).ToArray();
                }
            }
            catch (Exception ex)
            {
                HandleMyException(ex);
                return null;
            }
        }

        #region Find

        public UserProfileContract[] FindUserProfile(UserProfileDiscriminator d)
        {
            try
            {
                using (var records = RepositoryFactory.GetUserProfile())
                {
                    records.Discriminator = d;
                    return records.Select(r => new UserProfileContract() { UserProfile = r }).ToArray();
                }
            }
            catch (Exception ex)
            {
                HandleMyException(ex);
                return null;
            }
        }

        public ExtendedUserProfileContract[] FindExtendedUserProfile(UserProfileDiscriminator d)
        {
            try
            {
                using (var records = RepositoryFactory.GetUserProfile())
                {
                    records.Discriminator = d;
                    return records.OfType<DB.UserProfile>().Select(r => new ExtendedUserProfileContract() { UserProfile = r, WebPagesMembership = r.webpages_Membership, UserRoles = r.webpages_UsersInRoles.Select(x => x.webpages_Role) }).ToArray();
                }
            }
            catch (Exception ex)
            {
                HandleMyException(ex);
                return null;
            }
        }

        public RoleContract[] FindRole(RoleDiscriminator d)
        {
            try
            {
                using (var records = RepositoryFactory.GetRole())
                {
                    records.Discriminator = d;
                    return records.Select(r => new RoleContract() { Role = r }).ToArray();
                }
            }
            catch (Exception ex)
            {
                HandleMyException(ex);
                return null;
            }
        }

        public WebPagesMembershipContract[] FindWebPagesMembership(WebPagesMembershipDiscriminator d)
        {
            try
            {
                using (var records = RepositoryFactory.GetWebPagesMembership())
                {
                    records.Discriminator = d;
                    return records.Select(r => new WebPagesMembershipContract() { WebPagesMembership = r }).ToArray();
                }
            }
            catch (Exception ex)
            {
                HandleMyException(ex);
                return null;
            }
        }
        #endregion

        #region Update

        public void UpdateUserProfile(UserProfileContract[] d, int modifiedByUserId, bool allowDefaultValues = true)
        {
            try
            {
                lock(MDB)
                {
                    foreach (var profile in d)
                    {
                       var dbProfile = MDB.UserProfiles.First(p => p.UserId == profile.UserProfile.UserId);

                        //insert audit
                        var audit = new DB.UserProfileAudit()
                        {
                            Action = Convert.ToByte(AddRemoveOptions.PreviousVersion),
                            ModificationTime = DateTime.UtcNow,
                            ModifiedBy = modifiedByUserId
                        };
                        (dbProfile as IUserProfile).CopyTo(audit); //store previous profile data

                        MDB.UserProfileAudits.InsertOnSubmit(audit);

                        //update profile
                        profile.UserProfile.CopyTo(dbProfile, allowDefaultValues);
                    }

                    MDB.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                HandleMyException(ex);
            }
        }

        public void UpdateWebPagesMembership(WebPagesMembershipContract[] d)
        {
            lock (MDB)
            {
                try
                {
                    foreach (var membership in d)
                    {
                        var dbMembership = MDB.webpages_Memberships.First(p => p.UserId == membership.WebPagesMembership.UserId);
                        membership.WebPagesMembership.CopyTo(dbMembership, false);
                    }

                    MDB.SubmitChanges();
                }
                catch (Exception ex)
                {
                    HandleMyException(ex);
                }
            }
        }

        #endregion

        #region Insert

        public void InsertUserProfile(ExtendedUserProfileContract[] d)
        {
            try
            {
                lock(MDB)
                {
                    foreach (var x in d)
                    {
                        var pp = new DB.UserProfile();
                        x.UserProfile.CopyTo(pp);
                        MDB.UserProfiles.InsertOnSubmit(pp);
                        var mm = new DB.webpages_Membership();
                        x.WebPagesMembership.CopyTo(mm);
                        pp.webpages_Membership = mm;
                        MDB.webpages_Memberships.InsertOnSubmit(mm);
                    }
                    MDB.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                HandleMyException(ex);
            }
        }

        public void InsertWebpagesUsersInRole(WebpagesUsersInRolesContract[] d)
        {
            lock (MDB)
            {
                try
                {
                    foreach (var x in d)
                    {
                        var pp = new DB.webpages_UsersInRole();
                        x.WebpagesUsersInRoles.CopyTo(pp);
                        MDB.webpages_UsersInRoles.InsertOnSubmit(pp);
                    }
                    MDB.SubmitChanges();
                }
                catch (Exception ex)
                {
                    HandleMyException(ex);
                }
            }
        }

        #endregion

        public void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            lock (MDB)
            {
                try
                {
                    foreach (var u in usernames)
                    {
                        var user = MDB.UserProfiles.Single(x => x.UserName == u);
                        foreach (var r in roleNames)
                        {
                            var role = MDB.webpages_Roles.Single(x => x.RoleName == r);
                            var pp = new DB.webpages_UsersInRole()
                            {
                                UserProfile = user,
                                webpages_Role = role
                            };
                            MDB.webpages_UsersInRoles.InsertOnSubmit(pp);
                        }
                    }
                    MDB.SubmitChanges();
                }
                catch (Exception ex)
                {
                    HandleMyException(ex);
                }
            }
        }

        private void HandleMyException(Exception ex)
        {
            lock (MDB)
            {
                if (typeof(ChangeConflictException).IsInstanceOfType(ex)
                    //|| (typeof(AggregateException).IsInstanceOfType(ex) && ((AggregateException)ex).InnerException != null) && typeof(ChangeConflictException).IsInstanceOfType(((AggregateException)ex).InnerException))
                )
                {
                    try
                    {
                        System.Threading.Thread.Sleep(14);
                        MDB.Refresh(RefreshMode.KeepChanges, MDB.GetChangeSet().Updates);
                        MDB.SubmitChanges();
                        return; // problem solved
                    }
                    catch
                    {
                        //go on and log original exception
                    }
                }

                try
                {
                    EventLog.WriteEntry("Application"
                        , ex.ConcatenateTrace()
                        , EventLogEntryType.Error);
                }
                catch { }

#if !DEBUG
            throw ex;
#endif
            }
        }

        public LowLevelAccess(HttpContext context = null)
        {
            if (context == null)
                this.context = HttpContext.Current;
        }

        public LowLevelAccess()
        {
        }
    }
}

using MembershipCashierW.Code;
using SecurityDL.Access;
using SecurityUnified.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Xml.Serialization;

namespace MembershipCashierW.Models
{
    [Serializable]
    public class CompletePrincipal : IPrincipal, IDisposable
    {
        private CompleteIdentity identity;
        private IEnumerable<IRole> userRoles;

        [XmlIgnore]
        public CompleteIdentity Identity
        {
            get { return identity; }
        }

        [XmlIgnore]
        IIdentity IPrincipal.Identity
        {
            get { return identity; }
        }

        [XmlIgnore]
        [Display(Name = "Access Type")]
        public IEnumerable<string> RoleNames { 
            get {
                if (userRoles == null)
                    userRoles = ServiceAccessor.SecurityDb.GetUserRoles(identity).Select(r => r.Role).ToArray();

                return userRoles.Select(r => r.RoleName);
            }
            set { throw new NotImplementedException(); } // For security reasons
        }

        public bool IsInRole(string role)
        {
            return RoleNames != null && RoleNames.Any(r => r == role);
        }

        public CompletePrincipal(IIdentity sourceIdentity, IHasUserId sourceUser)
        {
            if (typeof(CompleteIdentity).IsInstanceOfType(sourceIdentity))
                identity = sourceIdentity as CompleteIdentity;
            else
                identity = new CompleteIdentity(sourceIdentity, sourceUser);
        }

        public void Dispose()
        {
        }
    }
}
using SecurityUnified.Interfaces;
using System;
using System.Security.Principal;
using System.Xml.Serialization;

namespace MembershipCashierW.Models
{
    [Serializable]
    public sealed class CompleteIdentity : IIdentity, IHasUserId
    {
        IIdentity identity;
        IHasUserId user;

        [XmlIgnore]
        public string AuthenticationType
        {
            get { return identity.AuthenticationType ; }
        }
        [XmlIgnore]
        public bool IsAuthenticated
        {
            get { return identity.IsAuthenticated ; }
        }
        [XmlIgnore]
        public string Name
        {
            get { return identity.Name; }
        }
        [XmlIgnore]
        public int UserId
        {
            get
            {
                return user.UserId;
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }
        [XmlIgnore]
        public IUserProfile UserProfile
        {
            get
            {
                if (typeof(IUserProfile).IsInstanceOfType(user))
                    return (IUserProfile) user;
                return null;
            }
        }

        public CompleteIdentity(IIdentity sourceIdentity, IHasUserId sourceUser)
        {
            this.identity = sourceIdentity;
            this.user = sourceUser;
        }
    }
}
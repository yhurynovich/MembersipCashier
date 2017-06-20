using MembershipCashierUnified.Interfaces;
using SecurityUnified.Interfaces;
using System.Security.Principal;

namespace MembershipCashierW.Models
{
    public class OwnerPrincipal : CompletePrincipal, IHasOwner
    {
        private IOwner ownerProfile;

        /// <summary>
        /// Principle as Owner
        /// </summary>
        public IOwner Owner
        {
            get
            {
                if(ownerProfile == null)
                {
                    ownerProfile = this.Identity.UserProfile.GetRelatedOwner();
                }

                return ownerProfile;
            }
            set
            {
                ownerProfile = value;
            }
        }

        public OwnerPrincipal(IIdentity sourceIdentity, IHasUserId sourceUser)
            : base(sourceIdentity, sourceUser)
        { }
    }
}
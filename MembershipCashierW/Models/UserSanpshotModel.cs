using MembershipCashierUnified.Contracts;
using MembershipCashierUnified.Interfaces;
using SecurityUnified.Contracts;
using SecurityUnified.Interfaces;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MembershipCashierW.Models
{
    [KnownType(typeof(ProfileCreditContract))]
    [KnownType(typeof(ProfileCreditImplementor))]
    [KnownType(typeof(UserProfileImplementor))]
    [KnownType(typeof(UserProfileContract))]
    [KnownType(typeof(CreditTransactionImplementor))]
    [KnownType(typeof(CreditTransactionContract))]
    public class UserSanpshotModel : IHasUserProfile2, IHasProfileCredits, IHasCreditTransactions
    {
        public IUserProfile2 UserProfile2 { get; set; }

        public IEnumerable<IProfileCredit> ProfileCredits { get; set; }

        public IEnumerable<ICreditTransaction> CreditTransactions { get; set; }
    }
}
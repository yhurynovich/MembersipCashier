using System.Collections.Generic;

namespace MembershipCashierUnified.Interfaces
{
    public interface IHasProfileCredits
    {
        IEnumerable<IProfileCredit> ProfileCredits { get; set; }
    }
}

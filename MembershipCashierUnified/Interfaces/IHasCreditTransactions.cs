using System.Collections.Generic;

namespace MembershipCashierUnified.Interfaces
{
    public interface IHasCreditTransactions
    {
        IEnumerable<ICreditTransaction> CreditTransactions { get; set; }
    }
}

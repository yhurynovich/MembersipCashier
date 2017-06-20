using SecurityUnified.Interfaces;

namespace MembershipCashierUnified.Interfaces
{
    public interface ICreditTransaction : IHasCreditTransactionId, IHasProductId, IHasLocationId, IHasUserId, IHasDescription, IHasBallanceUnits
    {
        System.DateTime TransactionTime { get; set; }
    }
}

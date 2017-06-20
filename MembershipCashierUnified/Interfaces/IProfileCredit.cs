using SecurityUnified.Interfaces;
using System;

namespace MembershipCashierUnified.Interfaces
{
    public interface IProfileCredit : IHasProductId, IHasUserId, IHasLocationId, IHasBallance, IHasBallanceUnits
    {
        DateTime CalculatedTime { get; set; }
        bool? HasBallance { get; }
    }
}

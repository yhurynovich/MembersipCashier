using System;

namespace MembershipCashierUnified.Interfaces
{
    public interface IPayment : IHasPaymentId
    {
        Int16 Sequence { get; set; }
        char PaymentMethod { get; set; }
        decimal? Amount { get; set; }
        string Currency { get; set; }
    }
}

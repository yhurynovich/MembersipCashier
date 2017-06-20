using System;

namespace MembershipCashierUnified.Interfaces
{
    public interface IAddress : IHasAddressId, IIAddress
    {
        string ProvinceName { get; set; }
        bool? IsResidential { get; set; }
        byte ValidityLevel { get; set; }
        DateTime? ValidatedTime { get; set; }
    }
}

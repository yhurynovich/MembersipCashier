using System.Collections.Generic;

namespace MembershipCashierUnified.Interfaces
{
    public interface IHasProducts
    {
        IEnumerable<IProduct> Products { get; set; }
    }
}

using MembershipCashierUnified.Interfaces;
using SecurityUnified.Contracts;
using System.Runtime.Serialization;

namespace MembershipCashierUnified.Contracts
{
    [KnownType(typeof(AddressContract))]
    [KnownType(typeof(AddressImplementor))]
    public class AddressDiscriminator : DataDiscriminator<IAddress>
    {
    }
}

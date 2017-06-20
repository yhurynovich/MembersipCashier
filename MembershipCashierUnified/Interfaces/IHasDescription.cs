using System.Runtime.Serialization;

namespace MembershipCashierUnified.Interfaces
{
    public interface IHasDescription
    {
        [DataMember]
        string Description { get; set; }
    }
}

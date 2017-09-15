using MembershipCashierUnified.Contracts;
using MembershipCashierUnified.Interfaces;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MembershipCashierDL.MixedContracts
{
    [KnownType(typeof(ProductImplementor))]
    [KnownType(typeof(ProfileCreditImplementor))]
    [KnownType(typeof(PaymentImplementor))]
    [DataContract]
    public class UserAndProductsContarct : SecurityUnified.Contracts.UserProfileContract
    {
        [DataMember]
        public IEnumerable<ProductCreditContract> Products { get; set; }
        [DataMember]
        public IPayment LastPayment { get; set; }
    }
}

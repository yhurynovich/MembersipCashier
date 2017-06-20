using MembershipCashierUnified.Contracts;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MembershipCashierDL.MixedContracts
{
    [KnownType(typeof(ProductImplementor))]
    [KnownType(typeof(ProfileCreditImplementor))]
    [DataContract]
    public class UserAndProductsContarct : SecurityUnified.Contracts.UserProfileContract
    {
        [DataMember]
        public IEnumerable<ProductCreditContract> Products { get; set; }
    }
}

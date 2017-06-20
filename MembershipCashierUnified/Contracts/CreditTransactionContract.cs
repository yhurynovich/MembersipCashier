using MembershipCashierUnified.Interfaces;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace MembershipCashierUnified.Contracts
{
    [DataContract]
    public class CreditTransactionContract : IHasCreditTransaction
    {
        [DataMember(Name = "CreditTransaction")]
        private CreditTransactionImplementor _creditTransaction;

        [JsonIgnore]
        public ICreditTransaction CreditTransaction
        {
            get { return (_creditTransaction as ICreditTransaction); }
            set { var x = new CreditTransactionImplementor(); value.CopyTo(x); _creditTransaction = x; }
        }
    }
}

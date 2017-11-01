using MembershipCashierUnified;
using MembershipCashierUnified.Contracts;
using MembershipCashierUnified.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;

namespace MembershipCashierDL.MixedContracts
{
    [DataContract]
    public class ProductAndTransactionsContract : ProductContract, IHasProduct
    {
        [DataMember(Name = "CreditTransactions")]
        private CreditTransactionImplementor[] _creditTransactions;

        [JsonIgnore]
        public IEnumerable<ICreditTransaction> CreditTransactions
        {
            get { return _creditTransactions.OfType<ICreditTransaction>(); }
            set {
                if (value == null || !value.Any())
                {
                    _creditTransactions = null;
                    return;
                }

                var cnt = value.Count();
                var x = new CreditTransactionImplementor[cnt];
                int index = 0;
                foreach(var trn in value)
                {
                    var res = new CreditTransactionImplementor();
                    trn.CopyTo(res);
                    x[index] = res;
                    index++;
                }

                _creditTransactions = x;
            }
        }
    }
}


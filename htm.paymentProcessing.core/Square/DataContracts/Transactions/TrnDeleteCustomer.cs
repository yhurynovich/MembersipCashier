using htm.paymentProcessing.core.Square.Interfaces;
using System.Runtime.Serialization;
using System;
using Newtonsoft.Json;

namespace htm.paymentProcessing.core.Square.DataContracts.Transactions
{
    [DataContract]
    public class TrnDeleteCustomer : TransactionBase, ITrnDeleteCustomer
    {

        [DataMember]
        public override string AccessToken { get; set; }
        [DataMember]
        public string CustomerId { get; set; }
    }
}

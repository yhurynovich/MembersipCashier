using htm.paymentProcessing.core.Square.Interfaces;
using System.Runtime.Serialization;
using htm.paymentProcessing.core.Interfaces;
using System;
using Newtonsoft.Json;

namespace htm.paymentProcessing.core.Square.DataContracts.Transactions
{
    [DataContract]
    public class TrnCreateCustomer : TransactionBase, ITrnCreateCustomer
    {
        [DataMember(Name = "TransactionId")]
        string transactionId;

        [DataMember]
        public override string AccessToken { get; set; }

        [DataMember]
        public IAddress Address { get; set; }

        [DataMember]
        public string CompanyName { get; set; }

        [DataMember]
        public IParty Party { get; set; }
    }
}

using htm.paymentProcessing.core.Square.Interfaces;
using System.Runtime.Serialization;
using System;
using Newtonsoft.Json;
using htm.paymentProcessing.core.Interfaces;

namespace htm.paymentProcessing.core.Square.DataContracts.Transactions
{
    [DataContract]
    public class TrnCreateCustomerCard : TrnDeleteCustomerCard, ITrnCreateCustomerCard
    {
        [DataMember]
        public IAddress Address { get; set; }
        [DataMember]
        public string CardholderName { get; set; }
    }
}

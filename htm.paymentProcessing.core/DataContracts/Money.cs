using htm.paymentProcessing.core.Enumerations;
using htm.paymentProcessing.core.Interfaces;
using System.Runtime.Serialization;

namespace htm.paymentProcessing.core.DataContracts
{
    [DataContract]
    public class Money : IMoney
    {
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public CurrencyOptions Currency { get; set; }
    }
}

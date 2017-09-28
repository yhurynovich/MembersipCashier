using htm.paymentProcessing.core.Square.Interfaces;
using System.Runtime.Serialization;
using htm.paymentProcessing.core.Interfaces;
using System;
using Newtonsoft.Json;

namespace htm.paymentProcessing.core.Square.DataContracts.Transactions
{
    [DataContract]
    public class TrnCharge : TransactionBase, ITrnCharge
    {
        [DataMember(Name = "TransactionId")]
        string transactionId;

        [DataMember]
        public override string AccessToken { get; set; }

        [DataMember]
        public string CardNonce { get; set; }

        [DataMember]
        public string LocationId { get; set; }

        [DataMember]
        public IMoney Money { get; set; }

        [JsonIgnore]
        public string TransactionId
        {
            get
            {
                if (string.IsNullOrEmpty(transactionId))
                    transactionId = NewIdempotencyKey();
                return transactionId;
            }
            set { transactionId = value; }
        }

        private string NewIdempotencyKey()
        {
            return Guid.NewGuid().ToString();
        }
    }
}

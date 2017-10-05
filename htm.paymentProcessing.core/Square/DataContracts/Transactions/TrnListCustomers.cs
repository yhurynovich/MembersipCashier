using htm.paymentProcessing.core.Square.Interfaces;
using System.Runtime.Serialization;
using System;
using Newtonsoft.Json;

namespace htm.paymentProcessing.core.Square.DataContracts.Transactions
{
    [DataContract]
    public class TrnListCustomers : TransactionBase, ITrnListCustomers
    {
        [DataMember(Name = "PaginationCursor")]
        private string paginationCursor;

        [DataMember]
        public override string AccessToken { get; set; }
        [JsonIgnore]
        public string PaginationCursor
        {
            get { return paginationCursor; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    paginationCursor = null;
                else
                    paginationCursor = value.Trim();
            }
        }
    }
}

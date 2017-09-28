using htm.paymentProcessing.core.Square.Interfaces;
using System.Runtime.Serialization;

namespace htm.paymentProcessing.core.Square.DataContracts.Transactions
{
    [DataContract]
    public class TrnGetLocations : TransactionBase, ITrnGetLocations
    {
        [DataMember]
        public override string AccessToken { get; set; }
    }
}

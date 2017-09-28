using htm.paymentProcessing.core.Square.Interfaces;

namespace htm.paymentProcessing.core.Square.DataContracts.Transactions
{
    public abstract class TransactionBase : IHasAccessToken
    {
        public abstract string AccessToken { get; set; }
    }
}

using Square.Connect.Api;

namespace htm.paymentProcessing.core.Square.ServiceCalls.Base
{
    internal abstract class TransactionCallerbase<ResponseType> : CallerBase<ResponseType>
    {
        protected TransactionsApi transactionsApi;

        internal TransactionCallerbase(TransactionsApi transactionsApi)
        {
            this.transactionsApi = transactionsApi;
        }
    }
}

using htm.paymentProcessing.core.Square.Interfaces;
using Square.Connect.Api;

namespace htm.paymentProcessing.core.Square.ServiceCalls.Base
{
    internal abstract class TransactionCallerbase<ResponseType> : CallerBase<ResponseType>
    {
        protected TransactionsApi transactionsApi;

        public override ResponseType Execute<T>(T transaction)
        {
            var data = (IHasAccessToken)transaction;
            transactionsApi.AddDefaultHeader("Authorization", $"Bearer {data.AccessToken}");
            return default(ResponseType);
        }

        internal TransactionCallerbase(TransactionsApi transactionsApi)
        {
            this.transactionsApi = transactionsApi;
        }
    }
}

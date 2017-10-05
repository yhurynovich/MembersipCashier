using htm.paymentProcessing.core.Square.Interfaces;
using Square.Connect.Api;

namespace htm.paymentProcessing.core.Square.ServiceCalls.Base
{
    internal abstract class CustomerCallerBase<ResponseType> : CallerBase<ResponseType>
    {
        protected CustomersApi customersApi;

        public override ResponseType Execute<T>(T transaction)
        {
            var data = (IHasAccessToken)transaction;
            customersApi.AddDefaultHeader("Authorization", $"Bearer {data.AccessToken}");
            return default(ResponseType);
        }

        internal CustomerCallerBase(CustomersApi customersApi)
        {
            this.customersApi = customersApi;
        }
    }
}

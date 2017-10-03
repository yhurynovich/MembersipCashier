using htm.paymentProcessing.core.Square.Interfaces;
using Square.Connect.Api;

namespace htm.paymentProcessing.core.Square.ServiceCalls.Base
{
    internal abstract class LocationCallerBase<ResponseType> : CallerBase<ResponseType>
    {
        protected LocationsApi locationsApi;

        public override ResponseType Execute<T>(T transaction)
        {
            var data = (IHasAccessToken)transaction;
            locationsApi.AddDefaultHeader("Authorization", $"Bearer {data.AccessToken}");
            return default(ResponseType);
        }

        internal LocationCallerBase(LocationsApi locationsApi)
        {
            this.locationsApi = locationsApi;
        }
    }
}

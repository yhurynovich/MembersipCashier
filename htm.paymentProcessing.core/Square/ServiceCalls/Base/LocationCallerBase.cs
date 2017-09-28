using Square.Connect.Api;

namespace htm.paymentProcessing.core.Square.ServiceCalls.Base
{
    internal abstract class LocationCallerBase<ResponseType> : CallerBase<ResponseType>
    {
        protected LocationsApi locationsApi;

        internal LocationCallerBase(LocationsApi locationsApi)
        {
            this.locationsApi = locationsApi;
        }
    }
}

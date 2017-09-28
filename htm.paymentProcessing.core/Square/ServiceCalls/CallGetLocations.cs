using htm.paymentProcessing.core.Square.Interfaces;
using htm.paymentProcessing.core.Square.ServiceCalls.Base;
using Square.Connect.Api;
using Square.Connect.Model;

namespace htm.paymentProcessing.core.Square.ServiceCalls
{
    internal class CallGetLocations : LocationCallerBase<ListLocationsResponse>
    {
        public override ListLocationsResponse Execute<T>(T transaction)
        {
            ITrnGetLocations data = (ITrnGetLocations)transaction;
            var response = locationsApi.ListLocations();
            return response;
        }

        public CallGetLocations(LocationsApi locationsApi) : base(locationsApi)
        {
        }
    }
}

using htm.paymentProcessing.core.Square.DataContracts.Transactions;
using htm.paymentProcessing.core.Square.Interfaces;
using Square.Connect.Api;
using Square.Connect.Model;
using System;

namespace htm.paymentProcessing.core.Square.ServiceCalls
{
    internal class ServiceCallFactory
    {
        static TransactionsApi transactionsApi = new TransactionsApi();
        static LocationsApi locationsApi = new LocationsApi();

        public ListLocationsResponse ListLocations(ITrnGetLocations data)
        {
            return new CallGetLocations(locationsApi).Execute(data);
        }

        public ChargeResponse Charge(ITrnCharge data)
        {
            return new CallCharge(transactionsApi).Execute(data);
        }
    }
}

using htm.paymentProcessing.core.Square.DataContracts.Transactions;
using htm.paymentProcessing.core.Square.Interfaces;
using Square.Connect.Api;
using Square.Connect.Model;
using System;

namespace htm.paymentProcessing.core.Square.ServiceCalls
{
#if DEBUG
    public class ServiceCallFactory
#else
    internal class ServiceCallFactory
#endif
    {
        static TransactionsApi transactionsApi = new TransactionsApi();
        static LocationsApi locationsApi = new LocationsApi();
        static CustomersApi customersApi = new CustomersApi();

        public ListLocationsResponse ListLocations(ITrnGetLocations data)
        {
            return new CallGetLocations(locationsApi).Execute(data);
        }

        public ChargeResponse Charge(ITrnCharge data)
        {
            return new CallCharge(transactionsApi).Execute(data);
        }

        public ListCustomersResponse ListCustomers(ITrnListCustomers data)
        {
            return new CallListCustomers(customersApi).Execute(data);
        }

        public CreateCustomerResponse CreateCustomer(ITrnCreateCustomer data)
        {
            return new CallCreateCustomer(customersApi).Execute(data);
        }

        public DeleteCustomerResponse DeleteCustomer(ITrnDeleteCustomer data)
        {
            return new CallDeleteCustomer(customersApi).Execute(data);
        }

        public DeleteCustomerResponse RetrieveCustomer(ITrnRetrieveCustomer data)
        {
            return new CallDeleteCustomer(customersApi).Execute(data);
        }
    }
}

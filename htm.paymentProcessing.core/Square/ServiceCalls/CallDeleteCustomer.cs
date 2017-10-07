using htm.paymentProcessing.core.Square.Interfaces;
using htm.paymentProcessing.core.Square.ServiceCalls.Base;
using Square.Connect.Api;
using Square.Connect.Model;

namespace htm.paymentProcessing.core.Square.ServiceCalls
{
    internal class CallDeleteCustomer : CustomerCallerBase<DeleteCustomerResponse>
    {
        public override DeleteCustomerResponse Execute<T>(T transaction)
        {
            base.Execute(transaction); //Needed to inject Authorization headers
            ITrnDeleteCustomer data = (ITrnDeleteCustomer)transaction;
            var response = customersApi.DeleteCustomer(data.CustomerId);
            return response;
        }

        public CallDeleteCustomer(CustomersApi customersApi) : base(customersApi)
        {
        }
    }
}

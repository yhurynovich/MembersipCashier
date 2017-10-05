using htm.paymentProcessing.core.Square.Interfaces;
using htm.paymentProcessing.core.Square.ServiceCalls.Base;
using Square.Connect.Api;
using Square.Connect.Model;

namespace htm.paymentProcessing.core.Square.ServiceCalls
{
    internal class CallListCustomers : CustomerCallerBase<ListCustomersResponse>
    {
        public override ListCustomersResponse Execute<T>(T transaction)
        {
            base.Execute(transaction); //Needed to inject Authorization headers
            ITrnListCustomers data = (ITrnListCustomers)transaction;
            var response = customersApi.ListCustomers(data.PaginationCursor);
            return response;
        }

        public CallListCustomers(CustomersApi customersApi) : base(customersApi)
        {
        }
    }
}

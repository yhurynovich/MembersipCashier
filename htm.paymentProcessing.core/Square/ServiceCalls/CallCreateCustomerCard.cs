using htm.paymentProcessing.core.Square.Interfaces;
using htm.paymentProcessing.core.Square.ServiceCalls.Base;
using Square.Connect.Api;
using Square.Connect.Model;

namespace htm.paymentProcessing.core.Square.ServiceCalls
{
    internal class CallCreateCustomerCard : CustomerCallerBase<CreateCustomerCardResponse>
    {
        public override CreateCustomerCardResponse Execute<T>(T transaction)
        {
            base.Execute(transaction); //Needed to inject Authorization headers
            ITrnCreateCustomerCard data = (ITrnCreateCustomerCard)transaction;
            var request = new CreateCustomerCardRequest
            {
                BillingAddress = data.Address.ToNativeSquareAddress(),
                CardholderName = data.CardholderName,
                CardNonce = data.CardNonce
            };
            var response = customersApi.CreateCustomerCard(data.CustomerId, request);
            return response;
        }

        public CallCreateCustomerCard(CustomersApi customersApi) : base(customersApi)
        {
        }
    }
}

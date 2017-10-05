﻿using htm.paymentProcessing.core.Square.Interfaces;
using htm.paymentProcessing.core.Square.ServiceCalls.Base;
using Square.Connect.Api;
using Square.Connect.Model;

namespace htm.paymentProcessing.core.Square.ServiceCalls
{
    internal class CallCreateCustomer : CustomerCallerBase<CreateCustomerResponse>
    {
        public override CreateCustomerResponse Execute<T>(T transaction)
        {
            base.Execute(transaction); //Needed to inject Authorization headers
            ITrnCreateCustomer data = (ITrnCreateCustomer)transaction;
            var body = new CreateCustomerRequest
            {
                 //Address = data.Address.ToSquareAddress()
            };
            var response = customersApi.CreateCustomer(body);
            return response;
        }

        public CallCreateCustomer(CustomersApi customersApi) : base(customersApi)
        {
        }
    }
}

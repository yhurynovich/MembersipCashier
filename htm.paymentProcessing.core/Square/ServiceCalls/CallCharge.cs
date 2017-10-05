using htm.paymentProcessing.core.Square.Interfaces;
using htm.paymentProcessing.core.Square.ServiceCalls.Base;
using Square.Connect.Api;
using Square.Connect.Model;

namespace htm.paymentProcessing.core.Square.ServiceCalls
{
    internal class CallCharge : TransactionCallerbase<ChargeResponse>
    {
        public override ChargeResponse Execute<T>(T transaction)
        {
            base.Execute(transaction); //Needed to inject Authorization headers
            ITrnCharge data = (ITrnCharge)transaction;
            ChargeRequest body = new ChargeRequest(
                AmountMoney: data.Money.ToSquareMoney(),
                IdempotencyKey: data.TransactionId,
                //ReferenceId: data.TransactionId,
                //Note: "",
                //DelayCapture: false,
                //BillingAddress: new DataContracts.SquareAddress() { Address1 = "3232 Lednier terr.", Province = "On", City = "Mississauga", PostalCode = "L4Y3Z8", Country = Address.CountryEnum.CA },
                //ShippingAddress: new DataContracts.SquareAddress() { Address1 = "3232 Lednier terr.", Province = "On", City="Mississauga", PostalCode="L4Y3Z8", Country= Address.CountryEnum.CA },
                BuyerEmailAddress: data.EmailAddress,
                CardNonce: data.CardNonce);
            var response = transactionsApi.Charge(data.LocationId, body);
            return response;
        }

        public CallCharge(TransactionsApi transactionsApi) : base(transactionsApi)
        {
        }
    }
}

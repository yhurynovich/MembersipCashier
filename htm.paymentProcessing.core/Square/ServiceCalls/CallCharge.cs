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
            ITrnCharge data = (ITrnCharge)transaction;
            ChargeRequest body = new ChargeRequest(AmountMoney: data.Money.ToSquareMoney(), IdempotencyKey: data.TransactionId, CardNonce: data.CardNonce);
            var response = transactionsApi.Charge(data.LocationId, body);
            return response;
        }

        public CallCharge(TransactionsApi transactionsApi) : base(transactionsApi)
        {
        }
    }
}

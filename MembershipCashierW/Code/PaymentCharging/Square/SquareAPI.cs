using Square.Connect.Api;
using Square.Connect.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MembershipCashierW.Code.PaymentCharging.Square
{
    public class SquareAPI
    {
        private static TransactionsApi transactionApi;

        // The access token to use in all Connect API requests. 
        // Use your *sandbox* accesstoken if you're just testing things out.
        //private string accessToken;

        // The ID of the business location to associate processed payments with.        
        // See [Retrieve your business's locations]
        // (https://docs.connect.squareup.com/articles/getting-started/#retrievemerchantprofile)
        // for an easy way to get your business's location IDs.
        // If you're testing things out, use a sandbox location ID.
        private string locationId;

        public string NewIdempotencyKey()
        {
            return Guid.NewGuid().ToString();
        }

        public string Charge(Money amount, string nonce)
        {
            // Every payment you process with the SDK must have a unique idempotency key.
            // If you're unsure whether a particular payment succeeded, you can reattempt
            // it with the same idempotency key without worrying about double charging
            // the buyer.
            string uuid = NewIdempotencyKey();

            ChargeRequest charge = new ChargeRequest(AmountMoney: amount, IdempotencyKey: uuid, CardNonce: nonce);

            return Charge(charge);
        }

        public string Charge(ChargeRequest charge)
        {
            var response = transactionApi.Charge(locationId, charge);
            return "Transaction complete\n" + response.ToJson();
        }

        public SquareAPI(string _locationId)
        {
            this.locationId = _locationId;
            //this.accessToken = _accessToken;
        }

        static SquareAPI()
        {
            transactionApi = new TransactionsApi();
        }
    }
}
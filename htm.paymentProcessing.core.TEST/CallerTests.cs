using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using htm.paymentProcessing.core.Square.ServiceCalls;
using htm.paymentProcessing.core.Square.DataContracts.Transactions;
using square = Square.Connect;

namespace htm.paymentProcessing.core.TEST
{
    [TestClass]
    public class CallerTests
    {
        [TestMethod]
        public void TestListLocations()
        {
            var factory = new ServiceCallFactory();
            var trn = new TrnGetLocations() { AccessToken = "sandbox-sq0atb-M8S-5tUs0Is0Bo3Nnf3r0A" };
            var ret = factory.ListLocations(trn);
        }

        [TestMethod]
        public void TestCharge()
        {
            var factory = new ServiceCallFactory();
            var trn = new TrnCharge() {
                AccessToken = "sandbox-sq0atb-M8S-5tUs0Is0Bo3Nnf3r0A",
                LocationId = "CBASED7U-n6EUiCOp_yGN-b1slUgAQ",
                Money = new Square.DataContracts.SquareMoney() { Amount = 200, Currency= square.Model.Money.CurrencyEnum.CAD },
                CardNonce = "fake-card-nonce-ok",
                EmailAddress = "test@lala.com"
            };
            var ret = factory.Charge(trn);
        }

        [TestMethod]
        public void TestListCustomers()
        {
            var factory = new ServiceCallFactory();
            var trn = new TrnListCustomers() { AccessToken = "sandbox-sq0atb-M8S-5tUs0Is0Bo3Nnf3r0A" };
            var ret = factory.ListCustomers(trn);
        }
    }
}

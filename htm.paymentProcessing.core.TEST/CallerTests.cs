using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using htm.paymentProcessing.core.Square.ServiceCalls;
using htm.paymentProcessing.core.Square.DataContracts.Transactions;
using square = Square.Connect;
using htm.paymentProcessing.core.DataContracts;

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

        [TestMethod]
        public void TestCreateCustomer()
        {
            var factory = new ServiceCallFactory();
            var trn = new TrnCreateCustomer()
            {
                AccessToken = "sandbox-sq0atb-M8S-5tUs0Is0Bo3Nnf3r0A",
                Address = new Square.DataContracts.SquareAddress() { Address1 = "3232 Lednier terr.", Province = "On", City = "Mississauga", PostalCode = "L4Y3Z8", Country = square.Model.Address.CountryEnum.CA },
                Party = new Party { EmailAddress = "lala@test.com", FirstName = "Joe", LastName = "Doe", PhoneNumber = "1111111111" }
            };
            var ret = factory.CreateCustomer(trn);
        }

        [TestMethod]
        public void TestDeleteCustomer()
        {
            var factory = new ServiceCallFactory();
            var trn = new TrnDeleteCustomer() {
                AccessToken = "sandbox-sq0atb-M8S-5tUs0Is0Bo3Nnf3r0A",
                CustomerId = "CBASEBAHrQzWIxqUgI4gpMm6hRQgAQ"
            };
            var ret = factory.DeleteCustomer(trn);
        }

        [TestMethod]
        public void TestRetrieveCustomer()
        {
            var factory = new ServiceCallFactory();
            var trn = new TrnRetrieveCustomer()
            {
                AccessToken = "sandbox-sq0atb-M8S-5tUs0Is0Bo3Nnf3r0A",
                CustomerId = "CBASEBAHrQzWIxqUgI4gpMm6hRQgAQ"
            };
            var ret = factory.RetrieveCustomer(trn);
        }

        [TestMethod]
        public void TestCreateCustomerCard()
        {
            var factory = new ServiceCallFactory();
            var trn = new TrnCreateCustomerCard()
            {
                AccessToken = "sandbox-sq0atb-M8S-5tUs0Is0Bo3Nnf3r0A",
                CustomerId = "CBASEBAHrQzWIxqUgI4gpMm6hRQgAQ",
                Address = new Square.DataContracts.SquareAddress() { Address1 = "3232 Lednier terr.", Province = "On", City = "Mississauga", PostalCode = "L4Y3Z8", Country = square.Model.Address.CountryEnum.CA },
                CardholderName = "Joe Doe",
                CardNonce = "fake-card-nonce-ok"
            };
            var ret = factory.CreateCustomerCard(trn);
        }

        [TestMethod]
        public void TestDeleteCustomerCard()
        {
            var factory = new ServiceCallFactory();
            var trn = new TrnDeleteCustomerCard()
            {
                AccessToken = "sandbox-sq0atb-M8S-5tUs0Is0Bo3Nnf3r0A",
                CustomerId = "CBASEBAHrQzWIxqUgI4gpMm6hRQgAQ",
                CardNonce = "fake-card-nonce-ok"
            };
            var ret = factory.DeleteCustomer(trn);
        }
    }
}

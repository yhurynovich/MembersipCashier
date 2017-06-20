using MembershipCashierDL.Access;
using MembershipCashierUnified.Contracts;
using MembershipCashierUnified.Interfaces;
using MembershipCashierW.Code.Documents.DocumentTemplates;
using SecurityUnified.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MembershipCashierW.Code.Documents.Generators
{
    public class LocationTransactionsReportGenerator
    {
        IEnumerable<int> locationIds;
        ILowLevelAccess Db;
        SecurityDL.Access.ILowLevelAccess SecrityDB;

        public IEnumerable<int> LocationIds
        {
            get
            {
                if (locationIds == null || !locationIds.Any())
                    locationIds = new int[] { SessionGlobal.CurrentLocation.LocationId };
                return locationIds;
            }
            set { locationIds = value; }
        }

        public string GetReportHTML(DateTime from, DateTime to)
        {
            var reportLocationIds = LocationIds;
            var locations = Db.FindLocation(new LocationDiscriminator() { Filter = x => reportLocationIds.Contains(x.LocationId) }, null, null);
            var reportAddresses = locations.Select(l=>l.Location.AddressId).ToArray();
            var addresses = Db.FindAddress(new AddressDiscriminator() { Filter = x => reportAddresses.Contains(x.AddressId) });
            var trns = Db.FindCreditTransaction(new CreditTransactionDiscriminator() { Filter = x => reportLocationIds.Contains(x.LocationId) && x.TransactionTime >= from && x.TransactionTime < to });
            var productIds = trns.Select(x => x.CreditTransaction.ProductId).Distinct().ToArray();
            var productFilter = new ProductDiscriminator() { Filter = x => productIds.Contains(x.ProductId) };
            var products = Db.FindProduct(productFilter, null, null);
            var prices = Db.FindProductPriceHistory(productFilter, true);
            var userIds = trns.Select(t => t.CreditTransaction.UserId).Distinct().ToArray();
            var users = SecrityDB.FindUserProfile(new UserProfileDiscriminator() { Filter = x=> userIds.Contains(x.UserId) });

            var trnTemplate = new LocationTransactions();
            trnTemplate.Locations = locations.Select(l => l.Location);
            trnTemplate.Addresses = addresses.Select(a=>a.Address);
            trnTemplate.From = from;
            trnTemplate.To = to;
            trnTemplate.CreditTransactions = trns.Select(x => x.CreditTransaction);
            trnTemplate.Products = products.Select(x => x.Product);
            trnTemplate.Prices = prices.Select(x => x.ProductPriceHistory);
            trnTemplate.Users = users.Select(u=>u.UserProfile);
            return trnTemplate.TransformText();
        }

        internal LocationTransactionsReportGenerator(ILowLevelAccess db, SecurityDL.Access.ILowLevelAccess securityDb)
        {
            this.Db = db;
            this.SecrityDB = securityDb;
        }
    }
}
using MembershipCashierDL.Access;
using MembershipCashierUnified.Contracts;
using MembershipCashierUnified.Interfaces;
using MembershipCashierW.Code.Documents.DocumentTemplates;
using SecurityUnified.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MembershipCashierW.Code.Documents.Generators
{
    public class UserReceiptGenerator
    {
        IEnumerable<int> locationIds;
        ILowLevelAccess Db;

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

        public int UserId { get; set; }

        public string GetReportHTML()
        {
            var reportLocationIds = LocationIds;
            IAddress address = null;
            if (SessionGlobal.CurrentLocation != null && SessionGlobal.CurrentLocation.AddressId.HasValue)
            {
                long curAddressId = SessionGlobal.CurrentLocation.AddressId.Value;
                var addressC = Db.FindAddress(new AddressDiscriminator() { Filter = x => x.AddressId == curAddressId }).FirstOrDefault();
                if (addressC != null)
                    address = addressC.Address;
            }
            var user = Db.FindUserProfile(new UserProfile2Discriminator() { Filter = x => x.UserId == UserId }, null, null).First();
            var locations = Db.FindLocation(new LocationDiscriminator() { Filter = x => reportLocationIds.Contains(x.LocationId) }, null, null);
            var lastTransaction = Db.FindCreditTransaction(new CreditTransactionDiscriminator() { Filter=x=> x.UserId==UserId && reportLocationIds.Contains(x.LocationId), Take = 1, OrderBy=new SortByFld[] { new SortByFld() { FieldName="Id", IsDescending=true } } }).FirstOrDefault();
            if (lastTransaction != null)
            {
                DateTime lastTrnTime = lastTransaction.CreditTransaction.TransactionTime.AddMinutes(-1);
                var reportLocationIds2 = LocationIds;
                var lastTransactions = Db.FindCreditTransaction(new CreditTransactionDiscriminator() { Filter = x => x.UserId == UserId && reportLocationIds2.Contains(x.LocationId) && x.TransactionTime >= lastTrnTime });
                var productIds = lastTransactions.Select(x => x.CreditTransaction.ProductId).Distinct().ToArray();
                var productFilter = new ProductDiscriminator() { Filter = x => productIds.Contains(x.ProductId) };
                var products = Db.FindProduct(productFilter, null, null, default(int));
                var prices = Db.FindProductPriceHistory(productFilter, true);
                var receiptTemplate = new Receipt();
                receiptTemplate.User = user.UserProfile2;
                receiptTemplate.Locations = locations.Select(l => l.Location);
                receiptTemplate.CreditTransactions = lastTransactions.Select(x => x.CreditTransaction);
                receiptTemplate.Products = products.Select(x => x.Product);
                receiptTemplate.Prices = prices.Select(x=>x.ProductPriceHistory);
                receiptTemplate.LocationAddress = address;
                return receiptTemplate.TransformText();
            }
            else
                return null;
        }

        internal UserReceiptGenerator(ILowLevelAccess db)
        {
            this.Db = db;
        }
    }
}
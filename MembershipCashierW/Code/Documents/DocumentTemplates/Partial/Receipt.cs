using MembershipCashierDL.MixedContracts;
using MembershipCashierUnified.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MembershipCashierW.Code.Documents.DocumentTemplates
{
    public partial class Receipt
    {
        public IEnumerable<ILocation> Locations { get; set; }
        /// <summary>
        /// Address of location that issued the receipt
        /// </summary>
        public IAddress LocationAddress { get; set; }
        public IEnumerable<ICreditTransaction> CreditTransactions { get; set; }
        public IEnumerable<IProduct> Products { get; set; }
        public IEnumerable<IProductPriceHistory> Prices { get; set; }
        public IUserProfile2 User { get; set; }

        public IProduct GetProduct(ICreditTransaction trn)
        {
            return Products.FirstOrDefault(x=>x.ProductId == trn.ProductId);
        }

        public decimal GetTotal()
        {
            var q = from t in CreditTransactions
                    join price in Prices on t.ProductId equals price.ProductId
                    select price.Price * t.BallanceUnits;
            return q.Sum(t=>t);
        }

        public string GetLocationDescription(ILocation loc)
        {
            return string.IsNullOrWhiteSpace(loc.Description) ? loc.LocationCode : loc.Description;
        }

        public string GetCurrentTimeStr()
        {
            if (SessionGlobal.CurrentLocation == null || string.IsNullOrEmpty( SessionGlobal.CurrentLocation.TimeZoneCode) )
                return $"{DateTime.UtcNow.ToString("MMMM d, yyyy h:mm tt")} UTC";
            else
                return DateTime.UtcNow.ToLocationLocalTimeStr();
        }
    }
}
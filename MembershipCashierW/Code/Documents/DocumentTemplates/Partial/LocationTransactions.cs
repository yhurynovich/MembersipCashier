using MembershipCashierUnified.Interfaces;
using SecurityUnified.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MembershipCashierW.Code.Documents.DocumentTemplates
{
    public partial class LocationTransactions
    {
        public IEnumerable<ILocation> Locations { get; set; }
        /// <summary>
        /// Address of location that issued the receipt
        /// </summary>
        public IEnumerable<IAddress> Addresses { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public IEnumerable<ICreditTransaction> CreditTransactions { get; set; }

        public IEnumerable<IProduct> Products { get; set; }

        public IEnumerable<IProductPriceHistory> Prices { get; set; }

        public IEnumerable<IUserProfile> Users { get; set; }

        public string GetLocationDescription(ILocation loc)
        {
            return string.IsNullOrWhiteSpace(loc.Description) ? loc.LocationCode : loc.Description;
        }

        public IProduct GetProduct(ICreditTransaction trn)
        {
            return Products.FirstOrDefault(x => x.ProductId == trn.ProductId);
        }

        public decimal GetTotalDebit()
        {
            var q = from t in CreditTransactions.Where(t=> IsCredit(t).HasValue && !IsCredit(t).Value)
                    join price in Prices on t.ProductId equals price.ProductId
                    select price.Price * t.BallanceUnits;
            return q.Sum(t => t);
        }

        public decimal GetTotalCredit()
        {
            var q = from t in CreditTransactions.Where(t => IsCredit(t).HasValue && IsCredit(t).Value)
                    join price in Prices on t.ProductId equals price.ProductId
                    select price.Price * t.BallanceUnits;
            return q.Sum(t => t);
        }

        public string GetUserName(ICreditTransaction trn)
        {
            var user = Users.FirstOrDefault(x => x.UserId == trn.UserId);
            if (user == null)
                return string.Empty;

            return $"{user.FirstName} {user.LastName} [{user.UserName}]";
        }

        public bool? IsCredit(ICreditTransaction trn)
        {
            var location = Locations.First(l=>l.LocationId == trn.LocationId);
            if (location.IsCredeitReversed)
                return trn.BallanceUnits > 0;
            else
                return trn.BallanceUnits < 0;
        }
    }
}
using MembershipCashierUnified.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MembershipCashierDL.Code
{
    internal class BallanceManager
    {
        internal DB.ProductPriceHistory FindProductPriceAtPointInTime(DB.MembershipCashierEntitiesDataContext Db, int productId, DateTime timeAt)
        {
            return Db.ProductPriceHistories.Where(x => x.ProductId == productId && x.ChangeDate <= timeAt).OrderByDescending(x => x.ChangeDate).First();
        }

        internal DB.ProfileCredit ReplaceCreditTransaction(DB.MembershipCashierEntitiesDataContext Db, DB.CreditTransaction prevCt, ICreditTransaction newCt)
        {
            var pc = Db.ProfileCredits.First(x => x.LocationId == prevCt.LocationId && x.ProductId == prevCt.ProductId && x.UserId == prevCt.UserId);
            var adjustmentUnits = newCt.BallanceUnits - prevCt.BallanceUnits;
            var oldPrice = FindProductPriceAtPointInTime(Db, prevCt.ProductId, prevCt.TransactionTime);
            var newPrice = FindProductPriceAtPointInTime(Db, prevCt.ProductId, prevCt.TransactionTime);
            var priceDiff = newPrice.Price - oldPrice.Price;
            if (adjustmentUnits != 0 || priceDiff != decimal.Zero)
            {
                if (adjustmentUnits == 0) //only price changed
                {
                    if(pc.Location.IsCredeitReversed)
                        pc.Ballance = pc.Ballance + (prevCt.BallanceUnits * oldPrice.Price) - (newCt.BallanceUnits * newPrice.Price);
                    else
                        pc.Ballance = pc.Ballance - (prevCt.BallanceUnits * oldPrice.Price) + (newCt.BallanceUnits * newPrice.Price);
                }
                else if (priceDiff == decimal.Zero) //price stayed the same
                {
                    pc.BallanceUnits += adjustmentUnits;
                    if (pc.Location.IsCredeitReversed)
                        pc.Ballance = pc.Ballance - (adjustmentUnits * oldPrice.Price);
                    else
                        pc.Ballance = pc.Ballance + (adjustmentUnits * oldPrice.Price);
                }
                else //both price and amount changed
                {
                    pc.BallanceUnits += adjustmentUnits;
                    if (pc.Location.IsCredeitReversed)
                        pc.Ballance = pc.Ballance + (prevCt.BallanceUnits * oldPrice.Price) - (newCt.BallanceUnits * newPrice.Price);
                    else
                        pc.Ballance = pc.Ballance - (prevCt.BallanceUnits * oldPrice.Price) + (newCt.BallanceUnits * newPrice.Price);
                }
            }

            return pc;
        }

        internal IEnumerable<DB.ProfileCredit> RecalcBallance(DB.MembershipCashierEntitiesDataContext Db, IEnumerable<DB.CreditTransaction> transactions)
        {
            List<DB.ProfileCredit> affectedProfileCredits = new List<DB.ProfileCredit>();
            foreach (var ct in transactions)
            {
                var pc = Db.ProfileCredits.FirstOrDefault(x => x.LocationId == ct.LocationId && x.ProductId == ct.ProductId && x.UserId == ct.UserId);

                if (pc == null)
                {
                    pc = new DB.ProfileCredit() { LocationId = ct.LocationId, ProductId = ct.ProductId, UserId = ct.UserId };
                    Db.ProfileCredits.InsertOnSubmit(pc);
                }

                pc.CalculatedTime = DateTime.UtcNow;

                //TODO: this logic is more complex then implemented below
                pc.BallanceUnits += ct.BallanceUnits;
                var product = Db.Products.First(p=>p.ProductId == ct.ProductId);
                var lastKnownPrice = product.ProductPriceHistories.LastOrDefault();
                if(lastKnownPrice!=null)
                    pc.Ballance += ct.BallanceUnits * product.ProductPriceHistories.LastOrDefault().Price;

                //Collect distinct affectedProfileCredits
                if (!affectedProfileCredits.Any(x => x.LocationId == ct.LocationId && x.ProductId == ct.ProductId && x.UserId == ct.UserId))
                    affectedProfileCredits.Add(pc);
            }
            return affectedProfileCredits;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MembershipCashierDL.Code
{
    internal class BallanceManager
    {
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

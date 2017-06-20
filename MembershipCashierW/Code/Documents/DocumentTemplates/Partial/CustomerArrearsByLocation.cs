using MembershipCashierDL.MixedContracts;
using MembershipCashierUnified.Interfaces;
using MembershipCashierW.Code.Documents.DocumentTemplates.Partial;
using SecurityUnified.Interfaces;
using System.Collections.Generic;
using System.Linq;
using static MembershipCashierW.Code.Documents.DocumentTemplates.Partial.CustomerBalancesBase;

namespace MembershipCashierW.Code.Documents.DocumentTemplates
{
    public partial class CustomerArrearsByLocation
    {
        private class WBase : CustomerBalancesBase
        {
            public override IEnumerable<IGrouping<int, ProductCreditContract>> CreditsGroupedByUser()
            {
                if (creditsGroupedByUser == null)
                    creditsGroupedByUser = ProfileCredits
                        .Where(x => IsCredit(x.ProfileCredit).HasValue && IsCredit(x.ProfileCredit).Value)
                        .GroupBy(c => c.ProfileCredit.UserId);

                return creditsGroupedByUser;
            }
        }

        private WBase worker;

        public IEnumerable<ILocation> Locations { get { return worker.Locations; } set { worker.Locations = value; } }
        /// <summary>
        /// Address of location that issued the receipt
        /// </summary>
        public IEnumerable<IAddress> Addresses { get { return worker.Addresses; } set { worker.Addresses = value; } }

        public IEnumerable<ProductCreditContract> ProfileCredits { get { return worker.ProfileCredits; } set { worker.ProfileCredits = value; } }

        public IEnumerable<IUserProfile> Users { get { return worker.Users; } set { worker.Users = value; } }

        public string GetLocationDescription(ILocation loc)
        {
            return worker.GetLocationDescription(loc);
        }

        public IEnumerable<IGrouping<int, ProductCreditContract>> CreditsGroupedByUser()
        {
            return worker.CreditsGroupedByUser();
        }

        public IEnumerable<UserTotalBallance> GetUserBallances()
        {
            return worker.GetUserBallances();
        }

        public string GetUserName(UserTotalBallance b)
        {
            return worker.GetUserName(b);
        }

        public bool? IsCredit(IProfileCredit trn)
        {
            return worker.IsCredit(trn);
        }

        public CustomerArrearsByLocation()
        {
            worker = new WBase();
        }
    }
}
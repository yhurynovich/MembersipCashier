using MembershipCashierDL.MixedContracts;
using MembershipCashierUnified.Interfaces;
using SecurityUnified.Interfaces;
using System.Collections.Generic;
using System.Linq;


namespace MembershipCashierW.Code.Documents.DocumentTemplates.Partial
{
    public class CustomerBalancesBase
    {
        protected IEnumerable<IGrouping<int, ProductCreditContract>> creditsGroupedByUser;

        private IEnumerable<UserTotalBallance> userBallances;

        public class UserTotalBallance
        {
            private IEnumerable<ProductCreditContract> credits;
            public int UserId { get; private set; }

            public decimal GetTotalCredit()
            {
                return credits.Sum(x => x.ProfileCredit.Ballance);
            }

            public UserTotalBallance(int userId, IEnumerable<ProductCreditContract> credits)
            {
                UserId = userId;
                this.credits = credits;
            }
        }

        public IEnumerable<ILocation> Locations { get; set; }
        /// <summary>
        /// Address of location that issued the receipt
        /// </summary>
        public IEnumerable<IAddress> Addresses { get; set; }

        public IEnumerable<ProductCreditContract> ProfileCredits { get; set; }

        public IEnumerable<IUserProfile> Users { get; set; }

        public string GetLocationDescription(ILocation loc)
        {
            return string.IsNullOrWhiteSpace(loc.Description) ? loc.LocationCode : loc.Description;
        }

        public virtual IEnumerable<IGrouping<int, ProductCreditContract>> CreditsGroupedByUser()
        {
            if (creditsGroupedByUser == null)
                creditsGroupedByUser = ProfileCredits
                    .GroupBy(c => c.ProfileCredit.UserId);

            return creditsGroupedByUser;
        }

        public IEnumerable<UserTotalBallance> GetUserBallances()
        {
            if (userBallances == null)
                userBallances = CreditsGroupedByUser()
                .Select(x => new UserTotalBallance(x.Key, x))
                .Where(x => x.GetTotalCredit() != null)
                .ToArray();

            return userBallances;
        }

        public string GetUserName(UserTotalBallance b)
        {
            var user = Users.FirstOrDefault(x => x.UserId == b.UserId);
            if (user == null)
                return string.Empty;

            return $"{user.FirstName} {user.LastName} [{user.UserName}]";
        }

        public bool? IsCredit(IProfileCredit trn)
        {
            var location = Locations.First(l => l.LocationId == trn.LocationId);
            if (location.IsCredeitReversed)
                return trn.BallanceUnits > 0;
            else
                return trn.BallanceUnits < 0;
        }
    }
}
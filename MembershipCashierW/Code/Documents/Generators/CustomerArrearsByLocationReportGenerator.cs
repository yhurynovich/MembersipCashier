using MembershipCashierDL.Access;
using MembershipCashierUnified.Contracts;
using MembershipCashierW.Code.Documents.DocumentTemplates;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MembershipCashierW.Code.Documents.Generators
{
    public class CustomerArrearsByLocationReportGenerator
    {
        IEnumerable<int> locationIds;
        ILowLevelAccess Db;
        SecurityDL.Access.ILowLevelAccess SecrityDB;
        ModeOptions mode;

        public enum ModeOptions
        {
            ARREARS,
            BALANCES
        }

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
            var reportAddresses = locations.Select(l => l.Location.AddressId).ToArray();
            var addresses = Db.FindAddress(new AddressDiscriminator() { Filter = x => reportAddresses.Contains(x.AddressId) });
            var locationUsers = Db.FindUserProfile(null, new UserProfileVsLocationDiscriminator() { }, null);
            var userIds = locationUsers.Select(u => u.UserProfile2.UserId).ToArray();
            var credits = Db.FindProfileCredit(new ProfileCreditDiscriminator() { Filter = x => x.HasBallance.HasValue && x.HasBallance.Value && userIds.Contains(x.UserId) });

            switch(mode)
            {
                case ModeOptions.ARREARS:
                    var arrearsTemplate = new CustomerArrearsByLocation();
                    arrearsTemplate.Locations = locations.Select(l => l.Location);
                    arrearsTemplate.Addresses = addresses.Select(a => a.Address);
                    arrearsTemplate.ProfileCredits = credits;
                    arrearsTemplate.Users = locationUsers.Select(u => u.UserProfile2);
                    return arrearsTemplate.TransformText();
                default:
                    var balanceTemplate = new CustomerBalancesByLocation();
                    balanceTemplate.Locations = locations.Select(l => l.Location);
                    balanceTemplate.Addresses = addresses.Select(a => a.Address);
                    balanceTemplate.ProfileCredits = credits;
                    balanceTemplate.Users = locationUsers.Select(u => u.UserProfile2);
                    return balanceTemplate.TransformText();
            }
        }

        internal CustomerArrearsByLocationReportGenerator(ILowLevelAccess db, ModeOptions mode)
        {
            this.Db = db;
            this.mode = mode;
        }
    }
}
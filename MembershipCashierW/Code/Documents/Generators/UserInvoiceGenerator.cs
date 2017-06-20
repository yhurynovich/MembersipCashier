using MembershipCashierDL.Access;
using MembershipCashierUnified.Contracts;
using MembershipCashierUnified.Interfaces;
using MembershipCashierW.Code.Documents.DocumentTemplates;
using System.Collections.Generic;
using System.Linq;

namespace MembershipCashierW.Code.Documents.Generators
{
    public class UserInvoiceGenerator
    {
        IEnumerable<int> locationIds;
        ILowLevelAccess Db;

        public IEnumerable<int> LocationIds
        {
            get
            {
                if (locationIds==null || !locationIds.Any())
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
            var user = Db.FindUserProfile(new UserProfile2Discriminator() { Filter = x=>x.UserId == UserId }, null, null).First();
            var userCredits = Db.FindProfileCredit(new ProfileCreditDiscriminator() { Filter=x=>x.HasBallance.HasValue && x.HasBallance.Value && x.UserId == UserId && reportLocationIds.Contains(x.LocationId) });
            var locations = Db.FindLocation(new LocationDiscriminator() { Filter=x=> reportLocationIds.Contains(x.LocationId) }, null, null);
            var invoiceTemplate = new Invoice();
            invoiceTemplate.User = user.UserProfile2;
            invoiceTemplate.Locations = locations.Select(l=>l.Location);
            invoiceTemplate.Credits = userCredits;
            invoiceTemplate.LocationAddress = address;
            return invoiceTemplate.TransformText();
        }

        internal UserInvoiceGenerator(ILowLevelAccess db)
        {
            this.Db = db;
        }
    }
}
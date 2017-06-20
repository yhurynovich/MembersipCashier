using MembershipCashierDL.MixedContracts;
using MembershipCashierUnified.Interfaces;
using System;
using System.Collections.Generic;

namespace MembershipCashierW.Code.Documents.DocumentTemplates
{
    public partial class Invoice
    {
        public IEnumerable<ILocation> Locations { get; set; }
        public IAddress LocationAddress { get; set; }
        public IEnumerable<ProductCreditContract> Credits { get; set; }
        public IUserProfile2 User { get; set; }

        public string GetLocationDescription(ILocation loc)
        {
            return string.IsNullOrWhiteSpace(loc.Description) ? loc.LocationCode : loc.Description;
        }

        public string GetCurrentTimeStr()
        {
            if (SessionGlobal.CurrentLocation == null)
                return $"{DateTime.UtcNow.ToString("MMMM d, yyyy h:mm tt")} UTC";
            else
                return DateTime.UtcNow.ToLocationLocalTimeStr();
        }
    }
}
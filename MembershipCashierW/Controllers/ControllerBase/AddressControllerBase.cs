using FedExIScanWeb.Code;
using RetailShipUnified.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MembershipCashierW.Controllers.ControllerBase
{
    public abstract class AddressControllerBase : WebApiControllerBase
    {
        protected IEnumerable<AddressValidationResult> Validate(IEnumerable<IIAddress> data, bool strict = true)
        {
            if (data == null || !data.Any())
                throw new ArgumentException("No Address change data submitted");

            if (SessionGlobal.CurrentPrincipal == null || SessionGlobal.CurrentPrincipal.Identity == null || !SessionGlobal.CurrentPrincipal.Identity.IsAuthenticated)
                throw new Exception("Current user is not authorized to make changes to Address");

            foreach (var addr in data)
                RetailShipUnified.Extentions_Validate.Trim(addr);

            return data.AsParallel().Select(d => d.Validate(strict).GetAwaiter().GetResult());
        }
    }
}
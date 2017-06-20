using MembershipCashierW.Controllers.Authorization;
using MembershipCashierW.Controllers.ControllerBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace MembershipCashierW.Controllers
{
    [RoleAuthorize(Roles = Constants.ALL_AUTHENTICATED)]
    public class UserLocationsController : WebApiControllerBase
    {
        public class UserLocationMixedImplementor
        {
            //public UserLocationImplementor UserLocation { get; set; }
            //public AddressImplementor Address { get; set; }
            //public string StoreId { get; set; }
        }

        public class ChangeLocationResponse
        {
            public bool Success { get; set; }
            public string Reason { get; set; }
        }

        [System.Web.Mvc.ValidateAntiForgeryToken]
        public IHttpActionResult Get()
        {
            return base.Ok();
            //return Execute(delegate
            //{
            //    if (SessionGlobal.CurrentUser == null)
            //        throw new SecurityUnified.Exceptions.Xxception(Errors.No_principal_or_device_found);

            //    var discriminator = new UserLocationDiscriminator { Filter = x => x.FedExIdentityId == SessionGlobal.CurrentPrincipal.FedExIdentity.FedExIdentityId };

            //    return base.Ok(Db.FindUserLocation(discriminator).Select(x => new UserLocationMixedImplementor
            //    {
            //        UserLocation = x.UserLocation as UserLocationImplementor,
            //        Address = x.Address as AddressImplementor,
            //        StoreId = x.StoreId
            //    }));
            //});
        }

        //[System.Web.Mvc.ValidateAntiForgeryToken]
        //public IHttpActionResult Post([FromBody] int locationId)
        //{
        //    return Execute(delegate
        //    {
        //        if (SessionGlobal.CurrentPrincipal == null || SessionGlobal.CurrentPrincipal.FedExIdentity == null)
        //            return base.Ok(new ChangeLocationResponse
        //            {
        //                Success = false,
        //                Reason = "Unauthorized"
        //            });

        //        int locId = locationId;
        //        int fedExIdentity = SessionGlobal.CurrentPrincipal.FedExIdentity.FedExIdentityId;

        //        var validLocationDiscriminator = new UserLocationDiscriminator { Filter = d => d.FedExIdentityId == fedExIdentity && d.LocationId == locId };
        //        var validUserLocation = Db.FindUserLocation(validLocationDiscriminator).Select(l => l.UserLocation as UserLocationImplementor).Any();

        //        if (!validUserLocation)
        //            return base.Ok(new ChangeLocationResponse
        //            {
        //                Success = false,
        //                Reason = "Unauthorized"
        //            });

        //        var findMasterDiscriminator = new DeviceDiscriminator { Filter = d => d.LocationId == locationId };
        //        var master = Db.FindDevice(findMasterDiscriminator).Select(d => d.Device as DeviceImplementor).FirstOrDefault(d => (d.RetailTypeCode == "ms" || d.RetailTypeCode == "ma"));

        //        if (master == null)
        //            return base.Ok(new ChangeLocationResponse
        //            {
        //                Success = false,
        //                Reason = ErrorMessages.Current_location_is_missing_a_Master_Device
        //            });

        //        SessionGlobal.CurrentDevice = master;

        //        var locations = SessionGlobal.CurrentUserLocations;
        //        foreach (var location in locations)
        //        {
        //            if (location.LocationId == master.LocationId)
        //                location.IsCurrentLocation = true;
        //        }

        //        //if (HttpContext.Current.Session["CurrentUser"] != null)
        //        //    SessionGlobal.CurrentUser.CurrentLocationId = master.LocationId; //Legacy support

        //        return base.Ok(new ChangeLocationResponse
        //        {
        //            Success = true,
        //        });
        //    });
        //}
    }
}
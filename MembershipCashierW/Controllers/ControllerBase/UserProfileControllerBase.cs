using MembershipCashierUnified.Contracts;
using MembershipCashierUnified.Interfaces;
using MembershipCashierW.Controllers.Authorization;
using SecurityUnified.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace MembershipCashierW.Controllers.ControllerBase
{
    [RoleAuthorize(Roles = Constants.USERS_WITH_PROFILE_ACCESS)]
    public abstract class UserProfileControllerBase : WebApiControllerBase
    {
        private UserProfileVsLocationDiscriminator authorizedLocationFilter;

        protected virtual int[] AuthorizedLocations
        {
            get
            {
                return SessionGlobal.CurrentUserOwnedLocations.Select(l => l.LocationId).ToArray();
            }
        }

        protected virtual int CurrentLocationId
        {
            get {
                return SessionGlobal.CurrentLocation.LocationId;
            }
        }

        protected virtual UserProfileVsLocationDiscriminator AuthorizedLocationFilter
        {
            get {
                if (authorizedLocationFilter == null)
                {
                    var authorizedLocationIds = AuthorizedLocations;
                    if (authorizedLocationIds.Any())
                        authorizedLocationFilter = new UserProfileVsLocationDiscriminator() { Filter = x => authorizedLocationIds.Contains(x.LocationId) };
                    else
                        authorizedLocationFilter = new UserProfileVsLocationDiscriminator();
                }

                return authorizedLocationFilter;
            }
        }

        protected Expression<Func<ILocation, bool>> EnhanceFilterByAuthorizedLocations(Expression<Func<ILocation, bool>> filterToEnhance)
        {
            var authorizedLocationIds = AuthorizedLocations;
            if (authorizedLocationIds.Any())
            {
                if (filterToEnhance == null)
                    return x => authorizedLocationIds.Contains(x.LocationId);
                else
                {
                    var param = filterToEnhance.Parameters[0];
                    Expression<Func<ILocation, bool>> locOrPart;
                    Expression locAndPart = Expression.Constant(true);
                    foreach (int locId in authorizedLocationIds)
                    {
                        locOrPart = x => x.LocationId == locId;
                        locAndPart = Expression.AndAlso(locOrPart.Body, locAndPart);
                    }

                    return Expression.Lambda<Func<ILocation, bool>>(Expression.AndAlso(filterToEnhance.Body, locAndPart), param);
                }
            }

            return filterToEnhance;
        }

        //protected virtual Expression<Func<IUserProfile, bool>> GetOwnerStoreFilter()
        //{
        //    if (SessionGlobal.CurrentUser != null)
        //    {
        //        var ownerProfile = SessionGlobal.CurrentUser.Identity.UserProfile;
        //        if(ownerProfile!=null && ownerProfile.)
        //    }
        //}
    }
}
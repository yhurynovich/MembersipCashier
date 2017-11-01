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
using System.Reflection;
using System.Web;

namespace MembershipCashierW.Controllers.ControllerBase
{
    [RoleAuthorize(Roles = Constants.ALL_AUTHENTICATED)]
    public abstract class CreditTransactionControllerBase : WebApiControllerBase
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
            get
            {
                return SessionGlobal.CurrentLocation.LocationId;
            }
        }

        protected virtual UserProfileVsLocationDiscriminator AuthorizedLocationFilter
        {
            get
            {
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

        protected Expression<Func<ICreditTransaction, bool>> EnhanceFilterByAuthorizedLocations(Expression<Func<ICreditTransaction, bool>> filterToEnhance)
        {
            var authorizedLocationIds = AuthorizedLocations;
            if (authorizedLocationIds.Any())
            {
                if (filterToEnhance == null)
                    return x => authorizedLocationIds.Contains(x.LocationId);
                else
                {
                    var param = filterToEnhance.Parameters[0];
                    BinaryExpression locOrPart;
                    Expression locAndPart = Expression.Constant(true);

                    var locationIdProperty = typeof(ICreditTransaction).GetPublicProperties().First(x => x.Name == "LocationId");
                    var field = Expression.Property(param, locationIdProperty);
                    foreach (int locId in authorizedLocationIds)
                    {
                        locOrPart = Expression.Equal(field, Expression.Constant(locId));
                        locAndPart = Expression.AndAlso(locOrPart, locAndPart);
                    }

                    return Expression.Lambda<Func<ICreditTransaction, bool>>(Expression.AndAlso(filterToEnhance.Body, locAndPart), param);
                }
            }

            return filterToEnhance;
        }
    }
}
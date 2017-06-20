using MembershipCashierUnified.Contracts;
using MembershipCashierUnified.Interfaces;
using MembershipCashierW.Models;
using SecurityUnified.Contracts;
using SecurityUnified.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Web.Helpers;
using System.Web.Http.Controllers;

namespace MembershipCashierW
{
    public static class Extentions //TODO: cache db service?
    {
        public static void ValidateRequestHeader(this HttpActionContext actionContext)
        {
            HttpRequestMessage request = actionContext.Request;
            string cookieToken = "";
            string formToken = "";

            CookieHeaderValue cookie = request.Headers.GetCookies(WebApiApplication.ANTIFORGERY_TOKEN_HEADER_NAME).FirstOrDefault();
            if (cookie != null)
            {
                cookieToken = cookie[WebApiApplication.ANTIFORGERY_TOKEN_HEADER_NAME].Value;
                if (!string.IsNullOrWhiteSpace(cookieToken))
                {
                    IEnumerable<string> tokenHeaders;
                    if (request.Headers.TryGetValues("RequestVerificationToken", out tokenHeaders))
                    {
                        formToken = tokenHeaders.First();
                    }
                }
            }

            AntiForgery.Validate(cookieToken, formToken);
        }

        /// <summary>
        /// Retreives all owners by location
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static IEnumerable<IOwner> GetOwners(this IHasLocationId location)
        {
            var db = new MembershipCashierDL.Access.LowLevelAccess();
            return db.FindOwner(null, new LocationDiscriminator() { }).Select(o => o.Owner).ToArray();
        }

        /// <summary>
        /// Looks up user locations
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static IEnumerable<LocationModel> GetLocations(this IHasUserId user)
        {
            var db = new MembershipCashierDL.Access.LowLevelAccess();
            return db.FindLocation(null, null, new UserProfileDiscriminator() { Filter = x => x.UserId == user.UserId }).Select(l => l.Location.ToLocationModel() );
        }

        public static IEnumerable<LocationModel> GetLocations(this IHasOwnerId user)
        {
            var db = new MembershipCashierDL.Access.LowLevelAccess();
            return db.FindLocation(null, new OwnerVsLocationDiscriminator() { Filter = x => x.OwnerId == user.OwnerId }, null).Select(l => l.Location.ToLocationModel());
        }

        public static LocationModel ToLocationModel(this ILocation l)
        {
            return new LocationModel() { LocationCode = l.LocationCode, LocationId = l.LocationId };
        }

        public static OwnerPrincipal ToOwnerPrincipal(this IUserProfile profile)
        {
            var ret = new OwnerPrincipal(new GenericIdentity(profile.UserName), profile);
            if (ret.IsInRole(Constants.OWNER))
            {
                var db = new MembershipCashierDL.Access.LowLevelAccess();
                var owner = db.FindOwner(new OwnerDiscriminator() { Filter = x => x.OwnerUserId == profile.UserId }, null).FirstOrDefault();
                if (owner != null)
                    ret.Owner = owner.Owner;
            }
                
            return ret;
        }

        /// <summary>
        /// Gets IOwner record by IUserProfile record
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        public static IOwner GetRelatedOwner(this IUserProfile profile)
        {
            var db = new MembershipCashierDL.Access.LowLevelAccess();
            var owner = db.FindOwner(new OwnerDiscriminator() { Filter = x => x.OwnerUserId == profile.UserId }, null).FirstOrDefault();
            if (owner != null)
                return owner.Owner;

            return null;
        }

        public static string ToShortAddressStr(this IAddress addr)
        {
            var sb = new StringBuilder(addr.Address1);
            sb.Append(", ");
            sb.Append(addr.City);
            sb.Append(" ");
            if (string.IsNullOrEmpty(addr.Province))
                sb.Append(addr.ProvinceName);
            else
                sb.Append(addr.Province);
            sb.Append(", ");
            sb.Append(addr.PostalCode);
            sb.Append(" ");
            sb.Append(addr.Country);
            return sb.ToString();
        }

        #region DateTime manipulation

        public static DateTime Floor(this DateTime dateTime, TimeSpan interval)
        {
            return dateTime.AddTicks(-(dateTime.Ticks % interval.Ticks));
        }

        public static DateTime Ceiling(this DateTime dateTime, TimeSpan interval)
        {
            var overflow = dateTime.Ticks % interval.Ticks;

            return overflow == 0 ? dateTime : dateTime.AddTicks(interval.Ticks - overflow);
        }

        public static DateTime Round(this DateTime dateTime, TimeSpan interval)
        {
            var halfIntervelTicks = (interval.Ticks + 1) >> 1;

            return dateTime.AddTicks(halfIntervelTicks - ((dateTime.Ticks + halfIntervelTicks) % interval.Ticks));
        }

        public static DateTime RoundDateTime(this DateTime dt, TimeSpan roundSpan)
        {
            return new DateTime(((dt.Ticks + roundSpan.Ticks - 1) / roundSpan.Ticks) * roundSpan.Ticks);
        }

        public static DateTime FloorDateTime(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day);
        }

        public static DateTime ToAdjustedUTC_or_Current(this DateTime dt, TimeSpan timeZoneOffset)
        {
            if (dt == default(DateTime))
                dt = DateTime.UtcNow;
            else if (timeZoneOffset != TimeSpan.Zero)
                dt = dt.ToUniversalTime().Add(timeZoneOffset);
            return dt;
        }

        public static TimeSpan GetDlsTimeZoneOffSet(this ILocation location)
        {
            if (location == null)
                return TimeSpan.Zero;

            return MembershipCashierUnified.Code.TimeZones.GetTimeZoneOffset(location.TimeZoneCode);
        }

        public static DateTime ToLocationLocalTime(this DateTime dt)
        {
            var location = SessionGlobal.CurrentLocation;
            if (location == null) throw new Exception("Location not found");
            var utcOffset = new DateTimeOffset(dt, TimeSpan.Zero);
            var timeZone = MembershipCashierUnified.Code.TimeZones.GetTimeZone(location.TimeZoneCode);
            return utcOffset.ToOffset(TimeZoneInfo.FindSystemTimeZoneById(timeZone).GetUtcOffset(utcOffset)).DateTime;
        }

        public static string ToLocationLocalTimeStr(this DateTime dt)
        {
            return ToLocationLocalTime(dt).ToString("MMMM d, yyyy h:mm tt");
        }
        #endregion
    }
}
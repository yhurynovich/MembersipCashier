using MembershipCashierDL.Access;
using MembershipCashierUnified.Interfaces;
using MembershipCashierW.Code;
using MembershipCashierW.Models;
using MembershipCashierW.Properties;
using SecurityUnified.Contracts;
using SecurityUnified.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace MembershipCashierW
{
    public static class SessionGlobal
    {
        public static int RetryCount { get { return HttpContext.Current.Session["LoginRetryCount"] == null ? 0 : (int)HttpContext.Current.Session["LoginRetryCount"]; } set { HttpContext.Current.Session["LoginRetryCount"] = value; } }

        //public static CountryOptions? Country
        //{
        //    get
        //    {
        //        string ret = null;
        //        if (HttpContext.Current != null && HttpContext.Current.Request["country"] != null)
        //        {
        //            ret = HttpContext.Current.Request["country"];
        //            if (HttpContext.Current.Session != null)
        //            {
        //                HttpContext.Current.Session["Country"] = ret;
        //                HttpCookie cook = HttpContext.Current.Response.Cookies["Country"];
        //                if (cook == null)
        //                    HttpContext.Current.Response.Cookies.Add(new HttpCookie("Country", ret));
        //                else
        //                    cook.Value = ret;
        //            }
        //        }
        //        if (string.IsNullOrEmpty(ret) && HttpContext.Current != null && HttpContext.Current.Session != null)
        //        {
        //            ret = Convert.ToString(HttpContext.Current.Session["Country"]);
        //        }
        //        if (string.IsNullOrEmpty(ret) && HttpContext.Current != null && HttpContext.Current.Request.Cookies["Country"] != null)
        //        {
        //            ret = HttpContext.Current.Request.Cookies["Country"].Value;
        //        }

        //        if (string.IsNullOrEmpty(ret))
        //        {
        //            Pool<DBUsersLqDataContext>.PoolItem db = null;
        //            try
        //            {
        //                db = UsersLqDBPool.Instance.GetDB();

        //                if (SessionGlobal.CurrentUser == null)
        //                {
        //                    UnknownCountry();
        //                    return null;
        //                }

        //                var loggedInUserCountries = SessionGlobal.CurrentUser.SelectedCountries;

        //                if (loggedInUserCountries.Count() == 1)
        //                {
        //                    return (CountryOptions)Enum.Parse(typeof(CountryOptions), loggedInUserCountries.First());
        //                }
        //                else
        //                {
        //                    UnknownCountry();
        //                    return null;
        //                }

        //                ////TODO: could raise a problem to a user with multiple contries
        //                //if (loggedInUserCountries.Count() == 1)
        //                //{
        //                //    return (CountryOptions)Enum.Parse(typeof(CountryOptions), loggedInUserCountries.First());
        //                //}
        //                //else
        //                //{
        //                //    if (UnknownCountry != null)
        //                //    {
        //                //        UnknownCountry();
        //                //        return CountryOptions.CA;
        //                //    }
        //                //    else
        //                //    {
        //                //        throw new Exception("Unable to identify user's country association");
        //                //    }
        //                //}
        //            }
        //            finally
        //            {
        //                UsersLqDBPool.Instance.Return(db);
        //            }
        //        }
        //        else
        //            return (CountryOptions)Enum.Parse(typeof(CountryOptions), ret);
        //    }
        //    set
        //    {
        //        if (HttpContext.Current.Session != null)
        //        {
        //            HttpContext.Current.Session["Country"] = value;
        //        }
        //        HttpCookie cook = HttpContext.Current.Response.Cookies["Country"];
        //        if (cook == null)
        //            HttpContext.Current.Response.Cookies.Add(new HttpCookie("Country", value.ToString()));
        //        else
        //            cook.Value = value.ToString();
        //    }
        //}

        public static string Language
        {
            get
            {
#if ALL_IN_FRENH
                System.Threading.Thread.CurrentThread.CurrentCulture
                    = System.Threading.Thread.CurrentThread.CurrentUICulture
                    = new System.Globalization.CultureInfo("fr");
                return "fr";
#endif
                string ret = null;
                if (HttpContext.Current!=null)
                {
                    if (HttpContext.Current.Session != null)
                    {
                        ret = (string)HttpContext.Current.Session["Language"];
                    }
                    if (string.IsNullOrEmpty(ret) && HttpContext.Current.Request["lng"] != null)
                    {
                        ret = HttpContext.Current.Request["lng"];
                    }
                    if (string.IsNullOrEmpty(ret) && HttpContext.Current.Request.Cookies["Language"] != null)
                    {
                        ret = HttpContext.Current.Request.Cookies["Language"].Value;
                    }
                }

                if (string.IsNullOrEmpty(ret))
                    return System.Globalization.CultureInfo.InvariantCulture.IetfLanguageTag;
                else
                    return ret;
            }
            set
            {
                if (HttpContext.Current.Session != null)
                {
                    HttpContext.Current.Session["Language"] = value;
                }
                HttpCookie cook = HttpContext.Current.Response.Cookies["Language"];
                if (cook == null)
                    HttpContext.Current.Response.Cookies.Add(new HttpCookie("Language", value));
                else
                    cook.Value = value;
            }
        }

        public static LocationModel CurrentLocation
        {
            get
            {
                LocationModel ret = null;

                if (HttpContext.Current != null
                    && HttpContext.Current.Session != null
                    && HttpContext.Current.Session["CurrentLocation"] != null
                    && typeof(LocationModel).IsInstanceOfType(HttpContext.Current.Session["CurrentLocation"]))
                {
                    ret = (LocationModel)HttpContext.Current.Session["CurrentLocation"];
                }

                return ret;
            }
            set
            {
                HttpContext.Current.Session["CurrentLocation"] = value;
            }
        }

        public static IEnumerable<LocationModel> CurrentUserOwnedLocations
        {
            get {
                IEnumerable<LocationModel> ret = null;

                if (HttpContext.Current != null
                    && HttpContext.Current.Session != null
                    && HttpContext.Current.Session["CurrentUserOwnedLocations"] != null
                    && typeof(LocationModel).IsInstanceOfType(HttpContext.Current.Session["CurrentUserOwnedLocations"]))
                {
                    ret = (IEnumerable<LocationModel>)HttpContext.Current.Session["CurrentUserOwnedLocations"];
                }
                else
                {
                    var currentUser = CurrentUser;
                    if (currentUser != null && currentUser.Owner!=null) //TODO: this takes two db calls. Need to optimize
                    {
                        ret = (currentUser.Owner as IHasOwnerId).GetLocations();
                        HttpContext.Current.Session["CurrentUserOwnedLocations"] = ret;
                    }
                }

                return ret;
            }
            set {
                HttpContext.Current.Session["CurrentUserLocations"] = value;
            }
        }

        #region Current User Principal
        public static OwnerPrincipal CurrentUser
        {
            get
            {
                try
                {
                    OwnerPrincipal ret = null;

                    if (HttpContext.Current != null 
                        && HttpContext.Current.Session != null 
                        && HttpContext.Current.Session["CurrentUser"] != null
                        && typeof(CompletePrincipal).IsInstanceOfType(HttpContext.Current.Session["CurrentUser"]))
                    {
                        CompletePrincipal princpl = (CompletePrincipal)HttpContext.Current.Session["CurrentUser"];

                        if (typeof(OwnerPrincipal).IsInstanceOfType(princpl))
                            ret = (OwnerPrincipal) princpl;
                        else
                        {
                            ret = new OwnerPrincipal(princpl.Identity, princpl.Identity.UserProfile);
                        }
                    }

                    if(ret == null)
                        ret = GetLoggedInUser();

                    return ret;
                }
                catch (Exception ex)
                {
                    Utils.WriteToEventLog(ex);
                    return null;
                }
            }
        }

        private static OwnerPrincipal GetLoggedInUser()
        {
            if(HttpContext.Current == null 
                || HttpContext.Current.User == null 
                || HttpContext.Current.User.Identity == null
                || string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                return null;


            var profile = ServiceAccessor.SecurityDb.FindUserProfile(new UserProfileDiscriminator() { Filter = x => x.UserName == HttpContext.Current.User.Identity.Name }).Single();
            //return new CompletePrincipal(HttpContext.Current.User.Identity, profile.UserProfile);
            return profile.UserProfile.ToOwnerPrincipal();
        }
        #endregion

        #region ServiceAntiforgery
        private static Code.Collections.HalfPersistentList<string> ServiceAntiforgeryTokens
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.Session["ServiceAntiforgeryTokens"] != null)
                {
                    return (Code.Collections.HalfPersistentList<string>)HttpContext.Current.Session["ServiceAntiforgeryTokens"];
                }
                return new Code.Collections.HalfPersistentList<string>(Properties.Settings.Default.PersistantAntiforgeryTokens, Settings.Default.ServiceAntiforgeryHistoryDepth);
            }
        }

        public static IEnumerable<string> GetServiceAntiforgeryTokens()
        {
            return ServiceAntiforgeryTokens;
        }

        public static string RegisterServiceAntiforgeryToken(string token)
        {
            if (HttpContext.Current != null)
            {
                bool monitored = false;
                try
                {
                    var curTokens = ServiceAntiforgeryTokens;
                    if (System.Threading.Monitor.TryEnter(HttpContext.Current.Session))
                    {
                        monitored = true;
                        curTokens.Add(token);
                        HttpContext.Current.Session["ServiceAntiforgeryTokens"] = curTokens;
                        return token;
                    }
                    else if (curTokens.Any())
                        return curTokens[curTokens.Count - 1];
                    else
                        return null;
                }
                finally
                {
                    if (monitored)
                        System.Threading.Monitor.Exit(HttpContext.Current.Session);
                }
            }

            return null;
        }

        #endregion
    }
}
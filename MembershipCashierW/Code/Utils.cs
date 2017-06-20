using MembershipCashierW.Code.Enumerations;
using MembershipCashierW.Controllers;
using MembershipCashierW.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace MembershipCashierW.Code
{
    public static class Utils
    {
        private static long[][] _privateIps = new long[][] {
          new long[] {ConvertIPToLong("0.0.0.0"), ConvertIPToLong("2.255.255.255")},
          new long[] {ConvertIPToLong("10.0.0.0"), ConvertIPToLong("10.255.255.255")},
          new long[] {ConvertIPToLong("127.0.0.0"), ConvertIPToLong("127.255.255.255")},
          new long[] {ConvertIPToLong("169.254.0.0"), ConvertIPToLong("169.254.255.255")},
          new long[] {ConvertIPToLong("172.16.0.0"), ConvertIPToLong("172.31.255.255")},
          new long[] {ConvertIPToLong("192.0.2.0"), ConvertIPToLong("192.0.2.255")},
          new long[] {ConvertIPToLong("192.168.0.0"), ConvertIPToLong("192.168.255.255")},
          new long[] {ConvertIPToLong("255.255.255.0"), ConvertIPToLong("255.255.255.255")}
        };

        //public static string FormatPhone(string phone)
        //{
        //    if (string.IsNullOrWhiteSpace(phone))
        //        return string.Empty;

        //    return Regex.Replace(phone, Constants.PHONE_FORMAT_RegEx.Key, Constants.PHONE_FORMAT_RegEx.Value);
        //}

        //public static string GetDescription(this Enum value)
        //{
        //    FieldInfo field = value.GetType().GetField(value.ToString());

        //    DisplayAttribute attribute
        //            = Attribute.GetCustomAttribute(field, typeof(DisplayAttribute))
        //                as DisplayAttribute;

        //    if (attribute.ResourceType != null && attribute.ResourceType.Name == "FieldNames")
        //        return FieldNames.ResourceManager.GetString(value.ToString());
        //    else
        //        return attribute == null ? value.ToString() : attribute.Description;
        //}

        #region IP

        public static string DetermineIP(HttpRequest request)
        {
            if (request.ServerVariables.AllKeys.Contains("HTTP_CLIENT_IP") && CheckIP(request.ServerVariables["HTTP_CLIENT_IP"]))
                return request.ServerVariables["HTTP_CLIENT_IP"];

            if (request.ServerVariables.AllKeys.Contains("HTTP_X_FORWARDED_FOR"))
                foreach (string ip in request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(','))
                    if (CheckIP(ip.Trim()))
                        return ip.Trim();

            if (request.ServerVariables.AllKeys.Contains("HTTP_X_FORWARDED") && CheckIP(request.ServerVariables["HTTP_X_FORWARDED"]))
                return request.ServerVariables["HTTP_X_FORWARDED"];

            if (request.ServerVariables.AllKeys.Contains("HTTP_X_CLUSTER_CLIENT_IP") && CheckIP(request.ServerVariables["HTTP_X_CLUSTER_CLIENT_IP"]))
                return request.ServerVariables["HTTP_X_CLUSTER_CLIENT_IP"];

            if (request.ServerVariables.AllKeys.Contains("HTTP_FORWARDED_FOR") && CheckIP(request.ServerVariables["HTTP_FORWARDED_FOR"]))
                return request.ServerVariables["HTTP_FORWARDED_FOR"];

            if (request.ServerVariables.AllKeys.Contains("HTTP_FORWARDED") && CheckIP(request.ServerVariables["HTTP_FORWARDED"]))
                return request.ServerVariables["HTTP_FORWARDED"];

            return request.ServerVariables["REMOTE_ADDR"];
        }


        public static string DetermineIP(HttpContextBase context)
        {
            if (context.Request.ServerVariables.AllKeys.Contains("HTTP_CLIENT_IP") && CheckIP(context.Request.ServerVariables["HTTP_CLIENT_IP"]))
                return context.Request.ServerVariables["HTTP_CLIENT_IP"];

            if (context.Request.ServerVariables.AllKeys.Contains("HTTP_X_FORWARDED_FOR"))
                foreach (string ip in context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(','))
                    if (CheckIP(ip.Trim()))
                        return ip.Trim();

            if (context.Request.ServerVariables.AllKeys.Contains("HTTP_X_FORWARDED") && CheckIP(context.Request.ServerVariables["HTTP_X_FORWARDED"]))
                return context.Request.ServerVariables["HTTP_X_FORWARDED"];

            if (context.Request.ServerVariables.AllKeys.Contains("HTTP_X_CLUSTER_CLIENT_IP") && CheckIP(context.Request.ServerVariables["HTTP_X_CLUSTER_CLIENT_IP"]))
                return context.Request.ServerVariables["HTTP_X_CLUSTER_CLIENT_IP"];

            if (context.Request.ServerVariables.AllKeys.Contains("HTTP_FORWARDED_FOR") && CheckIP(context.Request.ServerVariables["HTTP_FORWARDED_FOR"]))
                return context.Request.ServerVariables["HTTP_FORWARDED_FOR"];

            if (context.Request.ServerVariables.AllKeys.Contains("HTTP_FORWARDED") && CheckIP(context.Request.ServerVariables["HTTP_FORWARDED"]))
                return context.Request.ServerVariables["HTTP_FORWARDED"];

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }

        private static bool CheckIP(string ip)
        {
            if (!String.IsNullOrEmpty(ip))
            {
                long ipToLong = -1;
                //Is it valid IP address
                if (TryConvertIPToLong(ip, out ipToLong))
                {
                    //Does it fall within a private network range
                    foreach (long[] privateIp in _privateIps)
                        if ((ipToLong >= privateIp[0]) && (ipToLong <= privateIp[1]))
                            return false;
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        private static bool TryConvertIPToLong(string ip, out long ipToLong)
        {
            try
            {
                ipToLong = ConvertIPToLong(ip);
                return true;
            }
            catch
            {
                ipToLong = -1;
                return false;
            }
        }

        private static long ConvertIPToLong(string ip)
        {
            string[] ipSplit = ip.Split('.');
            return (16777216 * Convert.ToInt32(ipSplit[0]) + 65536 * Convert.ToInt32(ipSplit[1]) + 256 * Convert.ToInt32(ipSplit[2]) + Convert.ToInt32(ipSplit[3]));
        }
        #endregion

        private static string GetErrorMessageByType(Exception source, Type t)
        {
            Exception avatar = source;
            string message = string.Empty;
            while (avatar != null)
            {
                if (message == null)
                    message = avatar.Message;
                if (source.GetType() == t)
                {
                    message = avatar.Message;
                    break;
                }
            }

            return message;
        }

        #region Dealing with errors

        #region HandleError
        internal static void HandleError(this ApiController controller, Exception ex)
        {
            Exception displayException = ex;

            try
            {
                string message = null;
                //if (typeof(MembershipCreateUserException).IsInstanceOfType(ex))
                //{
                //    message = GetErrorMessageByType(ex, typeof(MembershipCreateUserException));
                //}
                //else if (typeof(ExternalSystemTimeOutException).IsInstanceOfType(ex))
                //{
                //    message = GetErrorMessageByType(ex, typeof(ExternalSystemTimeOutException));
                //}
                //else if (typeof(InvalidTrackingEntryException).IsInstanceOfType(ex))
                //{
                //    message = GetErrorMessageByType(ex, typeof(InvalidTrackingEntryException));
                //}
                //else
                //{
                message = LocalizationController.Instance.Localize(key: "CriticalException", type: typeof(Errors).ToString(), resultCulture: new CultureInfo(SessionGlobal.Language)).First().Value;
                displayException = new Exception(string.IsNullOrWhiteSpace(message) ? ex.Message : message);
                //}
            }
            catch (Exception ex2)
            {
                Utils.WriteToEventLog(ex2);
            }
            finally
            {
                //TODO: Categorize and write critical only
                WriteToEventLog(ex);
            }

            if (HttpContext.Current != null)
                HttpContext.Current.AddError(displayException);
        }
        #endregion

        #region WriteToEventLog

        public static void WriteToEventLog(string message, ModelMessageTypeOptions severity = ModelMessageTypeOptions.ERROR)
        {
            try
            {
                EventLogEntryType logtype;
                switch (severity)
                {
                    case ModelMessageTypeOptions.ERROR:
                        logtype = EventLogEntryType.Error;
                        break;
                    case ModelMessageTypeOptions.SUCCESS:
                        logtype = EventLogEntryType.SuccessAudit;
                        break;
                    case ModelMessageTypeOptions.YES_NO:
                        logtype = EventLogEntryType.Warning;
                        break;
                    case ModelMessageTypeOptions.INFO:
                        logtype = EventLogEntryType.Information;
                        break;
                    default:
                        logtype = EventLogEntryType.Error;
                        break;
                }
                EventLog.WriteEntry(Settings.Default.LogSourceName, message, logtype);
            }
            catch
            { 
                //nowhere to log?
            }
        }

        public static void WriteToEventLog(Exception ex)
        {
            var traceStr = "failed to build trace";
            try
            {
                if (typeof(SecurityUnified.Exceptions.Xxception).IsInstanceOfType(ex) && ((SecurityUnified.Exceptions.Xxception)ex).IsLogged
                    || typeof(HttpAntiForgeryException).IsInstanceOfType(ex))
                    return;

                traceStr = ConcatTrace(ex);
                EventLog.WriteEntry(Settings.Default.LogSourceName
                    , traceStr
                    , EventLogEntryType.Error);

                if (typeof(SecurityUnified.Exceptions.Xxception).IsInstanceOfType(ex))
                    ((SecurityUnified.Exceptions.Xxception)ex).IsLogged = true;
            }
            catch { }
            //finally
            //{
            //    try
            //    {
            //        var logger = new System.SystemEvents();
            //        logger.AddSystemEvent(new SystemEventImplementor[1]{ new SystemEventImplementor() {
            //                 EventDesc = traceStr,
            //                 Severity = Convert.ToByte(SystemEventSeverity.CRITICAL),
            //                 TimeStamp = DateTime.UtcNow
            //            }});
            //    }
            //    catch
            //    { }
            //}
        }

        #endregion

        #region ConcatTrace

        private static string ConcatTraceForWeb(Exception exp)
        {
            return ConcatTrace(exp, true);
        }

        private static string ConcatTrace(Exception exp, bool forweb = false)
        {
            try
            {
                string newLine;
                if (forweb)
                    newLine = "<br />";
                else
                    newLine = Environment.NewLine;

                var sb = new StringBuilder();
                Exception ex = exp;
                while (ex != null)
                {
                    sb.Append(newLine);
                    sb.Append(ex.Message);
                    sb.Append(newLine);
                    sb.Append(ex.StackTrace == null ? string.Empty : ex.StackTrace.Replace("\\n", newLine));
                    ex = ex.InnerException;
                    if (ex != null)
                    {
                        sb.Append(newLine);
                        sb.Append("-----------------------------------");
                        sb.Append(newLine);
                    }
                }
                return sb.ToString(); ;
            }
            catch (Exception cex)
            {
                return cex.Message;
            }
        }

        #endregion

        #endregion

        public static DateTime RoundDateTime(this DateTime dt, TimeSpan roundSpan)
        {
            return new DateTime(((dt.Ticks + roundSpan.Ticks - 1) / roundSpan.Ticks) * roundSpan.Ticks);
        }

        public static DateTime FloorDateTime(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day);
        }

        //[Authorize(Roles = Constants.ALL_ADMINS_AND_MARKETING)]
        //public static Thread[] GetActiveThreads(Func<Thread, bool> filter = null)
        //{
        //    var ret = Process.GetCurrentProcess().Threads.OfType<Thread>();
        //    if (filter != null)
        //        ret = ret.Where(filter);
        //    return ret.ToArray();
        //}
    }
}
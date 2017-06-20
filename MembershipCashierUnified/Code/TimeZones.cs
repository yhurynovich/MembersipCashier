using MembershipCashierUnified.EmbeddedResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MembershipCashierUnified.Code
{
    public class TimeZones
    {
        #region Static Constructor

        static TimeZones()
        {
            var timezoneXML = EmbeddedResourceProvider.GetResource(EmbeddedResourceProvider.ResourceTypeOptions.TimeZonesXML);
            _timeZonesXML = XDocument.Parse(timezoneXML);
        }

        #endregion

        #region Properties & Fields

        private static XDocument _timeZonesXML;
        private static Dictionary<string, string> allTimeZones;

        #endregion

        #region TimeZones

        /// <summary>
        /// Gets time zones from resources.
        /// </summary>
        /// <returns>Time Zone Key and Time Zone Details</returns>
        public static Dictionary<string, string> GetTimeZones()
        {
            if (allTimeZones == null)
            {
                allTimeZones = _timeZonesXML.Root.Elements("timezone").Select(s => new
                {
                    Key = s.Attribute("name").Value,
                    Value = $"{s.Element("value").Value} {s.Element("comment").Value}"
                }).ToDictionary(x => x.Key, x => x.Value);
            }

            return allTimeZones;
        }

        /// <summary>
        /// Given a time zone code, it returns time zone ISO full name.
        /// </summary>
        /// <param name="code">Time zone code from resource.</param>
        /// <returns>Time Zone Full ISO Name</returns>
        public static string GetTimeZone(string code)
        {
            var timezoneXML = _timeZonesXML.Root.Elements("timezone").First(s => s.Attribute("name").Value == code);
            return timezoneXML.Element("value").Value;
        }

        /// <summary>
        /// Given a time zone code, it returns time zone offset based on time of year
        /// example: North American daylight savings.
        /// </summary>
        /// <param name="code">Time Zone Code</param>
        /// <returns>Time Zone Offset</returns>
        public static TimeSpan GetTimeZoneOffset(string code)
        {
            return TimeZoneInfo.FindSystemTimeZoneById(TimeZones.GetTimeZone(code)).BaseUtcOffset;
        }

        #endregion
    }
}

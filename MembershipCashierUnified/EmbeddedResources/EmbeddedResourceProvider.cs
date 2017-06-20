using System.IO;
using System.Xml;
using System.Xml.Xsl;

namespace MembershipCashierUnified.EmbeddedResources
{
    public static class EmbeddedResourceProvider
    {
        private static string sSCodeXML_Denormalized;

        public enum ResourceTypeOptions
        {
            CountryProvinceCityXML,
            TimeZonesXML
        }

        public static string GetResource(ResourceTypeOptions resourceType)
        {
            switch (resourceType)
            {
                case ResourceTypeOptions.CountryProvinceCityXML:
                    return GetResourceTextFile("CountryProvinceCity.xml");
                case ResourceTypeOptions.TimeZonesXML:
                    return GetResourceTextFile("TimeZones.xml");
                default:
                    return string.Empty;
            }
        }

        public static Stream GetResourceStream(ResourceTypeOptions resourceType)
        {
            switch (resourceType)
            {
                case ResourceTypeOptions.CountryProvinceCityXML:
                    return GetResourceStream("CountryProvinceCity.xml");
                default:
                    return null;
            }
        }

        internal static string GetResourceTextFile(string filename)
        {
            string result = string.Empty;

            using (Stream stream = GetResourceStream(filename))
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    result = sr.ReadToEnd();
                }
            }
            return result;
        }

        internal static Stream GetResourceStream(string filename)
        {
            return typeof(ResourceTypeOptions).Assembly.GetManifestResourceStream(string.Concat("MembershipCashierUnified.", filename));
        }
    }
}

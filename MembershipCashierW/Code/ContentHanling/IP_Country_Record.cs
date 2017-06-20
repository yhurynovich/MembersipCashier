using LINQtoCSV;
using System.Net;
using System.Linq;
using FedExIScanWeb.Code.IP;

namespace MembershipCashierW.ContentHanling.Code.ContentHanling
{
    public class IP_Country_Record
    {
        [CsvColumn(Name = "IP_From", FieldIndex = 1)]
        public string IP_From { get; set; }
        [CsvColumn(Name = "IP_To", FieldIndex = 2)]
        public string IP_To { get; set; }
        [CsvColumn(Name = "CountryCode", FieldIndex = 3)]
        public string CountryCode { get; set; }

        public IP_Address IPFrom
        {
            get
            {
                return new IP_Address(IP_From);
            }
        }

        public IP_Address IPTo
        {
            get
            {
                return new IP_Address(IP_To);
            }
        }
    }
}
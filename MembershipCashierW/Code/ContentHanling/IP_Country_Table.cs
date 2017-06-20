using FedExIScanWeb.ContentHanling.Code.ContentHanling;
using LINQtoCSV;
using System.Collections.Generic;
using System.Web.Hosting;

namespace MembershipCashierW.Code.ContentHanling.ContentHanling
{
    public class IP_Country_Table : CsvContext
    {
        private IEnumerable<IP_Country_Record> records;

        public IEnumerable<IP_Country_Record> Records
        {
            get {
                if (records == null)
                {
                    CsvFileDescription inputFileDescription = new CsvFileDescription
                    {
                        SeparatorChar = ',',
                        FirstLineHasColumnNames = false,
                        EnforceCsvColumnAttribute = true
                    };

                    records = base.Read<IP_Country_Record>(HostingEnvironment.MapPath("~/Content/dbip-country.csv"), inputFileDescription);
                }

                return records;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace MembershipCashierW.Code.UrlRouting
{
    public class Routs
    {
        private ApplicationOptions application = ApplicationOptions.MembershipCashierW;

        public enum ApplicationOptions : byte
        {
            MembershipCashierW
        }

        public ApplicationOptions PrefferedApplication
        {
            get { return application; }
            set { application = value; }
        }

        private IEnumerable<KeyValuePair<string, Uri>> EntryPages
        {
            get
            {
                IEnumerable<KeyValuePair<string, Uri>> ret = new AppSpecificRotes.MembershipCashierWRouts();
                return ret;
            }
        }

        public Uri GetEntryPage(string key)
        {
            var ret = EntryPages.FirstOrDefault(e=>e.Key==key);
            if (ret.Value == null)
                return new AppSpecificRotes.MembershipCashierWRouts()[key];
            return ret.Value;
        }
    }
}
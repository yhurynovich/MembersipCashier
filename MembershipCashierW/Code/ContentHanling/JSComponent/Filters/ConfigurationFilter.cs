using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MembershipCashierW.Code.ContentHanling.JSComponent.Filters
{
    internal class ConfigurationFilter : IJSComponentFilter
    {
        private static ConfigurationFilter instance;

        internal static IJSComponentFilter Instance
        {
            get { return instance; }
        }

        public Func<JsFileDesc, bool> Filter
        {
            get
            {
                return delegate { return true; }; //TODO:
            }
        }

        static ConfigurationFilter()
        {
            lock (typeof(ConfigurationFilter))
            {
                instance = new ConfigurationFilter();
            }
        }
    }
}
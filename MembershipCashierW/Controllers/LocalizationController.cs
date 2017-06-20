using MembershipCashierW.App_Start;
using MembershipCashierW.Code.Captcha;
using MembershipCashierW.Code;
using MembershipCashierW.Controllers.ControllerBase;
using MembershipCashierW.Models;
using SecurityUnified.Contracts;
using SecurityUnified.Enumerations;
using System;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using WebMatrix.WebData;
using System.Collections.Generic;
using System.Resources;
using System.Globalization;
using System.Collections;
using System.Web.Http.Description;

namespace MembershipCashierW.Controllers
{
    public class LocalizationController : WebApiControllerBase
    {
        private static LocalizationController instance;

        public static LocalizationController Instance
        {
            get {
                if (LocalizationController.instance == null)
                    LocalizationController.instance = new LocalizationController();
                return LocalizationController.instance; 
            }
            private set { LocalizationController.instance = value; }
        }

        IEnumerable<ResourceManager> concatinatedResources;

        public IEnumerable<ResourceManager> ConcatinatedResources
        {
            get {
                if (concatinatedResources == null)
                {
                    concatinatedResources = new ResourceManager[] { 
                        Errors.ResourceManager
                    };
                }
                return concatinatedResources; }
        }

        [AllowAnonymous]
        [Route("localize")]
        [HttpGet]
        public IEnumerable<KeyValuePair<string, string>> Localize(string key, string type = null, string culture = null)
        {
            CultureInfo resultCulture;
            if(string.IsNullOrWhiteSpace(culture))
                resultCulture = CultureInfo.InvariantCulture;
            else
                resultCulture = new CultureInfo(culture);

            return Localize(key, type, resultCulture);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IEnumerable<KeyValuePair<string, string>> Localize(string key, string type = null, CultureInfo resultCulture = null)
        {
            var resources = ConcatinatedResources;
            if (type != null)
                resources = resources.Where(r => r.BaseName == type);

            if (resultCulture == null)
                resultCulture = CultureInfo.InvariantCulture;

            if (string.IsNullOrEmpty(key))
            {
                var dictionaries = resources.Select(r => r.GetResourceSet(resultCulture, true, true).OfType<DictionaryEntry>());
                return dictionaries.SelectMany(c => c.Where(s => !string.IsNullOrWhiteSpace(s.Value as string)).Select(x => new KeyValuePair<string, string>(x.Key as string, x.Value as string)));
            }
            else
                return resources.Select(r => new KeyValuePair<string, string>(key, r.GetString(key, resultCulture))).Where(s => !string.IsNullOrWhiteSpace(s.Value));
        }

        [AllowAnonymous]
        [Route("localize")]
        [HttpGet]
        public IEnumerable<KeyValuePair<string, string>> Localize(string txt, string culture = null, bool ignoreCase = true)
        {
            CultureInfo resultCulture;
            if(string.IsNullOrWhiteSpace(culture) || culture == "null")
                resultCulture = CultureInfo.InvariantCulture;
            else
                resultCulture = new CultureInfo(culture);

            return Localize(txt, resultCulture);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IEnumerable<KeyValuePair<string, string>> Localize(string txt, CultureInfo resultCulture)
        {
            List<KeyValuePair<string, string>> ret = new List<KeyValuePair<string, string>>();

            foreach (var source in ConcatinatedResources)
            {
                var rs = source.GetResourceSet(CultureInfo.InvariantCulture, true, true);
                foreach (var entry in rs)
                {
                    var e = (DictionaryEntry)entry;
                    if (e.Value as string == txt)
                        ret.Add(new KeyValuePair<string, string>(e.Key as string, source.GetString(e.Key as string, resultCulture)));
                }
            }

            return ret;
        }

        public LocalizationController()
        {
            LocalizationController instance = this;
        }
    }
}
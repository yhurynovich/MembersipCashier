using MembershipCashierUnified.Contracts;
using MembershipCashierUnified.EmbeddedResources;
using MembershipCashierUnified.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml.Linq;

namespace MembershipCashierUnified
{
    public static class Extentions_Validate
    {
        private static Regex poBoxRegex = new Regex(Constants.PO_BOX_REGEX, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex languageRegex = new Regex(Constants.LANGUAGE_ISO2_REGEX, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static ICollection<ValidationResult> Validate(this IIAddress address, bool strict = true)
        {
            if (strict)
                return ValidateDataAnnotations(address);
            else
            {
                return ValidateMinimalNeeded(address);
            }
        }

        public static ICollection<ValidationResult> ValidateDataAnnotations(this IIAddress address)
        {
            var currentThread = Thread.CurrentThread;
            var currentUiCulture = currentThread.CurrentUICulture;
            try
            {
                string addressCulture = CountryCodeToCulture(address.Country);

                if (addressCulture != null && currentUiCulture.IetfLanguageTag != addressCulture)
                    currentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(addressCulture);

                AddressImplementor a;
                if (typeof(AddressImplementor).IsInstanceOfType(address))
                    a = (AddressImplementor)address;
                else
                {
                    a = new AddressImplementor();
                    address.CopyTo(a);
                }

                var ret = new List<ValidationResult>();
                var vc = new ValidationContext(a, null, null);

                Validator.TryValidateObject(a, vc, ret, true);

                return ret;
            }
            finally
            {
                currentThread.CurrentUICulture = currentUiCulture;
            }
        }

        public static ICollection<ValidationResult> ValidateMinimalNeeded(this IIAddress address)
        {
            var currentThread = Thread.CurrentThread;
            var currentUiCulture = currentThread.CurrentUICulture;
            try
            {
                string addressCulture = CountryCodeToCulture(address.Country);

                if (addressCulture != null && currentUiCulture.IetfLanguageTag != addressCulture)
                    currentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(addressCulture);

                AddressImplementor a;
                if (typeof(AddressImplementor).IsInstanceOfType(address))
                    a = (AddressImplementor)address;
                else
                {
                    a = new AddressImplementor();
                    address.CopyTo(a);
                }

                //Get flags indicating if we need to validate specific fields
                XDocument countries = XDocument.Load(
                        EmbeddedResourceProvider.GetResourceStream(EmbeddedResourceProvider.ResourceTypeOptions.CountryProvinceCityXML)
                );

                bool require_postal_code = true;
                bool require_city = true;
                var countryNode = countries.Root.Elements("country").FirstOrDefault(c => c.Attribute("code").Value == address.Country);
                if (countryNode != null)
                {
                    require_postal_code = countryNode.Attribute("require_postal_code") == null ? require_postal_code : Convert.ToBoolean(countryNode.Attribute("require_postal_code").Value);
                    require_city = countryNode.Attribute("require_city") == null ? require_city : Convert.ToBoolean(countryNode.Attribute("require_city").Value);
                }

                var ret = new List<ValidationResult>();
                if (require_postal_code && string.IsNullOrWhiteSpace(address.PostalCode))
                {
                    if (require_city && string.IsNullOrWhiteSpace(address.City))
                        ret.Add(new ValidationResult("Expected City or Postal Code", new string[] { "PostalCode", "City" }));
                    if (typeof(IAddress).IsInstanceOfType(address))
                    {
                        var address2 = (IAddress)address;
                        if (string.IsNullOrWhiteSpace(address2.Province) && string.IsNullOrWhiteSpace(address2.ProvinceName))
                            ret.Add(new ValidationResult("Expected Province Code, Province Name or Postal Code", new string[] { "PostalCode", "Province" }));
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(address.Province))
                            ret.Add(new ValidationResult("Expected Province or Postal Code", new string[] { "PostalCode", "Province" }));
                    }
                }

                var vc = new ValidationContext(a, null, null);
                vc.MemberName = "Country";
                Validator.TryValidateProperty(address.Country, vc, ret);

                if (!string.IsNullOrWhiteSpace(address.Address1))
                {
                    vc.MemberName = "Address1";
                    Validator.TryValidateProperty(address.Address1, vc, ret);
                }

                if (require_postal_code && !string.IsNullOrWhiteSpace(address.PostalCode))
                {
                    vc.MemberName = "PostalCode";
                    Validator.TryValidateProperty(address.PostalCode, vc, ret);
                }

                if (require_city && !string.IsNullOrWhiteSpace(address.City))
                {
                    vc.MemberName = "City";
                    Validator.TryValidateProperty(address.City, vc, ret);
                }

                if (!string.IsNullOrWhiteSpace(address.Province))
                {
                    vc.MemberName = "Province";
                    Validator.TryValidateProperty(address.Province, vc, ret);
                }

                return ret;
            }
            finally
            {
                currentThread.CurrentUICulture = currentUiCulture;
            }
        }

        public static bool IsEmpty(this IIAddress address)
        {
            return string.IsNullOrWhiteSpace(address.Address1)
                && string.IsNullOrWhiteSpace(address.PostalCode)
                && string.IsNullOrWhiteSpace(address.City)
                && string.IsNullOrWhiteSpace(address.Province)
                && string.IsNullOrWhiteSpace(address.Country);
        }

        public static void Trim(this IIAddress address)
        {
            if (address.City != null)
                address.City = address.City.Trim();
            if (address.Country != null)
                address.Country = address.Country.Trim();
            if (address.Address1 != null)
                address.Address1 = address.Address1.Trim();
            if (address.PostalCode != null)
                address.PostalCode = Regex.Replace(address.PostalCode, @"\s*", "");
            if (address.Province != null)
                address.Province = address.Province.Trim();
        }

        public static Exception ToException(this IEnumerable<ValidationResult> validation, string message = null)
        {
            var ret = new AggregateException(message);
            foreach (var v in validation)
            {
                ret.Data.Add(string.Join(",", v.MemberNames), v.ErrorMessage);
            }
            return ret;
        }

        public static string CountryCodeToCulture(string country)
        {
            switch (country)
            {
                case "US":
                case "MX":
                    return "en-US";
                case "CA":
                    return "en-CA";
                case "GB":
                case "UK":
                    return "en-GB";
                case "AU":
                    return "en-AU";
                case "AT":
                    return "de-AT";
                case "BE":
                    return "nl-BE";
                case "BR":
                    return "pt-BR";
                case "BY":
                    return "be-BY";
                case "CN":
                    return "zh-CN";
                case "CO":
                    return "es-CO";
                case "CZ":
                    return "cs-CZ";
                case "DK":
                    return "da-DK";
                case "FI":
                    return "fi-FI";
                case "FR":
                    return "fr-FR";
                case "DE":
                    return "de-DE";
                case "GR":
                    return "el-GR";
                case "HU":
                    return "hu-HU";
                case "IN":
                    return "pa-IN";
                case "ID":
                    return "id-ID";
                case "IT":
                    return "it-IT";
                case "JP":
                    return "ja-JP";
                case "KR":
                    return "ko-KR";
                case "LU":
                    return "de-LU";
                case "MY":
                    return "ms-MY";
                case "NL":
                    return "nl-NL";
                case "NO":
                    return "nn-NO";
                case "PH":
                    return "en-PH";
                case "PL":
                    return "pl-PL";
                case "PT":
                    return "pt-PT";
                case "RU":
                    return "ru-RU";
                case "SG":
                    return "zh-SG";
                case "ZA":
                    return "en-ZA";
                case "ES":
                    return "es-ES";
                case "SE":
                    return "sv-SE";
                case "CH":
                    return "de-CH";
                case "TH":
                    return "th-TH";
                case "TR":
                    return "tr-TR";
                default:
                    return string.Empty;
            }
        }

        //public static ValidationResult ValidateEmail(this IHasEmail obj)
        //{
        //    if (obj == null || string.IsNullOrEmpty(obj.Email))
        //        return null;

        //    var re = new Regex(Constants.EMAIL_VALIDATION_EXPRESSION, RegexOptions.Compiled);
        //    if (!re.IsMatch(obj.Email))
        //    {
        //        return new ValidationResult(Resources.InvalidEmail, new string[1] { "Email" });
        //    }

        //    return null;
        //}

        //public static ICollection<ValidationResult> ValidateParty(this IParty obj)
        //{
        //    var res = new Collection<ValidationResult>();
        //    var emailValidation = obj.ValidateEmail();
        //    if (emailValidation != null)
        //        res.Add(emailValidation);

        //    if (obj.FirstName != null && obj.FirstName.Length > 50)
        //        res.Add(new ValidationResult("FirstName can not exceed 50 charachters", new string[1] { "FirstName" }));
        //    if (obj.LastName != null && obj.LastName.Length > 50)
        //        res.Add(new ValidationResult("LastName can not exceed 50 charachters", new string[1] { "FirstName" }));
        //    if (obj.CompanyName != null && obj.CompanyName.Length > 50)
        //        res.Add(new ValidationResult("CompanyName can not exceed 50 charachters", new string[1] { "FirstName" }));
        //    if (obj.Phone != null && obj.Phone.Length > 20)
        //        res.Add(new ValidationResult("Phone can not exceed 20 charachters", new string[1] { "FirstName" }));
        //    if (!string.IsNullOrEmpty(obj.DeliveryInstruction) && poBoxRegex.Match(obj.DeliveryInstruction).Success) //Regex is thread safe
        //        res.Add(new ValidationResult(Resources.FedEx_does_not_deliver_to_PO_boxes, new string[1] { "FirstName" }));
        //    if (obj.Language != null && !languageRegex.Match(obj.Language).Success) //Regex is thread safe
        //        res.Add(new ValidationResult(Resources.InvalidLanguageCode, new string[1] { "Language" }));

        //    return res;
        //}
    }
}

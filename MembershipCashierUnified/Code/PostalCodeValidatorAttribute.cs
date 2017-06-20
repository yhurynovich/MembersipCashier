using SecurityUnified;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MembershipCashierUnified.Code
{
    public class PostalCodeValidatorAttribute : ValidationAttribute
    {
        private Regex Regex { get; set; }

        // Summary:
        //     Gets the regular expression pattern.
        //
        // Returns:
        //     The pattern to match.
        public string Pattern
        {
            get
            {
                if (Regex == null)
                    return null;

                return Regex.ToString();
            }
            set
            {
                SetupRegex(pattern: value);
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, base.ErrorMessageString, new object[] { name, this.Pattern });
        }

        public override bool IsValid(object value)
        {
            string str = Convert.ToString(value, CultureInfo.CurrentCulture);
            if (string.IsNullOrEmpty(str))
            {
                return true;
            }
            SetupRegexByCountry();
            Match match = this.Regex.Match(str);
            return ((match.Success && (match.Index == 0)) && (match.Length == str.Length));
        }

        private void SetupRegex(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                throw new InvalidOperationException("The pattern must be set to a valid regular expression.");
            }
            this.Regex = new Regex(pattern);
        }

        private void SetupRegexByCountry(string country = null)
        {
            if (string.IsNullOrEmpty(country))
                country = CultureInfo.CurrentUICulture.Name;

            switch (country)
            {
                case "en-CA":
                case "fr-CA":
                    Pattern = Constants.POSTAL_CODE_CA;
                    break;
                case "en-GB":
                    Pattern = Constants.POSTAL_CODE_UK;
                    break;
                case "en-US":
                case "es-MX":
                    Pattern = Constants.POSTAL_CODE_USA;
                    break;
                case "en-AU":
                case "de-AT":
                case "nl-BE":
                case "da-DK":
                case "hu-HU":
                case "hu":
                case "fr-LU":
                case "de-LU":
                case "nl-NL":
                case "nb-NO":
                case "nn-NO":
                case "en-PH":
                case "pt-PT":
                case "en-ZA":
                case "af-ZA":
                case "fr-CH":
                case "de-CH":
                case "it-CH":
                    Pattern = Constants.POSTAL_CODE_AU;
                    break;
                case "pt-BR":
                    Pattern = Constants.POSTAL_CODE_BR;
                    break;
                case "zh":
                case "zh-CN":
                case "es-CO":
                case "gu-IN":
                case "hi-IN":
                case "kn-IN":
                case "kok-IN":
                case "mr-IN":
                case "pa-IN":
                case "sa-IN":
                case "ta-IN":
                case "te-IN":
                case "ru":
                case "ru-RU":
                case "zh-SG":
                case "be":
                case "be-BY":
                    Pattern = Constants.POSTAL_CODE_CN;
                    break;
                case "cs":
                case "cs-CZ":
                case "fi-FI":
                case "fr-FR":
                case "de-DE":
                case "el-GR":
                case "el":
                case "id":
                case "id-ID":
                case "it":
                case "it-IT":
                case "ms-MY":
                case "es-PR":
                case "es-ES":
                case "sv-SE":
                case "th-TH":
                case "tr":
                case "tr-TR":
                case "ko":
                case "ko-KR":
                    Pattern = Constants.POSTAL_CODE_CS;
                    break;
                case "ja":
                case "ja-JP":
                    Pattern = Constants.POSTAL_CODE_JP;
                    break;
                default:
                    Pattern = Constants.POSTAL_CODE_FREEFORM;
                    break;
            }
        }

        public PostalCodeValidatorAttribute()
        {
            SetupRegexByCountry();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MembershipCashierW.Code.BeardTemplate.Code
{
    public class TextSlice : Slice
    {
        static Regex placeHolderRegEx = new Regex(@"{{(\w+)}}");

        public override string Render()
        {
            string renderResult = sourceStr;
            Match m = placeHolderRegEx.Match(renderResult);
            if (m.Success)
            {
                var values = LocalContext;
                while (m.Success)
                {
                    string nm = m.Groups[1].Value;
                    if (values.ContainsKey(nm))
                        renderResult = renderResult.Replace(string.Concat("{{", nm, "}}"), Convert.ToString(values[nm]));

                    m = m.NextMatch();
                }
            }

            return renderResult;
        }

        public TextSlice(IDictionary<string, object> context, string sourceStr, Slice parent = null) : base(context, sourceStr, parent)
        {
        }

    }
}
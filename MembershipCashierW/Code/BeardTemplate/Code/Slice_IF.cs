using System;
using System.Collections.Generic;
using System.Linq;

namespace MembershipCashierW.Code.BeardTemplate.Code
{
    public class Slice_IF : OperationSlice
    {
        public override Slice[] GetResultSlices()
        {
            var value = Convert.ToString(Context[key]);

            if (string.IsNullOrEmpty(value) || value == "False" || value == "0")
                return new Slice[0] { };

            return base.Slices == null ? new Slice[0] { } : base.Slices.ToArray();
        }

        public override string Render()
        {
            return string.Join("\n", GetResultSlices().Select(x => x.Render()));
        }

        public Slice_IF(IDictionary<string, object> context, string key, string sourceStr, Slice parent = null) : base(context, key, sourceStr, parent)
        {
        }
    }
}
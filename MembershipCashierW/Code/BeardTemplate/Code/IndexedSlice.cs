using System.Collections.Generic;
using System.Linq;

namespace MembershipCashierW.Code.BeardTemplate.Code
{
    public class IndexedSlice : OperationSlice
    {
        int index;

        public override IDictionary<string, object> LocalContext
        {
            get
            {
                var ret = new Dictionary<string, object>(base.Context);
                ret.Add("this", (Context[key] as IEnumerable<object>).Skip(index).FirstOrDefault());
                return ret;
            }
        }

        public override Slice[] GetResultSlices()
        {
            var values = LocalContext;
            var ret = base.Slices.ToArray();
            foreach (var slice in ret)
                slice.Context = values;

            return ret;
        }

        public override string Render()
        {
            return string.Join("\n", GetResultSlices().Select(x => x.Render()));
        }

        public IndexedSlice(IDictionary<string, object> context, string key, int index, string sourceStr, Slice parent) : base(context, key, sourceStr, parent)
        {
            this.index = index;
        }
    }
}
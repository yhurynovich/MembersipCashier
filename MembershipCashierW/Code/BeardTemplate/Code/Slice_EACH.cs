using System.Collections.Generic;
using System.Linq;

namespace MembershipCashierW.Code.BeardTemplate.Code
{
    public class Slice_EACH : OperationSlice
    {
        public override Slice[] GetResultSlices()
        {
            if (Context.Keys.Contains(key))
            {
                var operand = Context[key] as IEnumerable<object>;

                var cnt = operand.Count();
                var res = new Slice[cnt];
                for (int i = 0; i < cnt; i++)
                {
                    res[i] = new IndexedSlice(Context, key, i, sourceStr, this);
                }
                return res;
            }
            else
            {
                Utils.WriteToEventLog(string.Format("Beard.Slice_EACH did not find key named {0}", key), Enumerations.ModelMessageTypeOptions.ERROR);
                return new Slice[0] { };
            }
        }

        public override string Render()
        {
            return string.Join("\n", GetResultSlices().Select(x => x.Render()));
        }

        public Slice_EACH(IDictionary<string, object> context, string key, string sourceStr, Slice parent = null) : base(context, key, sourceStr, parent)
        {
        }
    }
}
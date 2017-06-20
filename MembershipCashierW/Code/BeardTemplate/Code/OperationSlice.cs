using System.Collections.Generic;

namespace MembershipCashierW.Code.BeardTemplate.Code
{
    public abstract class OperationSlice : Slice
    {
        Slicer slicer;
        IEnumerable<Slice> slices;

        public Slicer Cutter
        {
            get
            {
                if (slicer == null)
                    slicer = new Slicer(Context, base.sourceStr);

                return slicer;
            }
        }

        public IEnumerable<Slice> Slices
        {
            get
            {
                if (slices == null)
                    slices = Cutter.Cut(base.sourceStr);
                return slices;
            }
        }

        public abstract Slice[] GetResultSlices();

        public OperationSlice(IDictionary<string, object> context, string key, string sourceStr, Slice parent = null) : base(context, sourceStr, parent)
        {
            this.key = key;
        }
    }
}
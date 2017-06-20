using MembershipCashierW.Code.BeardTemplate.Code;
using System.Collections.Generic;
using System.Linq;

namespace MembershipCashierW.Code.BeardTemplate
{
    public class Beard
    {
        string template;
        IDictionary<string, object> values;
        Slicer slicer;
        IEnumerable<Slice> slices;

        public Slicer Cutter
        {
            get
            {
                if (slicer == null)
                    slicer = new Slicer(values, template);

                return slicer;
            }
        }

        public IEnumerable<Slice> Slices
        {
            get
            {
                if (slices == null)
                    slices = Cutter.Cut(template);
                return slices;
            }
        }

        public string Render()
        {
            return string.Join("\n", Slices.Select(x => x.Render()));
        }

        public IDictionary<string, object> Values
        {
            get
            {
                return values;
            }
            set
            {
                values = value;
            }
        }

        public Beard(string template)
        {
            this.template = template;
        }

        public Beard(string template, IDictionary<string, object> values) : this(template)
        {
            this.Values = values;
        }
    }
}
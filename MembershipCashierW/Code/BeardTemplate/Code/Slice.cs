using System.Collections.Generic;

namespace MembershipCashierW.Code.BeardTemplate.Code
{
    public abstract class Slice
    {
        protected string key;
        private IDictionary<string, object> context;
        private IDictionary<string, object> localContext;
        protected string sourceStr;
        Slice parent;

        public virtual IDictionary<string, object> LocalContext
        {
            get
            {
                if (localContext == null)
                {
                    if (string.IsNullOrEmpty(key))
                        localContext = Context;
                    else
                    {
                        if (Context.ContainsKey(key))
                        {
                            var ret = new Dictionary<string, object>(Context);
                            ret.Add("this", Context[key]);
                            localContext = ret;
                        }
                    }

                    localContext = Context;
                }

                return localContext;
            }
        }

        public Slice Parent
        {
            get
            {
                return parent;
            }
        }

        public IDictionary<string, object> Context
        {
            get
            {
                return context;
            }
            set
            {
                context = value;
                localContext = null;
            }
        }

        public abstract string Render();

        public Slice(IDictionary<string, object> context, string sourceStr, Slice parent = null)
        {
            this.Context = context;
            this.sourceStr = sourceStr;
            this.parent = parent;
        }
    }
}
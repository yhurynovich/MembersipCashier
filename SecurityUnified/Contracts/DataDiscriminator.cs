using Serialize.Linq.Extensions;
using Serialize.Linq.Nodes;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace SecurityUnified.Contracts
{
    [DataContract]
    public class DataDiscriminator<T>
    {
        [DataMember]
        int take;
        [DataMember]
        ExpressionNode n_filter;

        public Expression<Func<T, bool>> Filter
        {
            get {
                if (n_filter == null)
                    n_filter = DefaultValue.ToExpressionNode();
                return n_filter.ToBooleanExpression<T>(); }
            set { n_filter = value.ToExpressionNode(); }
        }

        protected virtual Expression<Func<T, bool>> DefaultValue
        {
            get { return Expression.Lambda<Func<T, bool>>(Expression.Constant(true), Expression.Parameter(typeof(T))); }
        }

        [DataMember]
        public IEnumerable<SortByFld> OrderBy { get; set; }
        [DataMember]
        public int Skip { get; set; }
        [DataMember]
        public int Take { get { if (take > 0) return take; else return 100; } set { take = value; } }
    }
}

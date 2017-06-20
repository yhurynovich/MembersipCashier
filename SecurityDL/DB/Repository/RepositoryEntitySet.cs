using SecurityDL.Properties;
using SecurityUnified.Contracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;

namespace SecurityDL.DB.Repository
{
    public class RepositoryEntitySet<T, I> : DataContext, IQueryable<I>, IEnumerable<I>, IDisposable where T : class, I
    {
        public const int DEFAULT_MAX_RECORDS = 100;
        protected IQueryable<I> result;
        protected DataDiscriminator<I> discriminator;

        public virtual IQueryable<I> Result
        {
            get {
                if (result == null)
                {
                    result = GetTable<T>();

                    if (discriminator != null)
                    {
                        if (discriminator.Filter != null)
                            result = result.Where(discriminator.Filter);

                        if (discriminator.OrderBy != null)
                        {
                            foreach (var oby in discriminator.OrderBy)
                            {
                                result = OrderByHelper(result, oby.FieldName, oby.IsDescending);
                            }
                        }

                        result = result.Skip(discriminator.Skip).Take(discriminator.Take);
                    }
                    else
                        result = result.Take(DEFAULT_MAX_RECORDS);
                }
                return result;
            }
        }

        private RepositoryQueryProvider<T, I> qp;

        public IEnumerator<I> GetEnumerator()
        {
            return Result.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Result.GetEnumerator();
        }

        public IQueryable<I> Skip(int count)
        {
            result = Result.Skip(count);
            return result;
        }

        public IQueryable<I> Take(int count)
        {
            result = Result.Take(count);
            return result;
        }

        public int Count()
        {
            return Result.Count();
        }

        public IQueryable<I> OrderByDescending<TKey>(Expression<Func<I, TKey>> keySelector)
        {
            result = Result.OrderByDescending(keySelector);
            return result;
        }

        public IQueryable<I> OrderBy<TKey>(Expression<Func<I, TKey>> keySelector)
        {
            result = Result.OrderBy(keySelector);
            return result;
        }

        public IQueryable<TResult> OfType<TResult>() where TResult : I
        {
            return Result.OfType<TResult>();
        }

        public I First()
        {
            return Result.First();
        }

        public I FirstOrDefault()
        {
            return Result.FirstOrDefault();
        }

        public IQueryable<TResult> Select<TResult>(Expression<Func<I, TResult>> selector)
        {
            return Result.Select(selector);
        }

        public Type ElementType
        {
            get { return typeof(T); }
        }

        public virtual DataDiscriminator<I> Discriminator { get { return discriminator; } set { discriminator = value; result = null; } }

        Expression IQueryable.Expression
        {
            get { return Discriminator.Filter; }
        }

        public IQueryProvider Provider
        {
            get {
                if (qp == null)
                    qp = new RepositoryQueryProvider<T, I>(this);
                return qp;
            }
        }

        public IQueryable<T> OrderByHelper(IQueryable<T> source, string memberName, bool isDescending = false)
        {
            var type = typeof(T);
            var property = type.GetProperty(memberName);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            MethodCallExpression resultExp;
            if(isDescending)
                resultExp = Expression.Call(typeof(Queryable), "OrderByDescending", new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));
            else
                resultExp = Expression.Call(typeof(Queryable), "OrderBy", new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));
            return source.Provider.CreateQuery<T>(resultExp);
        }

        public IQueryable<I> OrderByHelper(IQueryable<I> source, string memberName, bool isDescending = false)
        {
            return OrderByHelper(source.OfType<T>(), memberName, isDescending);
            //var type = typeof(T);
            //var property = type.GetProperty(memberName);
            //var parameter = Expression.Parameter(type, "p");
            //var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            //var orderByExp = Expression.Lambda(propertyAccess, parameter);
            //MethodCallExpression resultExp;
            //if (isDescending)
            //    resultExp = Expression.Call(typeof(IQueryable<I>), "OrderByDescending", new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));
            //else
            //    resultExp = Expression.Call(typeof(IQueryable<I>), "OrderBy", new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));
            //return source.Provider.CreateQuery<T>(resultExp);
        }

        public new void Dispose()
        {
            qp = null;
            result = null;
            discriminator = null;

            base.Dispose();
        }

        public RepositoryEntitySet()
            : base(Settings.Default.MembershipCashierConnectionString)
        {
        }

        public RepositoryEntitySet(RepositoryQueryProvider<T, I> queryProvider)
            : this()
        {
            qp = queryProvider;
        }

        public RepositoryEntitySet(DataDiscriminator<I> filter, RepositoryQueryProvider<T, I> queryProvider)
            : this(queryProvider)
        {
            Discriminator = filter;
        }
    }
}

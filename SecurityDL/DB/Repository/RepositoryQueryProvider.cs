using System;
using System.Linq;
using System.Linq.Expressions;

namespace SecurityDL.DB.Repository
{
    public class RepositoryQueryProvider<T, I> : IQueryProvider where T : class, I
    {
        private RepositoryEntitySet<T, I> source;

        internal RepositoryEntitySet<T, I> Source
        {
            get {
                if (source == null)
                    source = new RepositoryEntitySet<T, I>(this);
                return source;
            }
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable CreateQuery(Expression expression)
        {
            try
            {
                //return (IQueryable)Activator.CreateInstance(typeof(RepositoryQueryProvider<,>).MakeGenericType(expression.Type), new object[] { this, expression });
                return Source.Where((Expression<Func<I, bool>>)expression);
            }
            catch (System.Reflection.TargetInvocationException tie)
            {
                throw tie.InnerException;
            }
        }

        public TResult Execute<TResult>(Expression expression)
        {
            throw new NotImplementedException();
        }

        public object Execute(Expression expression)
        {
            throw new NotImplementedException();
        }

        public RepositoryQueryProvider()
        { 
        }

        public RepositoryQueryProvider(RepositoryEntitySet<T, I> source)
        {
            this.source = source;
        }
    }
}


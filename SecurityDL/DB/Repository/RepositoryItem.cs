using System;

namespace SecurityDL.DB.Repository
{
    internal class RepositoryItem<T, I> : IDisposable where T : class, I
    {
        private RepositoryEntitySet<T, I> entitySet;
        private DateTime _lastUsed;

        public DateTime LastUsed
        {
            get { return _lastUsed; }
        }

        internal RepositoryEntitySet<T, I> EntitySet
        {
            get
            {
                if (entitySet == null)
                    entitySet = new RepositoryEntitySet<T, I>();

                lock (entitySet)
                {
                    _lastUsed = DateTime.UtcNow;
                }
                return entitySet;
            }
        }

        public void Dispose()
        {
            if (entitySet != null)
            {
                var x = entitySet;
                lock (entitySet)
                {
                    entitySet = null;
                }
                x.Dispose();
            }
        }
    }
}

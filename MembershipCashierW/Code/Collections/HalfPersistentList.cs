using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace MembershipCashierW.Code.Collections
{
    [Serializable]
    public class HalfPersistentList<T> : List<T>
    {
        public int PersistentDataLen { get; private set; }

        public int MaxItemsToKeep { get; private set; }

        public new void Add(T item)
        {
            base.Insert(PersistentDataLen, item);
            Trim();
        }

        public new void AddRange(IEnumerable<T> items)
        {
            base.InsertRange(PersistentDataLen, items);
            Trim();
        }

        public void Trim()
        {
            int removeFrom = PersistentDataLen + MaxItemsToKeep;
            int removeCnt = base.Count - removeFrom;
            if (removeCnt > 0)
                base.RemoveRange(removeFrom, removeCnt);
        }

        public HalfPersistentList(IEnumerable<T> persistentData, int maxItemsToKeep = 2)
        {
            this.MaxItemsToKeep = maxItemsToKeep;

            if (persistentData != null)
            {
                PersistentDataLen = persistentData.Count();
                base.AddRange(persistentData);
            }
        }

        public HalfPersistentList(StringCollection persistentData, int maxItemsToKeep = 2)
        {
            this.MaxItemsToKeep = maxItemsToKeep;

            if (persistentData != null)
            {
                PersistentDataLen = persistentData.Count;
                base.AddRange(persistentData.Cast<T>());
            }
        }
    }
}
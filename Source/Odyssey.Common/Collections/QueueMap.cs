using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Odyssey.Text.Logging;

namespace Odyssey.Collections
{
    public class QueueMap<TItem>
    {
        readonly Dictionary<Type, Queue<TItem>> queueMap;

        public QueueMap()
        {
            queueMap = new Dictionary<Type, Queue<TItem>>();
        }

        void Associate(Type messageType)
        {
            if (!queueMap.ContainsKey(messageType))
            {
                queueMap[messageType] = new Queue<TItem>();
            }
            else LogEvent.Engine.Warning("Map is already associated to " + messageType.Name);
        }

        public void Associate<TDerivedItem>()
            where TDerivedItem : TItem
        {
            Associate(typeof(TDerivedItem));
        }

        public void RemoveAssociation<TDerivedItem>()
            where TDerivedItem : TItem
        {
            RemoveAssociation(typeof(TDerivedItem));
        }

        void RemoveAssociation(Type messageType)
        {
            queueMap.Remove(messageType);
        }

        bool IsAssociated(Type messageType)
        {
            return queueMap.ContainsKey(messageType);
        }

        [Pure]
        public bool IsAssociated<TDerivedItem>()
            where TDerivedItem : TItem
        {
            return IsAssociated(typeof(TDerivedItem));
        }

        internal bool HasItems(Type messageType)
        {
            return queueMap.ContainsKey(messageType) && queueMap[messageType].Count > 0;
        }

        public bool HasItems<TDerivedItem>()
            where TDerivedItem : TItem
        {
            return HasItems(typeof(TDerivedItem));
        }

        void Enqueue<TDerivedItem>(Type indexType, TDerivedItem item )
            where TDerivedItem : TItem
        {
            if (!IsAssociated(indexType))
                Associate(indexType);

            queueMap[indexType].Enqueue(item);
        }

        public void Enqueue<TDerivedItem>(TDerivedItem item)
            where TDerivedItem : TItem
        {
            Contract.Requires<ArgumentNullException>(item!=null, "item");
            Type indexType = item.GetType();
            Enqueue(indexType, item);
        }

        TItem Dequeue(Type indexType)
        {
            return queueMap[indexType].Dequeue();
        }

        public TDerivedItem Dequeue<TDerivedItem>()
             where TDerivedItem : TItem
        {
            Type indexType = typeof(TDerivedItem);

            return (TDerivedItem)Dequeue(indexType);
        }

    }
}

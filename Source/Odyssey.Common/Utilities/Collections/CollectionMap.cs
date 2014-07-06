using Odyssey.Utilities.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Odyssey.Utilities.Collections
{
    public class CollectionMap<TIndex, TCollection, TItem>
        where TCollection : ICollection<TItem>, new()
    {
        readonly Dictionary<Type, TCollection> collectionMap;

        public CollectionMap()
        {
            collectionMap = new Dictionary<Type, TCollection>();
        }

        void Associate(Type index)
        {
            if (!collectionMap.ContainsKey(index))
            {
                collectionMap[index] = new TCollection();
            }
            else LogEvent.Engine.Warning("Map is already associated to " + index.Name);
        }

        public void Associate<TDerivedIndex>()
            where TDerivedIndex : TIndex
        {
            Associate(typeof(TDerivedIndex));
        }

        public void RemoveAssociation<TDerivedIndex>()
            where TDerivedIndex : TIndex
        {
            RemoveAssociation(typeof(TDerivedIndex));
        }

        void RemoveAssociation(Type messageType)
        {
            collectionMap.Remove(messageType);
        }

        bool IsAssociated(Type messageType)
        {
            return collectionMap.ContainsKey(messageType);
        }

        public bool IsAssociated<TDerivedIndex>()
            where TDerivedIndex : TIndex
        {
            return IsAssociated(typeof(TDerivedIndex));
        }

        bool IsMapEmpty(Type messageType)
        {
            return collectionMap[messageType].Count == 0;
        }

        [Pure]
        public bool IsRegistered<TDerivedIndex>(TItem item)
            where TDerivedIndex : TIndex
        {
            Type indexType = typeof(TDerivedIndex);

            if (!IsAssociated(indexType))
                return false;
            return collectionMap[indexType].Contains(item);
        }


        public void Add<TDerivedIndex>(TItem item)
            where TDerivedIndex : TIndex
        {
            Contract.Requires<ArgumentException>(!IsRegistered<TDerivedIndex>(item));
            Type indexType = typeof(TDerivedIndex);

            if (!IsAssociated(indexType))
                Associate(indexType);

            collectionMap[indexType].Add(item);
        }

        public void Remove<TDerivedIndex>(TItem item)
             where TDerivedIndex : TIndex
        {
            Contract.Requires<ArgumentException>(IsRegistered<TDerivedIndex>(item));
            Type messageType = typeof(TDerivedIndex);

            collectionMap[messageType].Remove(item);

            if (IsMapEmpty(messageType))
                RemoveAssociation(messageType);
        }

        public IEnumerable<TItem> Select<TDerivedIndex>()
        {
            Type messageType = typeof(TDerivedIndex);
            if (!collectionMap.ContainsKey(messageType))
                return Enumerable.Empty<TItem>();

            var subscribedSystems = collectionMap[messageType];

            if (subscribedSystems.Any())
                return subscribedSystems;
            else
                return Enumerable.Empty<TItem>();
        }

    }
}

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
        readonly Dictionary<TIndex, TCollection> collectionMap;

        public CollectionMap()
        {
            collectionMap = new Dictionary<TIndex, TCollection>();
        }

        public bool ContainsKey(TIndex index)
        {
            return collectionMap.ContainsKey(index);
        }

        public void AddTo(TIndex index, TItem item)
        {
            if (!collectionMap.ContainsKey(index))
                collectionMap.Add(index, new TCollection());
            collectionMap[index].Add(item);
        }

        public void RemoveFrom(TIndex index, TItem item)
        {
            Contract.Requires<ArgumentNullException>(item != null, "item");
            Contract.Requires<ArgumentException>(ContainsKey(index), "index");
            collectionMap[index].Remove(item);
        }

        public IEnumerable<TItem> Select(TIndex index)
        {
            return !ContainsKey(index) ? Enumerable.Empty<TItem>() : collectionMap[index];
        }
    }
}

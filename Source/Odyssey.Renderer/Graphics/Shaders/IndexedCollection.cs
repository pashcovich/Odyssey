using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Odyssey.Graphics.Shaders
{
    public class IndexedCollection<TIndex, TData> : IEnumerable<TData>
    {
        private readonly Dictionary<TIndex, List<TData>> data;

        public IndexedCollection()
        {
            data = new Dictionary<TIndex, List<TData>>();
        }

        public void AddItem(TIndex id, TData item)
        {
            Contract.Requires<ArgumentNullException>(item != null, "item");
            if (!data.ContainsKey(id))
                data.Add(id, new List<TData>());

            data[id].Add(item);
        }

        public bool HasItem(TIndex id)
        {
            return data.ContainsKey(id);
        }

        public IEnumerable<TIndex> Keys
        {
            get { return data.Keys; }
        }

        public IEnumerable<TData> this[TIndex key]
        {
            get { return data[key]; }
        }

        public IEnumerator<TData> GetEnumerator()
        {
            return data.Values.SelectMany(list => list).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

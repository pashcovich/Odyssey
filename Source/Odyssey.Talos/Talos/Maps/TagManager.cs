using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Utilities.Collections;

namespace Odyssey.Talos.Maps
{
    public sealed class TagManager : IEnumerable<string>
    {
        private readonly CollectionMap<long, SortedSet<string>, string> tags;

        public TagManager()
        {
            tags = new CollectionMap<long, SortedSet<string>, string>();
        }

        public void AddTagToEntity(Entity entity, string tag)
        {
            Contract.Requires<ArgumentNullException>(entity != null, "entity");
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(tag), "tag");
            tags.Associate<long>();
        }

        public IEnumerator<string> GetEnumerator()
        {
            return tags.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

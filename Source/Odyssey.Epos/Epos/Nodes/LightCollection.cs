using System.Collections.Generic;
using System.Linq;

namespace Odyssey.Epos.Nodes
{
    public class LightCollection : Dictionary<int, LightNode>, ILightService
    {
        public LightCollection()
        {
        }

        public LightCollection(int capacity)
            : base(capacity)
        {
        }

        public LightCollection(IEnumerable<LightNode> collection)
            : base(collection.ToDictionary(c => c.Id, c => c))
        {
        }
    }
}

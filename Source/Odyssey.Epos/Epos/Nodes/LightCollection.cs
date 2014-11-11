using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

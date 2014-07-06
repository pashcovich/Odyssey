using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Talos.Nodes
{
    public class CameraCollection : Dictionary<int, CameraNode>, ICameraService
    {
        public CameraCollection()
        {
        }

        public CameraCollection(int capacity)
            : base(capacity)
        {
        }

        public CameraCollection(IEnumerable<CameraNode> collection)
            : base(collection.ToDictionary(c => c.Id, c=>c))
        {
        }
    }
}

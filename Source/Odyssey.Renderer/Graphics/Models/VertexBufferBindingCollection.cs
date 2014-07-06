using System.Collections.Generic;

namespace Odyssey.Graphics.Models
{
    public class VertexBufferBindingCollection : List<VertexBufferBinding>
    {
        public VertexBufferBindingCollection()
        {
        }

        public VertexBufferBindingCollection(int capacity)
            : base(capacity)
        {
        }

        public VertexBufferBindingCollection(IEnumerable<VertexBufferBinding> collection)
            : base(collection)
        {
        }
    }
}

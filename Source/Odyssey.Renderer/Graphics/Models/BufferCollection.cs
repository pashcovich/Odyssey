using System.Collections.Generic;

namespace Odyssey.Graphics.Models
{
    public class BufferCollection : List<Buffer>
    {
        public BufferCollection()
        {
        }

        public BufferCollection(int capacity)
            : base(capacity)
        {
        }

        public BufferCollection(IEnumerable<Buffer> collection)
            : base(collection)
        {
        }
    }
}
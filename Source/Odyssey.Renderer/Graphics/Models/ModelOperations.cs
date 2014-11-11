using System.Collections.Generic;

namespace Odyssey.Graphics.Models
{
    public class ModelOperations : List<ModelOperation>
    {
        public ModelOperations()
        {
        }

        public ModelOperations(int capacity)
            : base(capacity)
        {
        }

        public ModelOperations(IEnumerable<ModelOperation> collection)
            : base(collection)
        {
        }
    }
}

using System.Collections.Generic;

namespace Odyssey.Graphics.Models
{
    public class ModelMeshPartCollection : List<ModelMeshPart>
    {
        public ModelMeshPartCollection()
        {
        }

        public ModelMeshPartCollection(int capacity)
            : base(capacity)
        {
        }

        public ModelMeshPartCollection(IEnumerable<ModelMeshPart> collection)
            : base(collection)
        {
        }
    }
}
using System.Collections.Generic;

namespace Odyssey.Graphics.Models
{
    public class ModelMeshCollection : List<ModelMesh>
    {
        public ModelMeshCollection()
        {
        }

        public ModelMeshCollection(int capacity)
            : base(capacity)
        {
        }

        public ModelMeshCollection(IEnumerable<ModelMesh> collection)
            : base(collection)
        {
        }
    }
}
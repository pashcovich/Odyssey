using System.Collections.Generic;
using System.Linq;
using Odyssey.Content.Meshes;

namespace Odyssey.Content.Models
{
    internal class MeshDataCollection : List<MeshData>
    {
        public MeshDataCollection()
        { }

        public MeshDataCollection(int capacity) : base(capacity)
        {}

        public bool IsEmpty { get { return Count == 0; } }

        public IEnumerable<Mesh> ToMeshCollection()
        {
            return (from mesh in this select new Mesh(mesh));
        }
    }
}

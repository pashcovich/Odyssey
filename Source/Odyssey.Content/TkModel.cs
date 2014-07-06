using System.Collections.Generic;
using System.Linq;

namespace Odyssey.Content
{
    public sealed class TkModel
    {
        public string Name { get; set;}
        public TkMeshPart[] MeshParts { get; private set; }

        public TkModel(string name, IEnumerable<TkMeshPart> meshParts)
        {
            Name = name;
            MeshParts = meshParts.ToArray();
        }
    }
}

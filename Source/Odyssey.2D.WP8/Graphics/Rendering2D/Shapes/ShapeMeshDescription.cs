using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Graphics.Rendering2D.Shapes
{
    public class ShapeMeshDescription
    {
        public string Id { get; internal set;}
        public int ArrayIndex { get; internal set; }
        public VertexPositionColor[] Vertices { get; internal set; }
        public ushort[] Indices { get; internal set; }
        public int Primitives { get { return Indices.Length / 3; } }

        public ShapeMeshDescription()
        {
        }

    }
}

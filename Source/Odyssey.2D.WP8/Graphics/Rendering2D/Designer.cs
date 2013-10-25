using Odyssey.Graphics.Rendering2D.Shapes;
using Odyssey.UserInterface.Style;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Graphics.Rendering2D
{
    public partial class Designer
    {
        private readonly List<ShapeMeshDescription> shapes;
        private readonly MeshInterface interfaceMesh;

        public ShapeMeshDescription[] Result { get; private set; }

        //public Vector2 Position { get; set; }
        //public float Width { get; set; }
        //public float Height { get; set; }
        //public Thickness BorderSize { get; set; }
        //public IGradient Gradient{ get; set; }
        //public Vector4[] Points { get; set; }

        public Designer()
        {
            shapes = new List<ShapeMeshDescription>();
        }

        public void Begin()
        {}

        public void End()
        {
            Result = shapes.ToArray();
        }

    }
}

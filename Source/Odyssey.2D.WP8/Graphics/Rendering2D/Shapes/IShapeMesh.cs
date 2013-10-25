using Odyssey.UserInterface.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Graphics.Rendering2D.Shapes
{
    public interface IShapeMesh
    {
        ShapeMeshDescription Shape { get; }
    }

    public interface IControlMesh : IControl
    {
        IEnumerable<ShapeMeshDescription> Shapes { get; }
    }
}

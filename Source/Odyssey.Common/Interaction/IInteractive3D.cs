using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Interaction
{
    public interface IInteractive3D
    {
        bool Intersects(Ray ray, out Vector3 point);
        bool IsInteractive { get; set; }
        bool IsHitTestVisible { get; set; }
    }
}

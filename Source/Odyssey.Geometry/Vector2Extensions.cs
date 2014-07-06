using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using Real = System.Single;
using Point = SharpDX.Vector2;

namespace Odyssey.Geometry
{
    internal static class Vector2Extensions
    {
        internal static void Offset(this Point vector, Real xOffset, Real yOffset)
        {
            vector.X += xOffset;
            vector.Y += yOffset;
        }
    }
}

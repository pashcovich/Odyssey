using System;
using SharpDX.Mathematics;
using Real = System.Single;
using Point = SharpDX.Mathematics.Vector2;
using Point3 = SharpDX.Mathematics.Vector3;
using Point4 = SharpDX.Mathematics.Vector4;

namespace Odyssey.Geometry
{
    public static class Vector3Extensions
    {
        public static Point XY(this Point3 point)
        {
            return new Vector2(point.X, point.Y);
        }

        public static Point4 ToVector4(this Point3 vector3, float w = 1.0f)
        {
            return new Point4(vector3, w);
        }
    }
}

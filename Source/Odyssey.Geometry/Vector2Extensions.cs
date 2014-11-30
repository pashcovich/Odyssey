using System;
using SharpDX.Mathematics;
using Real = System.Single;
using Point = SharpDX.Mathematics.Vector2;
using Point3 = SharpDX.Mathematics.Vector3;
using Point4 = SharpDX.Mathematics.Vector4;

namespace Odyssey.Geometry
{
    public static class Vector2Extensions
    {
        internal static void Offset(this Point point, Real xOffset, Single yOffset)
        {
            point.X += xOffset;
            point.Y += yOffset;
        }

        /// <summary>
        /// Calculates a vector perpendicular to the source vector.
        /// </summary>
        /// <param name="value">The source vector.</param>
        /// <returns>The resulting perpendicular vector.</returns>
        public static Point Perp(this Point value)
        {
            return new Point(-value.Y, value.X);
        }

        public static Point3 ToVector3(this Point vector2, float z = 0f)
        {
            return new Vector3(vector2, z);
        }

        public static Point4 ToVector4(this Point vector2, float z = 0f, float w = 0f)
        {
            return new Vector4(vector2, z, w);
        }
    }
}

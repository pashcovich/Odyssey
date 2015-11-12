using System;
using SharpDX;

namespace Odyssey.Geometry.Extensions
{
    public static class Vector2Extensions
    {
        internal static void Offset(this Vector2 point, Single xOffset, Single yOffset)
        {
            point.X += xOffset;
            point.Y += yOffset;
        }

        /// <summary>
        /// Calculates a vector perpendicular to the source vector.
        /// </summary>
        /// <param name="value">The source vector.</param>
        /// <returns>The resulting perpendicular vector.</returns>
        public static Vector2 Perp(this Vector2 value)
        {
            return new Vector2(-value.Y, value.X);
        }

        public static Vector3 ToVector3(this Vector2 vector2, float z = 0f)
        {
            return new Vector3(vector2, z);
        }

        public static Vector4 ToVector4(this Vector2 vector2, float z = 0f, float w = 0f)
        {
            return new Vector4(vector2, z, w);
        }
    }
}

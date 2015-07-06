using System;
using Odyssey.Engine;
using SharpDX.Mathematics;

namespace Odyssey.Geometry.Extensions
{
    public static class Vector3Extensions
    {
        public static Vector2 XY(this Vector3 point)
        {
            return new Vector2(point.X, point.Y);
        }

        public static Vector4 ToVector4(this Vector3 vector3, float w = 1.0f)
        {
            return new Vector4(vector3, w);
        }

        public static Vector2 Project(this Vector3 vector3, ICamera camera)
        {
            var viewport = camera.Viewport;
            return Vector3.Project(vector3, viewport.X, viewport.Y, viewport.Width, viewport.Height, viewport.MinDepth, viewport.MaxDepth, camera.View * camera.Projection).XY();
        }

        public static float Angle(Vector3 left, Vector3 right)
        {
            var lNorm = Vector3.Normalize(left);
            var rNorm = Vector3.Normalize(right);
            return (float)Math.Acos(Vector3.Dot(lNorm, rNorm));
        }

        public static float SignedAngle(Vector3 left, Vector3 right, Vector3 planeNormal)
        {
            var angle = Angle(left, right);
            var cross = Vector3.Cross(left, right);
            return angle*Math.Sign(Vector3.Dot(cross, planeNormal));
        }

    }
}

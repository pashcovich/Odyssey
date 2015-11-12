using Odyssey.Engine;
using SharpDX;

namespace Odyssey.Geometry.Extensions
{
    public static class RayExtensions
    {
        public static Ray GetPickRay(Vector2 position, ICamera camera)
        {
            return Ray.GetPickRay((int)position.X, (int)position.Y, camera.Viewport, camera.View * camera.Projection);
        }
    }
}

#if !ODYSSEY_ENGINE
using SharpDX;

namespace Odyssey.Interaction
{
    public interface IInteractive3D
    {
        bool Intersects(Ray ray, out Vector3 point);
        bool IsInteractive { get; set; }
        bool IsHitTestVisible { get; set; }
    }
}
#endif
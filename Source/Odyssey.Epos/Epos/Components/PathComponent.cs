using Odyssey.Geometry;
using Odyssey.Geometry.Primitives;
using SharpDX.Mathematics;

namespace Odyssey.Epos.Components
{
    public class PathComponent : Component, IFunction
    {
        public CurveBase<Vector3> Path { get; set; }

        public PathComponent() : base(ComponentTypeManager.GetType<PathComponent>()) {}

        public Vector3 Evaluate(float t)
        {
            return Path.Evaluate(t);
        }
    }
}

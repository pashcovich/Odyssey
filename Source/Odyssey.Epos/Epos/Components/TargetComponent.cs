using Odyssey.Animations;
using SharpDX;

namespace Odyssey.Epos.Components
{
    public class TargetComponent : Component
    {
        [Animatable]
        public Vector3 Location { get; set; }

        public TargetComponent() : base(ComponentTypeManager.GetType<TargetComponent>())
        {
            Location = Vector3.Zero;
        }
    }
}

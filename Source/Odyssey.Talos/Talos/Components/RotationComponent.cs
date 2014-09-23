using System.Diagnostics;
using Odyssey.Animations;
using SharpDX;

namespace Odyssey.Talos.Components
{
    [DebuggerDisplay("{Rotation}: ({Orientation})")]
    public class RotationComponent : Component
    {
        private Quaternion orientation;

        internal bool IsDirty { get; set; }

        [Animatable]
        public Quaternion Orientation
        {
            get { return orientation; }
            set
            {
                orientation = value;
                IsDirty = true;
            }
        }

        public RotationComponent() : base(ComponentTypeManager.GetType<RotationComponent>())
        {
            Orientation = Quaternion.Identity;
        }
    }
}

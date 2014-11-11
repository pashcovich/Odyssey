using System.Diagnostics;
using Odyssey.Animations;
using SharpDX.Mathematics;

namespace Odyssey.Epos.Components
{
    [DebuggerDisplay("{Orientation}: ({Orientation})")]
    public class OrientationComponent : Component
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

        public OrientationComponent() : base(ComponentTypeManager.GetType<OrientationComponent>())
        {
            Orientation = Quaternion.Identity;
        }
    }
}

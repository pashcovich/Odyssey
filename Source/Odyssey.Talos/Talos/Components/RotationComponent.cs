using System.Diagnostics;
using SharpDX;
using SharpYaml;
using SharpYaml.Serialization;

namespace Odyssey.Talos.Components
{
    [DebuggerDisplay("{Rotation}: ({Orientation})")]
    public class RotationComponent : Component
    {
        private Quaternion orientation;

        internal bool IsDirty { get; set; }

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

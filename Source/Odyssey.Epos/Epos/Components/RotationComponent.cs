using System.Diagnostics;
using SharpDX;

namespace Odyssey.Epos.Components
{
    [DebuggerDisplay("{Rotation}: ({Rotation})")]
    public class RotationComponent : Component
    {
        private Vector3 w;

        internal bool IsDirty { get; set; }

        public Vector3 AngularVelocity
        {
            get { return w; }
            set
            {
                w = value;
                IsDirty = true;
            }
        }

        public RotationComponent() : base(ComponentTypeManager.GetType<RotationComponent>())
        {
            w = Vector3.Zero;
        }
    }
}

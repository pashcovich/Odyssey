using SharpDX;
using SharpYaml;
using SharpYaml.Serialization;

namespace Odyssey.Talos.Components
{
    public class PositionComponent : Component
    {
        private Vector3 position;

        public Vector3 Position
        {
            get { return position; }
            set
            {
                position = value;
                IsDirty = true;
            }
        }

        internal bool IsDirty { get; set; }

        public PositionComponent()
            : base(ComponentTypeManager.GetType<PositionComponent>())
        {
            Position = Vector3.Zero;
        }

    }
}

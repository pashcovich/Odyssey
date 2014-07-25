using System.Diagnostics;
using SharpDX;
using SharpYaml;
using SharpYaml.Serialization;

namespace Odyssey.Talos.Components
{
    [DebuggerDisplay("{Scale}: ({Scaling})")]
    public class ScalingComponent : Component
    {
        public Vector3 Scale { get; set; }

        public ScalingComponent() : base(ComponentTypeManager.GetType<ScalingComponent>())
        {
            Scale = new Vector3(1, 1, 1);
        }
    }
}

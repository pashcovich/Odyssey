using SharpDX;
using SharpYaml;
using SharpYaml.Serialization;

namespace Odyssey.Talos.Components
{
    [YamlTag("Scaling")]
    public class ScalingComponent : Component
    {
        [YamlStyle(YamlStyle.Flow)]
        public Vector3 Scaling { get; set; }

        public ScalingComponent() : base(ComponentTypeManager.GetType<ScalingComponent>())
        {
            Scaling = new Vector3(1, 1, 1);
        }
    }
}

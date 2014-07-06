using SharpDX;
using SharpYaml;
using SharpYaml.Serialization;

namespace Odyssey.Talos.Components
{
    [YamlTag("Rotation")]
    public class RotationComponent : Component
    {
        [YamlStyle(YamlStyle.Flow)]
        public Quaternion Orientation { get; set; }
        [YamlStyle(YamlStyle.Flow)]
        public Quaternion Delta { get; set; }

        public RotationComponent() : base(ComponentTypeManager.GetType<RotationComponent>())
        {
            Orientation = Quaternion.Identity;
            Delta = Quaternion.Identity;
        }
    }
}

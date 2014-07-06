using SharpDX;
using SharpYaml;
using SharpYaml.Serialization;

namespace Odyssey.Talos.Components
{
    [YamlTag("Position")]
    public class PositionComponent : Component
    {
        [YamlMember(1)]
        [YamlStyle(YamlStyle.Flow)]
        public Vector3 Position { get; set; }

        public PositionComponent()
            : base(ComponentTypeManager.GetType<PositionComponent>())
        {
            Position = Vector3.Zero;
        }

    }
}

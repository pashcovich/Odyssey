using SharpYaml.Serialization;

namespace Odyssey.Talos.Components
{
    [YamlTag("PointLight")]
    public class PointLightComponent : LightComponent
    {
        public PointLightComponent() : base(ComponentTypeManager.GetType<PointLightComponent>())
        { }
    }
}

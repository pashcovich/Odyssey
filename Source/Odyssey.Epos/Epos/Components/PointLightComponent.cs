using SharpYaml.Serialization;

namespace Odyssey.Epos.Components
{
    [YamlTag("PointLight")]
    public class PointLightComponent : LightComponent
    {
        public PointLightComponent() : base(ComponentTypeManager.GetType<PointLightComponent>())
        { }
    }
}

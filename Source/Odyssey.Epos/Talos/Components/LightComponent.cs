using SharpDX;
using SharpYaml;
using SharpYaml.Serialization;

namespace Odyssey.Epos.Components
{
    [RequiredComponent(typeof(TransformComponent))]
    public abstract class LightComponent : Component
    {
        public int LightId { get; private set; }
        [YamlStyle(YamlStyle.Flow)]
        public Color4 Diffuse { get; set; }
        public float Intensity { get; set; }
        public float Range { get; set; }

        protected LightComponent(ComponentType type) : base(type)
        {
            LightId = 0;
            Diffuse = Color4.White;
            Intensity = 1.0f;
            Range = 100.0f;
        }
    }
}

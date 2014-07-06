using Odyssey.Graphics.Effects;
using SharpDX;
using SharpYaml;
using SharpYaml.Serialization;

namespace Odyssey.Talos.Components
{
    [OptionalComponent(typeof(ShaderComponent), typeof(PostProcessComponent))]
    [YamlTag("Material")]
    public class MaterialComponent : Component, IMaterial
    {
        public float AmbientCoefficient { get; set; }
        public float DiffuseCoefficient { get; set; }
        public float SpecularCoefficient { get; set; }
        public float SpecularPower { get; set; }
        [YamlStyle(YamlStyle.Flow)]
        public Color4 Ambient { get; set; }
        [YamlStyle(YamlStyle.Flow)]
        public Color4 Diffuse { get; set; }
        [YamlStyle(YamlStyle.Flow)]
        public Color4 Specular { get; set; }

        public MaterialComponent() : base(ComponentTypeManager.GetType<MaterialComponent>())
        {
            AmbientCoefficient = 1;
            DiffuseCoefficient = 1;
            SpecularCoefficient = 1f;
            SpecularPower = 16f;
            Specular = new Color4(1f, 1f, 1f, 1f);
            Ambient = new Color4(0.1f, 0.1f, 0.1f, 1f);
            Diffuse = new Color4(0.25f, 0.75f, 0.25f, 1);
        }
    }
}

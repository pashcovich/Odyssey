using System.Collections.Generic;
using System.Runtime.Serialization;
using Odyssey.Daedalus.Shaders.Nodes.Operators;
using Odyssey.Daedalus.Shaders.Structs;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Shaders;

namespace Odyssey.Daedalus.Shaders.Nodes.Functions
{
    public class NormalMappingNode : PhongLightingNode
    {
        private TechniqueKey lightingTechniqueKey;

        [DataMember]
        [SupportedType(Type.Float4)]
        [VertexShader(VertexShaderFlags.Tangent)]
        [PixelShader(PixelShaderFlags.NormalMap)]
        public TextureSampleNode NormalMapSamplerNode { get; set; }

        public override string Access()
        {
            return LightingMethod.Call((Struct) Light.Output, (Struct) Material.Output, (Vector) LightDirection.Output,
                (Vector) ViewDirection.Output, (Vector) Normal.Output,
                (Vector) DiffuseMapSample.Coordinates.Output);
        }

        public override void Validate(TechniqueKey key)
        {
            lightingTechniqueKey = new TechniqueKey(ps: PixelShaderFlags.Diffuse | PixelShaderFlags.DiffuseMap |  PixelShaderFlags.Specular);
            base.Validate(lightingTechniqueKey);
        }
    }
}

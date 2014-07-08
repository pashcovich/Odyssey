using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Shaders;
using Odyssey.Tools.ShaderGenerator.Shaders.Methods;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Operators;
using Odyssey.Tools.ShaderGenerator.Shaders.Structs;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Functions
{
    public class NormalMappingNode : PhongLightingNode
    {
        private TechniqueKey lightingTechniqueKey;

        [DataMember]
        [SupportedType(Type.Float4)]
        [VertexShader(VertexShaderFlags.Tangent)]
        [PixelShader(PixelShaderFlags.NormalMap)]
        public TextureSampleNode NormalMapSamplerNode { get; set; }

        public override IEnumerable<INode> DescendantNodes
        {
            get
            {
                foreach (INode node in base.DescendantNodes)
                    yield return node;

                yield return NormalMapSamplerNode;
                foreach (var node in NormalMapSamplerNode.DescendantNodes)
                    yield return node; 
            }
        }

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

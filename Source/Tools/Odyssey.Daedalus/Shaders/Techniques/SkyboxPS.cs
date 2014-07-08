using Odyssey.Engine;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Shaders;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Math;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Operators;
using Odyssey.Tools.ShaderGenerator.Shaders.Structs;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Techniques
{
    public class SkyboxPS : Shader
    {
        public SkyboxPS()
        {
            Name = "SkyboxPS";
            Type = ShaderType.Pixel;
            KeyPart = new TechniqueKey(ps: PixelShaderFlags.Diffuse);
            FeatureLevel = FeatureLevel.PS_4_0_Level_9_1;
            EnableSeparators = true;
            var inputStruct = SkyboxVS.VSOutput;
            inputStruct.Name = "input";

            InputStruct = inputStruct;
            OutputStruct = Struct.PixelShaderOutput;

            Struct cbMaterial = Structs.ConstantBuffer.CBMaterial;
            Struct material = (Struct)cbMaterial[Param.Struct.Material];
            Add(cbMaterial);

            Texture tDiffuse = Texture.CubeMap;
            Sampler sDiffuseSampler = Sampler.MinMagMipLinearWrap;
            Add(tDiffuse);
            Add(sDiffuseSampler);
            TextureSampleNode textureSample = new TextureSampleNode
            {
                Coordinates = new ReferenceNode {Value = InputStruct[Param.SemanticVariables.Texture]},
                Texture = tDiffuse,
                Sampler = sDiffuseSampler
            };
            MultiplyNode multiply = new MultiplyNode
            {
                Input1 = textureSample,
                Input2 = new ReferenceNode {Value = material[Param.Material.Diffuse]}
            };

            Result = new PSOutputNode
            {
                FinalColor = multiply,
                Output = OutputStruct
            };
        }
    }
}

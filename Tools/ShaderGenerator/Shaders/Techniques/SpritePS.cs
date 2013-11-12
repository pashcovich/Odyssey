using Odyssey.Engine;
using Odyssey.Graphics.Materials;
using Odyssey.Graphics.Rendering;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Operators;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Math;
using Odyssey.Tools.ShaderGenerator.Shaders.Structs;
using System.Runtime.Serialization;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Techniques
{
    [DataContract]
    [PixelShader(PixelShaderFlags.Diffuse)]
    [PixelShader(PixelShaderFlags.DiffuseMap)]
    public class SpritePS : Shader
    {
        public SpritePS()
        {
            Name = "SpritePS";
            Type = ShaderType.Pixel;
            KeyPart = new TechniqueKey(ps: PixelShaderFlags.Diffuse | PixelShaderFlags.DiffuseMap);
            FeatureLevel = Odyssey.Graphics.Materials.FeatureLevel.PS_4_0_Level_9_1;
            EnableSeparators = true;
            var inputStruct = SpriteVS.VSOut;
            inputStruct.Name = "input";

            InputStruct = inputStruct;
            OutputStruct = Struct.PixelShaderOutput;
            Struct cbMaterial = ConstantBuffer.CBMaterial;
            Struct material = (Struct)cbMaterial[Param.Struct.Material];
            Variable tDiffuse = Texture.Diffuse;
            Variable sDiffuse = Sampler.MinMagMipLinearWrap;

            Add(cbMaterial);
            Add(sDiffuse);
            Add(tDiffuse);

            tDiffuse.SetMarkup("Key", "SpriteDiffuse");

            TextureSampleNode nTexSample = new TextureSampleNode
            {
                Coordinates = new ConstantNode { Value = InputStruct[Param.SemanticVariables.Texture]} ,
                Texture = tDiffuse,
                Sampler = sDiffuse,
            };

            MultiplyNode nMultiply = new MultiplyNode()
            {
                Input1 = nTexSample,
                Input2 = new ConstantNode{ Value = material[Param.Material.Diffuse]},
                Output = new Vector { Name = "cFinal", Type = Shaders.Type.Float4 }
            };

            Result = new PSOutputNode()
            {
                FinalColor = nMultiply,
                Output = OutputStruct
            };
        }


    }
}

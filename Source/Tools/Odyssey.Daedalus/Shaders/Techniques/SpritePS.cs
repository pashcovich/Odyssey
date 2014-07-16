using Odyssey.Daedalus.Shaders.Nodes;
using Odyssey.Daedalus.Shaders.Nodes.Math;
using Odyssey.Daedalus.Shaders.Nodes.Operators;
using Odyssey.Daedalus.Shaders.Structs;
using Odyssey.Engine;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Shaders;
using System.Runtime.Serialization;
using ConstantBuffer = Odyssey.Daedalus.Shaders.Structs.ConstantBuffer; 

namespace Odyssey.Daedalus.Shaders.Techniques
{
    [DataContract]
    public class SpritePS : Shader
    {
        public SpritePS()
        {
            Name = "SpritePS";
            Type = ShaderType.Pixel;
            KeyPart = new TechniqueKey(ps: PixelShaderFlags.Diffuse | PixelShaderFlags.DiffuseMap);
            FeatureLevel = FeatureLevel.PS_4_0_Level_9_1;
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

            TextureSampleNode nTexSample = new TextureSampleNode
            {
                Coordinates = new ReferenceNode { Value = InputStruct[Param.SemanticVariables.Texture]} ,
                Texture = tDiffuse,
                Sampler = sDiffuse,
                IsVerbose = true,
                Output = new Vector() { Name="cDiffuse", Type = Shaders.Type.Float4}
            };

            MultiplyNode nMultiply = new MultiplyNode
            {
                Input1 = nTexSample,
                Input2 = new ReferenceNode{ Value = material[Param.Material.Diffuse]},
                Output = new Vector { Name = "cFinal", Type = Shaders.Type.Float4 }
            };

            Result = new PSOutputNode
            {
                FinalColor = nMultiply,
                Output = OutputStruct
            };
        }

    }

    public class SpriteDebugPS : Shader
    {
        public SpriteDebugPS()
        {
            Name = "SpriteDebugPS";
            Type = ShaderType.Pixel;
            KeyPart = new TechniqueKey(ps: PixelShaderFlags.Diffuse);
            FeatureLevel = FeatureLevel.PS_4_0_Level_9_1;
            EnableSeparators = true;
            var inputStruct = SpriteVS.VSOut;
            inputStruct.Name = "input";

            InputStruct = inputStruct;
            OutputStruct = Struct.PixelShaderOutput;
            Struct cbMaterial = ConstantBuffer.CBMaterial;
            Struct material = (Struct) cbMaterial[Param.Struct.Material];
            Add(cbMaterial);

            Result = new PSOutputNode()
            {
                FinalColor = new ReferenceNode { Value = material["Diffuse"]},
                Output = OutputStruct
            };
        }
    }
}

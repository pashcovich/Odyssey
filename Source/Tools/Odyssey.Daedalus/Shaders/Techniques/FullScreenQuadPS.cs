using Odyssey.Engine;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Shaders;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Operators;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Math;
using Odyssey.Tools.ShaderGenerator.Shaders.Structs;
using System.Runtime.Serialization;
using ConstantBuffer = Odyssey.Tools.ShaderGenerator.Shaders.Structs.ConstantBuffer; 

namespace Odyssey.Tools.ShaderGenerator.Shaders.Techniques
{
    public class FullScreenQuadPS : Shader
    {
        public FullScreenQuadPS()
        {
            Name = "FullScreenQuadPS";
            Type = ShaderType.Pixel;
            KeyPart = new TechniqueKey(ps: PixelShaderFlags.DiffuseMap);
            FeatureLevel = FeatureLevel.PS_4_0_Level_9_1;
            EnableSeparators = true;
            var inputStruct = SpriteVS.VSOut;
            inputStruct.Name = "input";

            InputStruct = inputStruct;
            OutputStruct = Struct.PixelShaderOutput;
            Variable tDiffuse = Texture.Diffuse;
            Variable sDiffuse = Sampler.MinMagMipLinearWrap;

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

            Result = new PSOutputNode
            {
                FinalColor = nTexSample,
                Output = OutputStruct
            };
        }

    }
}

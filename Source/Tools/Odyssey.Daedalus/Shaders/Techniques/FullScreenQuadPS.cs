using Odyssey.Daedalus.Shaders.Nodes;
using Odyssey.Daedalus.Shaders.Nodes.Operators;
using Odyssey.Daedalus.Shaders.Structs;
using Odyssey.Engine;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Shaders;
using Odyssey.Daedalus.Shaders.Nodes.Math;
using System.Runtime.Serialization;
using ConstantBuffer = Odyssey.Daedalus.Shaders.Structs.ConstantBuffer; 

namespace Odyssey.Daedalus.Shaders.Techniques
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

using Odyssey.Engine;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Shaders;
using Odyssey.Tools.ShaderGenerator.Shaders.Methods;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Functions;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Math;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Operators;
using Odyssey.Tools.ShaderGenerator.Shaders.Structs;
using ConstantBuffer = Odyssey.Tools.ShaderGenerator.Shaders.Structs.ConstantBuffer;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Techniques
{
    public class BloomExtractPS : Shader
    {
        public BloomExtractPS()
        {
            Name = "BloomExtractPS";
            Type = ShaderType.Pixel;
            KeyPart = new TechniqueKey(ps: PixelShaderFlags.Diffuse | PixelShaderFlags.DiffuseMap);
            FeatureLevel = FeatureLevel.PS_4_0_Level_9_1;
            EnableSeparators = true;
            var inputStruct = SpriteVS.VSOut;
            inputStruct.Name = "input";

            InputStruct = inputStruct;
            OutputStruct = Struct.PixelShaderOutput;
            Texture tDiffuse = Texture.Diffuse;
            Sampler sLinearWrap = Sampler.MinMagMipLinearWrap;
            var cbFrame = CBFrame;

            Add(sLinearWrap);
            Add(tDiffuse);
            Add(cbFrame);

            TextureSampleNode nTexSample = new TextureSampleNode
            {
                Coordinates = new ReferenceNode { Value = InputStruct[Param.SemanticVariables.Texture]} ,
                Texture = tDiffuse,
                Sampler = sLinearWrap,
                IsVerbose = true,
                Output = new Vector { Name="cDiffuse", Type = Shaders.Type.Float4}
            };

            UnaryFunctionNode nSaturate = new UnaryFunctionNode
            {
                Input1 = new DivisionNode()
                {
                    Input1 = new SubtractionNode()
                    {
                        Input1 = nTexSample,
                        Input2 = new ReferenceNode() {Value = cbFrame[Param.Floats.BloomThreshold]},
                        Parenthesize = true
                    },
                    Input2 = new SubtractionNode()
                    {
                        Input1 = new ScalarNode(){Value = 1.0f},
                        Input2 = new ReferenceNode() { Value = cbFrame[Param.Floats.BloomThreshold] },
                        Parenthesize = true
                    }
                },
                Function = HlslIntrinsics.Saturate
            };

            Result = new PSOutputNode
            {
                FinalColor = nSaturate,
                Output = OutputStruct
            };
        }

        public static ConstantBuffer CBFrame
        {
            get
            {
                var cbFrame = new ConstantBuffer
                {
                    Name = Param.ConstantBuffer.PerFrame,
                    UpdateType = UpdateType.InstanceFrame,
                };
                cbFrame.Add(Float.BloomThreshold);
                return cbFrame;
            }
        }
    }
}

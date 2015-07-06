using Odyssey.Daedalus.Shaders.Methods;
using Odyssey.Daedalus.Shaders.Nodes;
using Odyssey.Daedalus.Shaders.Nodes.Functions;
using Odyssey.Daedalus.Shaders.Nodes.Math;
using Odyssey.Daedalus.Shaders.Nodes.Operators;
using Odyssey.Daedalus.Shaders.Structs;
using Odyssey.Engine;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Shaders;
using ConstantBuffer = Odyssey.Daedalus.Shaders.Structs.ConstantBuffer;

namespace Odyssey.Daedalus.Shaders.Techniques
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

        public static Structs.ConstantBuffer CBFrame
        {
            get
            {
                var cbFrame = new Structs.ConstantBuffer
                {
                    Name = Param.ConstantBuffer.PerFrame,
                    CbUpdateType = CBUpdateType.InstanceFrame,
                };
                cbFrame.Add(Float.BloomThreshold);
                return cbFrame;
            }
        }
    }
}

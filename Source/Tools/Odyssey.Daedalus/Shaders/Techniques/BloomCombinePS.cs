#region Using Directives

using System.Collections.Generic;
using Odyssey.Daedalus.Data;
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

#endregion

namespace Odyssey.Daedalus.Shaders.Techniques
{
    public class BloomCombinePS : Shader
    {
        public BloomCombinePS()
        {
            Name = "BloomCombinePS";
            Type = ShaderType.Pixel;
            KeyPart = new TechniqueKey(ps: PixelShaderFlags.Diffuse | PixelShaderFlags.DiffuseMap);
            FeatureLevel = FeatureLevel.PS_4_0_Level_9_1;
            EnableSeparators = true;
            var inputStruct = SpriteVS.VSOut;
            inputStruct.Name = "input";

            InputStruct = inputStruct;
            OutputStruct = Struct.PixelShaderOutput;
            Texture tDiffuse = Texture.Diffuse;
            Texture tBloom = new Texture {Name = "tBloom", Type = Shaders.Type.Texture2D};
            Sampler sLinearWrap = Sampler.MinMagMipLinearWrap;
            var cbFrame = CBFrame;

            var blurParams = (Struct) cbFrame[0];

            Add(sLinearWrap);
            Add(tDiffuse);
            Add(tBloom);
            Add(cbFrame);

            TextureSampleNode nTexSample = new TextureSampleNode
            {
                Coordinates = new ReferenceNode {Value = InputStruct[Param.SemanticVariables.Texture]},
                Texture = tDiffuse,
                Sampler = sLinearWrap,
                IsVerbose = true,
                Output = new Vector {Name = "cDiffuse", Type = Shaders.Type.Float4},
            };

            TextureSampleNode nBloomSample = new TextureSampleNode
            {
                Coordinates = new ReferenceNode {Value = InputStruct[Param.SemanticVariables.Texture]},
                Texture = tBloom,
                Sampler = sLinearWrap,
                IsVerbose = true,
                Output = new Vector {Name = "cBloom", Type = Shaders.Type.Float4}
            };

            var mAdjustSaturation = new AdjustSaturation();

            FunctionNode nAdjustBloomSaturation = new FunctionNode
            {
                Inputs = new List<INode>
                {
                    nBloomSample,
                    new ReferenceNode {Value = blurParams[Param.Floats.BloomSaturation]}
                },
                Method = mAdjustSaturation,
                ReturnType = Shaders.Type.Float4
            };

            FunctionNode nAdjustBaseSaturation = new FunctionNode
            {
                Inputs = new List<INode>
                {
                    nTexSample,
                    new ReferenceNode {Value = blurParams[Param.Floats.BloomBaseSaturation]}
                },
                Method = mAdjustSaturation,
                ReturnType = Shaders.Type.Float4
            };

            MultiplyNode nMulBloom = new MultiplyNode
            {
                Input1 = nAdjustBloomSaturation,
                Input2 = new ReferenceNode {Value = blurParams[Param.Floats.BloomIntensity]},
                Output = nBloomSample.Output,
                IsVerbose = true,
                Declare = false
            };
            MultiplyNode nMulBase = new MultiplyNode
            {
                Input1 = nAdjustBaseSaturation,
                Input2 = new ReferenceNode {Value = blurParams[Param.Floats.BloomBaseIntensity]},
                Output = nTexSample.Output,
                IsVerbose = true,
                Declare = false
            };

            MultiplyNode nDarken = new MultiplyNode
            {
                Input1 = nMulBase,
                Input2 = new SubtractionNode
                {
                    Input1 = new ScalarNode {Value = 1},
                    Input2 = new UnaryFunctionNode {Input1 = nMulBloom, Function = HlslIntrinsics.Saturate},
                    Parenthesize = true
                },
                Output = nTexSample.Output,
                IsVerbose = true,
                Declare = false,
                AssignToInput1 = true
            };

            Result = new PSOutputNode
            {
                FinalColor = new AdditionNode {Input1 = nDarken, Input2 = nMulBloom},
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
                    CbUpdateType = CBUpdateType.InstanceFrame,
                };
                cbFrame.Add(BlurParameters);

                return cbFrame;
            }
        }

        public static Struct BlurParameters
        {
            get
            {
                Struct blurParams = new Struct
                {
                    CustomType = "BlurParameters",
                    Name = "blurParams",
                    EngineReference = ReferenceFactory.Effect.BloomParameters
                };

                blurParams.Add(new Vector {Name = Param.Floats.BloomIntensity, Type = Shaders.Type.Float});
                blurParams.Add(new Vector {Name = Param.Floats.BloomBaseIntensity, Type = Shaders.Type.Float});
                blurParams.Add(new Vector {Name = Param.Floats.BloomSaturation, Type = Shaders.Type.Float});
                blurParams.Add(new Vector {Name = Param.Floats.BloomBaseSaturation, Type = Shaders.Type.Float});
                return blurParams;
            }
        }
    }
}
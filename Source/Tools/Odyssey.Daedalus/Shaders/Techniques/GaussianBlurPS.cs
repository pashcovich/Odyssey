using Odyssey.Daedalus.Data;
using Odyssey.Daedalus.Shaders.Nodes;
using Odyssey.Daedalus.Shaders.Nodes.Math;
using Odyssey.Daedalus.Shaders.Nodes.Operators;
using Odyssey.Daedalus.Shaders.Structs;
using Odyssey.Engine;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.PostProcessing;
using Odyssey.Graphics.Shaders;

namespace Odyssey.Daedalus.Shaders.Techniques
{
    public class GaussianBlurPS : Shader
    {
        public GaussianBlurPS()
        {
            Name = "GaussianBlurPS";
            Type = ShaderType.Pixel;
            FeatureLevel = FeatureLevel.PS_4_0_Level_9_1;
            KeyPart = new TechniqueKey(ps: PixelShaderFlags.DiffuseMap);
            EnableSeparators = true;
            Struct inputStruct = SpriteVS.VSOut;
            inputStruct.Name = "input";
            InputStruct = inputStruct;
            var cbStatic = CBFrame;
            var fOffsetsAndWeights = CBFrame[Param.Floats.BlurOffsetsAndWeights];

            Texture tDiffuse = Texture.Diffuse;
            Sampler sLinear = Sampler.MinMagMipLinearWrap;

            Add(tDiffuse);
            Add(sLinear);
            Add(cbStatic);

            DeclarationNode nColor = DeclarationNode.InitNode("color", Shaders.Type.Float4, 0, 0, 0, 0);
            ArrayNode fOffsetWeight = new ArrayNode {Input = fOffsetsAndWeights, Index = "i"};

            AdditionNode nBlur = new AdditionNode
            {
                PreCondition = new ForBlock
                {
                    PreCondition = nColor,
                    StartCondition = new ScalarNode {Value = 0},
                    EndCondition = new ScalarNode {Value = 15}
                },
                OpensBlock = true,
                Input1 = nColor,
                Input2 = new MultiplyNode
                {
                    Input1 = new TextureSampleNode
                    {
                        Texture = tDiffuse,
                        Sampler = sLinear,
                        Coordinates = new AdditionNode
                        {
                            Input1 = new ReferenceNode {Value = InputStruct[Param.SemanticVariables.Texture]},
                            Input2 = new SwizzleNode {Input = fOffsetWeight, Swizzle = new []{Swizzle.X, Swizzle.Y}},
                        }
                    },
                    Input2 = new SwizzleNode { Input = fOffsetWeight, Swizzle = new[] { Swizzle.Z } },
                },
                ClosesBlock = true,
                IsVerbose = true,
                Declare = false,
                AssignToInput1 = true,
                Output = nColor.Output
            };

            OutputStruct = Struct.PixelShaderOutput;
            Result = new PSOutputNode
            {
                FinalColor = nBlur,
                Output = OutputStruct
            };
        }

        public static Structs.ConstantBuffer CBFrame
        {
            get
            {
                Structs.ConstantBuffer cbStatic = new Structs.ConstantBuffer
                {
                    Name = Param.ConstantBuffer.Static,
                    UpdateType = UpdateType.InstanceFrame,
                };
                var offsets = new FloatArray
                {
                    Name = Param.Floats.BlurOffsetsAndWeights,
                    ArrayItemType = Shaders.Type.Float4,
                    Length = Blur.SampleCount,
                    EngineReference = ReferenceFactory.Effect.BlurOffsetsWeights
                };
                cbStatic.Add(offsets);

                return cbStatic;
            }
        }

        

    }
}

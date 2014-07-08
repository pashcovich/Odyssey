using Odyssey.Engine;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Shaders;
using Odyssey.Tools.ShaderGenerator.Shaders.Methods;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Functions;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Math;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Operators;
using Odyssey.Tools.ShaderGenerator.Shaders.Structs;
using System.Runtime.Serialization;
using ConstantBuffer = Odyssey.Tools.ShaderGenerator.Shaders.Structs.ConstantBuffer;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Techniques
{
    public class PhongCubeMapPS : PhongPS
    {
        public PhongCubeMapPS()
        {
            Name = "PhongCubeMapPS";
            KeyPart = new TechniqueKey(ps: PixelShaderFlags.Diffuse | PixelShaderFlags.Specular | PixelShaderFlags.CubeMap);

            var input = PhongCubeMapVS.VSOut;
            input.Name = "input";
            InputStruct = input;

            PhongLightingNode nPhongLighting = (PhongLightingNode)((PSOutputNode)Result).FinalColor;
            nPhongLighting.DiffuseMap = true;
            nPhongLighting.CubeMap = true;

            Texture tDiffuse = Texture.CubeMap;
            Sampler sDiffuseSampler = Sampler.MinMagMipLinearWrap;
            nPhongLighting.DiffuseMapSample = new TextureSampleNode
            {
                Coordinates = new ReferenceNode { Value = InputStruct[Param.SemanticVariables.Texture] },
                Texture = tDiffuse,
                Sampler = sDiffuseSampler,
                IsVerbose = true
            };
            Add(tDiffuse);
            Add(sDiffuseSampler);
        }
    }

    [DataContract]
    public class PhongDiffuseMapPS : PhongPS
    {
        public PhongDiffuseMapPS()
        {
            Name = "PhongDiffuseMapPS";
            KeyPart = new TechniqueKey(ps: PixelShaderFlags.Diffuse | PixelShaderFlags.DiffuseMap | PixelShaderFlags.Specular);
            PhongLightingNode nPhongLighting = (PhongLightingNode)((PSOutputNode)Result).FinalColor;
            nPhongLighting.DiffuseMap = true;

            Texture tDiffuse = Texture.Diffuse;
            Sampler sDiffuseSampler = Sampler.MinMagMipLinearWrap;
            nPhongLighting.DiffuseMapSample = new TextureSampleNode
            {
                Coordinates = new ReferenceNode { Value = InputStruct[Param.SemanticVariables.Texture] },
                Texture = tDiffuse,
                Sampler = sDiffuseSampler,
                IsVerbose = true
            };
            Add(tDiffuse);
            Add(sDiffuseSampler);
        }
    }

    [DataContract]
    public class PhongPS : Shader
    {
        public PhongPS()
        {
            Name = "PhongPS";
            Type = ShaderType.Pixel;
            KeyPart = new TechniqueKey(ps: PixelShaderFlags.Diffuse | PixelShaderFlags.Specular);
            FeatureLevel = FeatureLevel.PS_4_0_Level_9_1;
            EnableSeparators = true;
            var input = Struct.VertexPositionNormalTextureOut;
            input.Name = "input";
            InputStruct = input;
            OutputStruct = Struct.PixelShaderOutput;

            ConstantBuffer cbStatic = CBLight;
            ConstantBuffer cbFrame = CBFrame;
            ConstantBuffer cbInstance = ConstantBuffer.CBMaterial;

            Struct pointLight = (Struct)cbStatic[Param.Struct.PointLight];
            Struct material = (Struct)cbInstance[Param.Struct.Material];

            Add(cbStatic);
            Add(cbFrame);
            Add(cbInstance);

            CastNode vWorldPos = new CastNode
            {
                Input = new SwizzleNode { Input = new ReferenceNode{Value = InputStruct[Param.SemanticVariables.WorldPosition] }
                    , Swizzle = new[] { Swizzle.X, Swizzle.Y, Swizzle.Z } },
                Output = new Vector { Type = Shaders.Type.Float3, Name = "vWorldPos", },
                IsVerbose = true
            };

            SubtractionNode vLightDirection = new SubtractionNode
            {
                Input1 = new ReferenceNode { Value = pointLight[Param.Light.Position] },
                Input2 = vWorldPos,
            };

            SubtractionNode vViewDirection = new SubtractionNode
            {
                Input1 = new ReferenceNode { Value = cbFrame[Param.Vectors.CameraPosition] },
                Input2 = vWorldPos,
            };

            ReferenceNode nNormal = new ReferenceNode {Value = InputStruct[Param.SemanticVariables.Normal]};
            
            UnaryFunctionNode normalizeNormal = new UnaryFunctionNode
            {
                Input1 = nNormal,
                Function = HlslIntrinsics.Normalize,
                Output = new Vector { Type = nNormal.Output.Type, Name = "vNormal"},
                IsVerbose = true,
            };

            UnaryFunctionNode normalizeViewDirection = new UnaryFunctionNode
            {
                Input1 = vViewDirection,
                Function = HlslIntrinsics.Normalize,
                Output = new Vector { Type = vViewDirection.Output.Type, Name = "vViewDirection" },
                IsVerbose = true,
            };
            
            UnaryFunctionNode normalizeLightDirection = new UnaryFunctionNode
            {
                Input1 = vLightDirection,
                Function = HlslIntrinsics.Normalize,
                Output = new Vector { Type = vLightDirection.Output.Type, Name = "vLightDirection" },
                IsVerbose = true,
            };

            InvertNode vLightDirectionInv = new InvertNode
            {
                Input = normalizeLightDirection,
                //Output = new Vector { Type = Shaders.Type.Float3, Name = "vLightDirection" },
            };

            PhongLightingNode nPhongLighting = new PhongLightingNode
            {
                Light = new ReferenceNode { Value = pointLight },
                Material = new ReferenceNode { Value = material },
                ViewDirection = normalizeViewDirection,
                Normal = normalizeNormal,
                LightDirection = vLightDirectionInv,
                Specular = true,
            };

            Result = new PSOutputNode
            {
                FinalColor = nPhongLighting,
                Output = OutputStruct
            };
        }

        public static ConstantBuffer CBFrame
        {
            get
            {
                ConstantBuffer cbFrame = new ConstantBuffer
                {
                    Name = Param.ConstantBuffer.PerFrame,
                    UpdateType = UpdateType.SceneFrame,
                };
                cbFrame.Add(Vector.CameraPosition);
                return cbFrame;
            }
        }

        public static ConstantBuffer CBLight
        {
            get
            {
                ConstantBuffer cbStatic = new ConstantBuffer
                {
                    Name = Param.ConstantBuffer.Static,
                    UpdateType = UpdateType.SceneFrame,
                };

                cbStatic.Add(Struct.PointLight);
                return cbStatic;
            }
        }
    }
}
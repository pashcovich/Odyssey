using Odyssey.Engine;
using Odyssey.Graphics.Materials;
using Odyssey.Graphics.Rendering;
using Odyssey.Tools.ShaderGenerator.Shaders.Methods;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Operators;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Functions;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Math;
using Odyssey.Tools.ShaderGenerator.Shaders.Structs;
using System.Runtime.Serialization;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Techniques
{
    [DataContract]
    [PixelShader(PixelShaderFlags.Diffuse)]
    [PixelShader(PixelShaderFlags.Specular)]
    public class PhongPS : Shader
    {
        public PhongPS()
        {
            Name = "PhongPS";
            Type = ShaderType.Pixel;
            KeyPart = new TechniqueKey(ps: PixelShaderFlags.Diffuse | PixelShaderFlags.Specular);
            FeatureLevel = Odyssey.Graphics.Materials.FeatureLevel.PS_4_0_Level_9_1;
            EnableSeparators = true;
            var input = PhongVS.VSOut;
            input.Name = "input";
            InputStruct = input;
            OutputStruct = Struct.PixelShaderOutput;

            ConstantBuffer cbStatic = CBStatic;
            ConstantBuffer cbFrame= CBFrame;

            Struct pointLight = (Struct)cbStatic[Param.Struct.PointLight];
            Struct material = (Struct)cbFrame[Param.Struct.Material];

            Add(cbStatic);
            Add(cbFrame);

            CastNode vWorldPos = new CastNode
            {
                Input = new ConstantNode { Value = InputStruct[Param.SemanticVariables.WorldPosition] },
                Output = new Vector { Type = Shaders.Type.Float3, Name = "vWorldPos", Swizzle = new[] { Swizzle.X, Swizzle.Y, Swizzle.Z } },
                IsVerbose = true
            };

            SubtractionNode vLightDirection = new SubtractionNode
            {
                Input1 =vWorldPos, 
                Input2 = new ConstantNode { Value = pointLight[Param.Light.Position] },
                Output = new Vector { Type = Shaders.Type.Float3, Name="vLightDirection"},
                IsVerbose = true,
            };
            SubtractionNode vViewDirection = new SubtractionNode
            {
                Input1 = new ConstantNode { Value = cbFrame[Param.Vectors.CameraPosition] },
                Input2 = vWorldPos,
                Output = new Vector { Type = Shaders.Type.Float3, Name="vViewDirection"},
                IsVerbose = true,
            };

            PhongLightingNode nPhong = new PhongLightingNode
            {
                Light = new ConstantNode { Value = pointLight },
                Material = new ConstantNode { Value = material },
                ViewDirection = vViewDirection,
                Normal = new ConstantNode {Value= InputStruct[Param.SemanticVariables.Normal] },
                LightDirection = vLightDirection,
                Specular = true,
            };

            Result = new PSOutputNode()
            {
                FinalColor = nPhong,
                Output = OutputStruct
            };
        }

        public static ConstantBuffer CBStatic
        {
            get
            {
                ConstantBuffer cbStatic = new ConstantBuffer
                {
                    Name = Param.ConstantBuffer.Static,
                    UpdateFrequency = UpdateFrequency.Static,
                };

                cbStatic.Add(Struct.PointLight0);
                return cbStatic;
            }
        }

        public static ConstantBuffer CBFrame
        {
            get
            {
                ConstantBuffer cbFrame = new ConstantBuffer
                {
                    Name = Param.ConstantBuffer.PerFrame,
                    UpdateFrequency = UpdateFrequency.PerFrame,
                };
                cbFrame.Add(Struct.Material);
                cbFrame.Add(Vector.CameraPosition);
                return cbFrame;
            }
        }
    }

    [DataContract]
    [PixelShader(PixelShaderFlags.Diffuse | PixelShaderFlags.DiffuseMap)]
    [PixelShader(PixelShaderFlags.Specular)]
    public class PhongDiffuseMapPS : PhongPS
    {
        public PhongDiffuseMapPS()
        {
            Name = "PhongDiffuseMapPS";
            PhongLightingNode nPhong = (PhongLightingNode)((PSOutputNode)Result).FinalColor;
            nPhong.DiffuseMap = true;

            Texture tDiffuse = Texture.Diffuse;
            Sampler sDiffuseSampler = Sampler.MinMagMipLinearWrap;
            nPhong.DiffuseMapSamplerNode = new TextureSampleNode
            {
                Coordinates = new ConstantNode { Value = InputStruct[Param.SemanticVariables.Texture] },
                Texture = tDiffuse,
                Sampler = sDiffuseSampler
            };
            Add(tDiffuse);
            Add(sDiffuseSampler);
        }
    }

    public class PhongCubeMapPS : PhongPS
    {
        public PhongCubeMapPS()
        {
            Name = "PhongCubeMapPS";
            KeyPart =  new TechniqueKey(ps: PixelShaderFlags.Diffuse | PixelShaderFlags.CubeMap | PixelShaderFlags.CubeMap);

            var input = PhongCubeMapVS.VSOut;
            input.Name = "input";
            InputStruct = input;

            PhongLightingNode nPhong = (PhongLightingNode)((PSOutputNode)Result).FinalColor;
            nPhong.DiffuseMap = true;
            nPhong.CubeMap = true;

            Texture tDiffuse = Texture.CubeMap;
            Sampler sDiffuseSampler = Sampler.MinMagMipLinearWrap;
            nPhong.DiffuseMapSamplerNode = new TextureSampleNode
            {
                Coordinates = new ConstantNode { Value = InputStruct[Param.SemanticVariables.Texture] },
                Texture = tDiffuse,
                Sampler = sDiffuseSampler
            };
            Add(tDiffuse);
            Add(sDiffuseSampler);
        }
    }
}

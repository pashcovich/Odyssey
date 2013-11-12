using Odyssey.Content.Shaders;
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
    public class PhongVS : Shader
    {
        public PhongVS()
        {
            Name = "PhongVS";
            Type = ShaderType.Vertex;
            KeyPart = new TechniqueKey(vs: VertexShaderFlags.Position | VertexShaderFlags.Normal | VertexShaderFlags.TextureUV);
            FeatureLevel = FeatureLevel.VS_4_0_Level_9_1;
            EnableSeparators = true;
            InputStruct = Struct.VertexPositionNormalTexture;
            OutputStruct = VSOut;

            ConstantBuffer cbFrame = ConstantBuffer.CBPerFrame;
            ConstantBuffer cbInstance = CBPerInstance;
            Add(cbFrame);
            Add(cbInstance);

            IVariable position = InputStruct[Param.SemanticVariables.ObjectPosition];
            IVariable normal = InputStruct[Param.SemanticVariables.Normal];
            IVariable texture = InputStruct[Param.SemanticVariables.Texture];

            CastNode nV3toV4 = new CastNode
            {
                Input = new ConstantNode { Value = position },
                Output = new Vector { Type = Shaders.Type.Float4, Name = "vPosition", Swizzle = new[] { Swizzle.X, Swizzle.Y, Swizzle.Z, Swizzle.Null } },
                Mask = new Vector { Type = Shaders.Type.Float4, Value = new float[]{0,0,0,1}},
                IsVerbose = true
            };

            MatrixMultiplyNode mulPosWVP = new MatrixMultiplyNode
            {
                Input1 = nV3toV4,
                Input2 = MatrixMultiplyNode.WorldViewProjection,
            };

            MatrixMultiplyNode mulWP = new MatrixMultiplyNode
            {
                Input1 = nV3toV4,
                Input2 = new ConstantNode { Value = Matrix.World },
            };

            MatrixMultiplyNode mulNormal = new MatrixMultiplyNode
            {
                Input1 = new ConstantNode { Value = normal },
                Input2 = CastNode.WorldInverseFloat3x3
            };

            Result = new PhongVSOutputNode
            {
                Position = mulPosWVP,
                WorldPosition = mulWP,
                Normal = mulNormal,
                Texture = new ConstantNode { Value = texture },
                Output = OutputStruct
            };
        }

        public static Struct VSOut
        {
            get
            {
                Struct vpt = new Struct()
                {
                    CustomType = CustomType.VSOut,
                    Name = "output",
                };
                vpt.Add(Vector.ClipPosition); 
                vpt.Add(Vector.WorldPosition4);
                vpt.Add(Vector.Normal);
                vpt.Add(Vector.TextureUV);
                return vpt;
            }
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

                cbStatic.Add(Struct.Material);
                cbStatic.Add(Struct.PointLight0);
                return cbStatic;
            }
        }

        public static ConstantBuffer CBPerInstance
        {
            get
            {
                ConstantBuffer cbInstance = ConstantBuffer.CBPerInstance;
                cbInstance.Add(Matrix.WorldInverse);
                return cbInstance;
            }
        }
    }

    public class PhongCubeMapVS : PhongVS
    {
        public PhongCubeMapVS()
        {
            Name = "PhongCubeMapVS";
            KeyPart = new TechniqueKey(vs: VertexShaderFlags.Position | VertexShaderFlags.Normal | VertexShaderFlags.TextureUVW);
            InputStruct = Struct.VertexPositionNormalTextureUVW;
            OutputStruct = VSOut;

            var outputNode = (PhongVSOutputNode)Result;
            outputNode.Texture = new ConstantNode { Value = InputStruct[Param.SemanticVariables.Texture] };
        }

        public new static Struct VSOut
        {
            get
            {
                Struct vpt = new Struct()
                {
                    CustomType = CustomType.VSOut,
                    Name = "output",
                };
                vpt.Add(Vector.ClipPosition);
                vpt.Add(Vector.WorldPosition4);
                vpt.Add(Vector.Normal);
                vpt.Add(Vector.TextureUVW);
                return vpt;
            }
        }
    }


}

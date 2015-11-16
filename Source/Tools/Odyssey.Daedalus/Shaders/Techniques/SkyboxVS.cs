using Odyssey.Daedalus.Shaders.Nodes;
using Odyssey.Daedalus.Shaders.Nodes.Math;
using Odyssey.Daedalus.Shaders.Nodes.Operators;
using Odyssey.Daedalus.Shaders.Structs;
using Odyssey.Engine;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Shaders;

namespace Odyssey.Daedalus.Shaders.Techniques
{
    public class SkyboxVS : Shader
    {
        public SkyboxVS()
        {
            Name = "SkyboxVS";
            Type = ShaderType.Vertex;
            KeyPart = new TechniqueKey(vs: VertexShaderFlags.Position | VertexShaderFlags.Normal | VertexShaderFlags.TextureUVW);
            FeatureLevel = FeatureLevel.VS_4_0_Level_9_1;
            EnableSeparators = true;
            InputStruct = Struct.VertexPositionNormalTextureUVW;
            OutputStruct = VSOutput;

            Add(CBStatic);
            Add(Structs.ConstantBuffer.CBPerFrame);

            IVariable position = InputStruct[Param.SemanticVariables.ObjectPosition];
            IVariable texture = InputStruct[Param.SemanticVariables.Texture];

            CastNode nV3toV4 = new CastNode
            {
                Input = new SwizzleNode { Input = new ReferenceNode {Value =  position}, Swizzle = new[] { Swizzle.X, Swizzle.Y, Swizzle.Z, Swizzle.Null } },
                Output = new Vector { Type = Shaders.Type.Float4, Name = "vPosition" },
                Mask = new[] { "0", "0", "0", "1" },
                IsVerbose = true
            };

            MatrixMultiplyNode mulPosWVP = new MatrixMultiplyNode
            {
                Input1 = nV3toV4,
                Input2 = MatrixMultiplyNode.WorldViewProjection,
                Output = new Vector { Type = Shaders.Type.Float4, Name="vSkyBoxPosition",},
            };

            Result = new PhongVSOutputNode
            {
                Position = new SwizzleNode { Input = mulPosWVP, Swizzle = new[] { Swizzle.X, Swizzle.Y, Swizzle.W, Swizzle.W } },
                Texture = new ReferenceNode { Value = texture },
                Output = OutputStruct
            };
        }

        internal static Struct VSOutput
        {
            get
            {
                Struct vpt = new Struct
                {
                    CustomType = CustomType.VSOut,
                    Name = "output",
                };
                vpt.Add(Vector.ClipPosition);
                vpt.Add(Vector.TextureUVW);
                return vpt;
            }
        }

        public static Structs.ConstantBuffer CBStatic
        {
            get
            {
                Structs.ConstantBuffer cbStatic = new Structs.ConstantBuffer()
                {
                    Name = Param.ConstantBuffer.Static,
                    CbUpdateType = CBUpdateType.InstanceStatic,
                };

                cbStatic.Add(Matrix.EntityWorld);

                return cbStatic;
            }
        }

        public static Structs.ConstantBuffer CBPerFrame
        {
            get
            {
                Structs.ConstantBuffer cbFrame = new Structs.ConstantBuffer
                {
                    Name = Param.ConstantBuffer.PerFrame,
                    CbUpdateType = CBUpdateType.SceneFrame
                };
                cbFrame.Add(Matrix.CameraView);
                cbFrame.Add(Matrix.CameraProjection);

                return cbFrame;
            }
        }
    }
}

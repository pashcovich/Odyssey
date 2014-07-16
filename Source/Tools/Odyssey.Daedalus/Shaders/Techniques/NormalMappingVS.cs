using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Daedalus.Shaders.Nodes;
using Odyssey.Daedalus.Shaders.Nodes.Math;
using Odyssey.Daedalus.Shaders.Nodes.Operators;
using Odyssey.Daedalus.Shaders.Structs;
using Odyssey.Engine;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Shaders;
using ConstantBuffer = Odyssey.Daedalus.Shaders.Structs.ConstantBuffer;

namespace Odyssey.Daedalus.Shaders.Techniques
{
    public class NormalMappingVS : Shader
    {
        public NormalMappingVS()
        {
            Name = "NormalMappingVS";
            Type = ShaderType.Vertex;
            KeyPart = new TechniqueKey(vs: VertexShaderFlags.Position | VertexShaderFlags.Normal | VertexShaderFlags.TextureUV | VertexShaderFlags.Tangent);
            FeatureLevel = FeatureLevel.VS_4_0_Level_9_1;
            EnableSeparators = true;
            InputStruct = Struct.VertexPositionNormalTextureTangent;
            OutputStruct = Struct.VertexPositionNormalTextureTangentOut;

            Structs.ConstantBuffer cbFrame = ConstantBuffer.CBPerFrame;
            Structs.ConstantBuffer cbInstance = CBPerInstance;
            Add(cbFrame);
            Add(cbInstance);

            IVariable position = InputStruct[Param.SemanticVariables.ObjectPosition];
            IVariable normal = InputStruct[Param.SemanticVariables.Normal];
            IVariable texture = InputStruct[Param.SemanticVariables.Texture];
            IVariable tangent = InputStruct[Param.SemanticVariables.Tangent];

            CastNode mWorld3x3 = CastNode.WorldFloat3x3;

            CastNode nV3toV4 = new CastNode
            {
                Input = new SwizzleNode { Input = new ReferenceNode { Value = position }, Swizzle = new[] { Swizzle.X, Swizzle.Y, Swizzle.Z, Swizzle.Null } },
                Output = new Vector { Type = Shaders.Type.Float4, Name = "vPosition" },
                Mask = new[] { "0", "0", "0", "1" },
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
                Input2 = new ReferenceNode { Value = Matrix.EntityWorld },
            };

            MatrixMultiplyNode mulNormal = new MatrixMultiplyNode
            {
                Input1 = new ReferenceNode { Value = normal },
                Input2 = mWorld3x3
            };

            MatrixMultiplyNode mulTangent = new MatrixMultiplyNode()
            {
                Input1 = new SwizzleNode { Input = new ReferenceNode { Value = tangent }, Swizzle = new[] { Swizzle.X, Swizzle.Y, Swizzle.Z, Swizzle.Null } },
                Input2 = mWorld3x3
            };

            CastNode tangentW = new CastNode
            {
                Input = mulTangent,
                Output = new Vector {Type = Shaders.Type.Float4, Name = "vTangent"},
                Mask = new[] {tangent.FullName + ".w"}
            };

            Result = new VSNormalTexturedTangentOutputNode()
            {
                Position = mulPosWVP,
                WorldPosition = mulWP,
                Normal = mulNormal,
                Texture = new ReferenceNode { Value = texture },
                Tangent = tangentW,
                Output = OutputStruct
            };
        }

        public static Structs.ConstantBuffer CBPerInstance
        {
            get
            {
                Structs.ConstantBuffer cbInstance = ConstantBuffer.CBPerInstance;
                return cbInstance;
            }
        }

    }
}

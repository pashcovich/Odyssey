using System.Collections.Generic;
using Odyssey.Daedalus.Shaders.Nodes;
using Odyssey.Daedalus.Shaders.Nodes.Functions;
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
    [DataContract]
    public class SpriteVS : Shader
    {
        public SpriteVS()
        {
            Name = "SpriteVS";
            Type = ShaderType.Vertex;
            FeatureLevel = FeatureLevel.VS_4_0_Level_9_1;
            KeyPart = new TechniqueKey(vs: VertexShaderFlags.Position | VertexShaderFlags.TextureUV | VertexShaderFlags.Normal);
            EnableSeparators = true;
            InputStruct = Struct.VertexPositionNormalTexture;
            OutputStruct = VSOut;

            Structs.ConstantBuffer cbStatic = CBStatic;
            Structs.ConstantBuffer cbInstance = CBPerInstance;
            Add(cbStatic);
            Add(cbInstance);

            ClipSpaceTransformNode cstNode = new ClipSpaceTransformNode
            {
                Position = new ReferenceNode { Value = InputStruct[Param.SemanticVariables.ObjectPosition] },
                InstancePosition = cbInstance[Param.Vectors.SpritePosition],
                Size = cbInstance[Param.Vectors.SpriteSize],
                ScreenSize = cbStatic[Param.Vectors.ViewportSize]
            };

            CustomOutputNode outputNode = new CustomOutputNode() { Output = OutputStruct };
            outputNode.RegisterField(Vector.ClipPosition, cstNode, Shaders.Type.Float4);
            outputNode.RegisterField(Vector.TextureUV, new ReferenceNode { Value = InputStruct[Param.SemanticVariables.Texture] }, Shaders.Type.Float2);

            Result = outputNode;
        }

        public static Struct VSOut
        {
            get
            {
                Struct vpt = new Struct
                {
                    CustomType = CustomType.VSOut,
                    Name = "output",
                };
                vpt.Add(Vector.ClipPosition);
                vpt.Add(Vector.TextureUV);
                return vpt;
            }
        }

        public static Structs.ConstantBuffer CBStatic
        {
            get
            {
                Structs.ConstantBuffer cbStatic = new Structs.ConstantBuffer
                {
                    Name = Param.ConstantBuffer.Static,
                    CbUpdateType = CBUpdateType.SceneStatic,
                };

                cbStatic.Add(Vector.ViewportSize);
                return cbStatic;
            }
        }

        public static Structs.ConstantBuffer CBPerInstance
        {
            get
            {
                Structs.ConstantBuffer cbInstance = new Structs.ConstantBuffer();
                cbInstance.Add(Vector.SpritePosition);
                cbInstance.Add(Vector.SpriteSize);
                return cbInstance;
            }
        }
    }
}

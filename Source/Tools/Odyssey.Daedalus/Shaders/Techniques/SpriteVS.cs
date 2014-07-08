using System.Collections.Generic;
using Odyssey.Engine;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Shaders;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Math;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Operators;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Functions;
using Odyssey.Tools.ShaderGenerator.Shaders.Structs;
using System.Runtime.Serialization;
using ConstantBuffer = Odyssey.Tools.ShaderGenerator.Shaders.Structs.ConstantBuffer;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Techniques
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

            ConstantBuffer cbStatic = CBStatic;
            ConstantBuffer cbInstance = CBPerInstance;
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

        public static ConstantBuffer CBStatic
        {
            get
            {
                ConstantBuffer cbStatic = new ConstantBuffer
                {
                    Name = Param.ConstantBuffer.Static,
                    UpdateType = UpdateType.SceneStatic,
                };

                cbStatic.Add(Vector.ViewportSize);
                return cbStatic;
            }
        }

        public static ConstantBuffer CBPerInstance
        {
            get
            {
                ConstantBuffer cbInstance = new ConstantBuffer();
                cbInstance.Add(Vector.SpritePosition);
                cbInstance.Add(Vector.SpriteSize);
                return cbInstance;
            }
        }
    }
}

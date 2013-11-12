using Odyssey.Engine;
using Odyssey.Graphics.Materials;
using Odyssey.Graphics.Rendering;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Operators;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Functions;
using Odyssey.Tools.ShaderGenerator.Shaders.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Techniques
{
    [DataContract]
    [VertexShader(VertexShaderFlags.Position)]
    [VertexShader(VertexShaderFlags.TextureUV)]
    public class SpriteVS : Shader
    {
        public SpriteVS()
        {
            Name = "SpriteVS";
            Type = ShaderType.Vertex;
            FeatureLevel = FeatureLevel.VS_4_0_Level_9_1;
            KeyPart = new TechniqueKey(vs: VertexShaderFlags.Position | VertexShaderFlags.TextureUV);
            EnableSeparators = true;
            InputStruct = Struct.VertexPositionNormalTexture;
            OutputStruct = VSOut;

            ConstantBuffer cbStatic = CBStatic;
            ConstantBuffer cbInstance = CBPerInstance;
            Add(cbStatic);
            Add(cbInstance);


            ClipSpaceTransformNode cstNode = new ClipSpaceTransformNode
            {
                Position = new ConstantNode { Value = InputStruct[Param.SemanticVariables.ObjectPosition] },
                InstancePosition = cbInstance[Param.SemanticVariables.WorldPosition],
                Size = cbInstance["fSize"],
                ScreenSize = cbStatic[Param.Vectors.ScreenSize]
            };

            Result = new VSTexturedOutputNode
            {
                Position = cstNode,
                Texture = new ConstantNode { Value = InputStruct[Param.SemanticVariables.Texture] },
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

                cbStatic.Add(Matrix.View);
                cbStatic.Add(Matrix.Projection);
                cbStatic.Add(Vector.ScreenSize);
                return cbStatic;
            }
        }

        public static ConstantBuffer CBPerInstance
        {
            get
            {
                ConstantBuffer cbInstance = ConstantBuffer.CBPerInstance;
                cbInstance.Add(Vector.WorldPosition);
                cbInstance.Add(Float.SpriteSize);
                return cbInstance;
            }
        }
    }
}

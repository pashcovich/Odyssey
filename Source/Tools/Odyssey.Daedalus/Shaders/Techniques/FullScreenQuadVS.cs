using Odyssey.Engine;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Shaders;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Functions;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Operators;
using Odyssey.Tools.ShaderGenerator.Shaders.Structs;
using ConstantBuffer = Odyssey.Tools.ShaderGenerator.Shaders.Structs.ConstantBuffer;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Techniques
{
    public class FullScreenQuadVS : Shader
    {
        public FullScreenQuadVS()
        {
            Name = "FullScreenQuadVS";
            Type = ShaderType.Vertex;
            FeatureLevel = FeatureLevel.VS_4_0_Level_9_1;
            KeyPart = new TechniqueKey(vs: VertexShaderFlags.Position | VertexShaderFlags.TextureUV | VertexShaderFlags.Normal);
            EnableSeparators = true;
            InputStruct = Struct.VertexPositionNormalTexture;
            OutputStruct = SpriteVS.VSOut;

            ConstantBuffer cbStatic = SpriteVS.CBStatic;
            Add(cbStatic);

            DeclarationNode nClip = ClipSpaceTransformNode.FullScreenNode(InputStruct, cbStatic[Param.Vectors.ViewportSize]);

            CustomOutputNode outputNode = new CustomOutputNode() { Output = OutputStruct };
            outputNode.RegisterField(Vector.ClipPosition, nClip, Shaders.Type.Float4);
            outputNode.RegisterField(Vector.TextureUV, new ReferenceNode { Value = InputStruct[Param.SemanticVariables.Texture] }, Shaders.Type.Float2);

            Result = outputNode;

        }
    }
}
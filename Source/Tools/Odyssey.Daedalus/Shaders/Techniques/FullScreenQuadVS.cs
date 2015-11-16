using Odyssey.Daedalus.Shaders.Nodes;
using Odyssey.Daedalus.Shaders.Nodes.Functions;
using Odyssey.Daedalus.Shaders.Nodes.Operators;
using Odyssey.Daedalus.Shaders.Structs;
using Odyssey.Engine;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Shaders;

namespace Odyssey.Daedalus.Shaders.Techniques
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

            Structs.ConstantBuffer cbStatic = SpriteVS.CBStatic;
            Add(cbStatic);

            DeclarationNode nClip = ClipSpaceTransformNode.FullScreenNode(InputStruct, cbStatic[Param.Vectors.ViewportSize]);

            CustomOutputNode outputNode = new CustomOutputNode() { Output = OutputStruct };
            outputNode.RegisterField(Vector.ClipPosition, nClip, Shaders.Type.Float4);
            outputNode.RegisterField(Vector.TextureUV, new ReferenceNode { Value = InputStruct[Param.SemanticVariables.Texture] }, Shaders.Type.Float2);

            Result = outputNode;

        }
    }
}
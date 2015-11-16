using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Shaders;
using System.Text;

namespace Odyssey.Daedalus.Shaders.Methods
{
    public class ClipSpaceTransform : MethodBase
    {
        internal const string size = "size";

        public ClipSpaceTransform()
        {
            Name = "ClipSpaceTransform";

            RegisterSignature(new MethodSignature(this, new TechniqueKey(VertexShaderFlags.Position | VertexShaderFlags.TextureUV | VertexShaderFlags.Normal),
                new[] { HLSLTypes.Float3, HLSLTypes.Float3, HLSLTypes.Float2, HLSLTypes.Float2 },
                new[] { Vectors.Position, Vectors.PositionInstance, Floats.SpriteSize, Vectors.ScreenSize }, Type.Float4));
        }

        public override string Body
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("{");
                sb.AppendLine("\tfloat4 pClip;");
                sb.AppendLine(string.Format("\tpClip.x = 2*({0}.x * {1}.x + {2}.x)/{3}.x -1;", Vectors.Position, Floats.SpriteSize, Vectors.PositionInstance, Vectors.ScreenSize));
                sb.AppendLine(string.Format("\tpClip.y = 1 - 2 * ({0}.y * {1}.y + {2}.y) / {3}.y;", Vectors.Position, Floats.SpriteSize, Vectors.PositionInstance, Vectors.ScreenSize));
                sb.AppendLine("\tpClip.z = 0;");
                sb.AppendLine("\tpClip.w = 1;");
                sb.AppendLine("\treturn pClip;");
                sb.AppendLine("}");
                return sb.ToString();
            }
        }

    }
}
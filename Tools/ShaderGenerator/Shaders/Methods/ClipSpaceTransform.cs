using Odyssey.Tools.ShaderGenerator.Shaders.Nodes;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Methods
{
    public class ClipSpaceTransform : MethodBase
    {
        internal const string size = "size";
        



        public override string Signature
        {
            get
            {
                return string.Format("{0} {1}({2} {3}, {4} {5}, {6} {7}, {8} {9})",
                    Mapper.Map(ReturnType),
                    Name,
                    Mapper.Map(Type.Float3), Vectors.Position,
                    Mapper.Map(Type.Float3), Vectors.PositionInstance,
                    Mapper.Map(Type.Float), Floats.SpriteSize,
                    Mapper.Map(Type.Float2), Vectors.ScreenSize);
            }
        }

        public ClipSpaceTransform()
        {
            Name = "ClipSpaceTransform";
            ReturnType = Type.Float4;
        }

        public string Call(Vector position, Vector pInstance, Vector size, Vector screenSize)
        {
            Contract.Requires<ArgumentException>(position.Type == Type.Float3);
            Contract.Requires<ArgumentException>(pInstance.Type == Type.Float3);
            Contract.Requires<ArgumentException>(size.Type == Type.Float);
            Contract.Requires<ArgumentException>(screenSize.Type == Type.Float2);
            return Call(new[] { position.FullName, pInstance.FullName, size.FullName, screenSize.FullName });
        }

        public override string Body
        {
            get {

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("{");
                sb.AppendLine("\tfloat4 pClip;");
                sb.AppendLine(string.Format("\tpClip.x = 2*({0}.x * {1} + {2}.x)/{3}.x -1;", Vectors.Position, Floats.SpriteSize, Vectors.PositionInstance, Vectors.ScreenSize));
                sb.AppendLine(string.Format("\tpClip.y = 1 - 2 * ({0}.y * {1} + {2}.y) / {3}.y;", Vectors.Position, Floats.SpriteSize, Vectors.PositionInstance, Vectors.ScreenSize));
                sb.AppendLine("\tpClip.z = 0;");
                sb.AppendLine("\tpClip.w = 1;");
                sb.AppendLine("\treturn pClip;");
                sb.AppendLine("}");
                return sb.ToString();
            }
        }
    }
}

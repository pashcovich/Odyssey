using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Structs
{
    public class VertexPositionColor : StructBase
    {
        public VertexPositionColor()
        {
            Type = Shaders.Type.Struct;
            Name = "VSInput";
            AddField(Field.Position);
            AddField(Field.Color);
        }
    }
}

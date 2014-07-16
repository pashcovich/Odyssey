using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Shaders;
using SharpDX;

namespace Odyssey.Daedalus.Shaders.Methods
{
    public class AdjustSaturation : MethodBase
    {
        public AdjustSaturation()
        {
            Name = "AdjustSaturation";

            RegisterSignature(new MethodSignature(this, new TechniqueKey(ps: PixelShaderFlags.DiffuseMap),
                new[] { HLSLTypes.Float4, HLSLTypes.Float},
                new[] { Colors.Color, Floats.Saturation}, Type.Float4));
        }

        public override string Body
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                //float grey = dot(color, float3(0.3, 0.59, 0.11));
                //return lerp(grey, color, saturation);
                sb.AppendLine("{");
                sb.AppendLine(string.Format("\tfloat grey = dot({0}, float3(0.3f, 0.59f, 0.11f));", Colors.Color));
                sb.AppendLine(string.Format("\treturn lerp(grey, {0}, {1});", Colors.Color, Floats.Saturation));
                sb.AppendLine("}");
                return sb.ToString();
            }
        }
    }
}

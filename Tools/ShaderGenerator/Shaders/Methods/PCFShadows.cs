using Odyssey.Engine;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes;
using Odyssey.Tools.ShaderGenerator.Shaders.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Methods
{
	public class PCFShadows : MethodBase
	{
        public IMethod TexOffset { get; set; }

		public PCFShadows()
		{
			Name = "PCFShadows";
			ReturnType = Type.Float;
            TexOffset = new TexOffset();
		}

		public override string Signature
		{
			get
			{
				return string.Format("{0} {1}({2} {3}, {4} {5}, {6} {7}, {8} {9})",
				  Mapper.Map(ReturnType),
				  Name,
				  Mapper.Map(CustomType.Material), Structs.material,
				  Mapper.Map(Type.Float4), Vectors.vShadowProjection,
				  Mapper.Map(Type.Texture2D), Textures.tShadowMap,
				  Mapper.Map(Type.SamplerComparisonState), Samplers.sShadowMap);
			}
		}

		public override string Body
		{
			get
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendLine("{");
				sb.AppendLine(string.Format("\t{0}.xyz /= {0}.w;", Vectors.vShadowProjection));
				//if position is not visible to the light - dont illuminate it results in hard light frustum
				sb.AppendLine(string.Format("\tif ({0}.x < -1.0f || {0}.x > 1.0f ||\n\t\t{0}.y < -1.0f || {0}.y > 1.0f ||\n\t\t{0}.z < 0.0f || {0}.z > 1.0f)\n\t\treturn {1}.{2};",
					Vectors.vShadowProjection, Structs.material, Param.Material.Ambient));
				//transform clip space coords to texture space coords (-1:1 to 0:1)
				sb.AppendLine(string.Format("\t{0}.x = {0}.x / 2 + 0.5;", Vectors.vShadowProjection));
				sb.AppendLine(string.Format("\t{0}.y = {0}.y / -2 + 0.5;", Vectors.vShadowProjection));
				sb.AppendLine(string.Format("\t{0}.z -= {1};", Vectors.vShadowProjection, Floats.ShadowBias));
				//PCF sampling for shadow map
				sb.AppendLine("\tfloat sum = 0;");
				sb.AppendLine("\tfloat x,y;");
				//perform PCF filtering on a 4 x 4 texel neighborhood
				sb.AppendLine("\tfor (y = -1.5; y <= 1.5; y += 1.0)");
				sb.AppendLine("\t{");
				sb.AppendLine("\t\tfor (x = -1.5; x <= 1.5; x += 1.0)");
                sb.AppendLine("\t\t{");
				sb.AppendLine(string.Format("\t\t\tsum += {0}.SampleCmpLevelZero({1}, {2}.xy + {3}, {2}.z);",
					Textures.tShadowMap, Samplers.sShadowMap, Vectors.vShadowProjection, TexOffset.Call("x","y")));
                sb.AppendLine("\t\t}");
				sb.AppendLine("\t}");
				sb.AppendLine("\treturn sum / 16.0f;");
				sb.AppendLine("}");

				return sb.ToString();
			}
		}


	}
}

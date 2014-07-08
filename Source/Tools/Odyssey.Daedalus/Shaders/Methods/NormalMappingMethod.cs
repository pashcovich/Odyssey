using System.Text;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Shaders;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Methods
{
    public class NormalMappingMethod : MethodBase
    {
        public NormalMappingMethod()
        {
            Name = "ApplyNormalMapping";
            
            // Default
            RegisterSignature(new MethodSignature(this, new TechniqueKey(ps: PixelShaderFlags.Diffuse | PixelShaderFlags.DiffuseMap | PixelShaderFlags.Specular | PixelShaderFlags.NormalMap),
                new[] { HLSLTypes.Float3, HLSLTypes.Float4, HLSLTypes.Float4},
                new[] { Vectors.Normal, Vectors.Tangent, Colors.NormalMap }, Type.Float3));
        }

        public override string Body
        {
            get { 
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("{");
                //sb.AppendLine(string.Format("\t{0} = normalize({0});",Vectors.Normal));
                //sb.AppendLine(string.Format("\t{0} T = {1}.xyz;", HLSLTypes.Float3, Vectors.Tangent));
                sb.AppendLine(string.Format("\t{0} T = normalize({1} - {2} * dot({2}, {1}));", HLSLTypes.Float3, Vectors.Tangent, Vectors.Normal));
                sb.AppendLine(string.Format("\t{0} {1} = (2.0f * {2}.rgb) - 1.0f;", HLSLTypes.Float3, Vectors.NormalMap, Colors.NormalMap));
                
                // Tangent should already be normalised
                sb.AppendLine(string.Format("\t{0} {1} = cross({2}, T) * {3}.w;",
                    HLSLTypes.Float3, Vectors.Bitangent, Vectors.Normal, Vectors.Tangent));

                sb.AppendLine(string.Format("\t{0} {1} = {0}(T, {2}, {3});",
                    HLSLTypes.Float3x3, Matrices.TangentSpace,Vectors.Bitangent, Vectors.Normal));

                sb.AppendLine(string.Format("\treturn normalize(mul({0}, {1}));", Vectors.NormalMap, Matrices.TangentSpace));

                sb.AppendLine("}");
                return sb.ToString();
            } 
        }
    }
}

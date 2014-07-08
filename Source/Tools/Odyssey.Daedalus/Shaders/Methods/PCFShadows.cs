using Odyssey.Engine;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Shaders;
using System.Text;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Methods
{
    public class PCFShadows : MethodBase
    {
        public PCFShadows()
        {
            Name = Methods.PCFShadows;

            // Shadows
            RegisterSignature(new MethodSignature(this, new TechniqueKey(ps: PixelShaderFlags.Diffuse | PixelShaderFlags.Specular | PixelShaderFlags.Shadows | PixelShaderFlags.ShadowMap, sm: ShaderModel.SM_5_0),
                new[] { HLSLTypes.Float4, Type.Texture2D.ToString(), Type.SamplerComparisonState.ToString() },
                new[] { Vectors.ShadowProjection, Textures.ShadowMap, Samplers.ShadowMap }, Type.Float));

            AddReference(new MethodReference(new TexOffset(), new[] { "x", "y" }));
        }

        public override string Body
        {
            get
            {
                MethodReference texOffset = GetReference(Methods.TexOffset);

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("{");
                sb.AppendLine(string.Format("\t{0}.xyz /= {0}.w;", Vectors.ShadowProjection));
                //if position is not visible to the light - dont illuminate it results in hard light frustum
                sb.AppendLine(string.Format("\tif ({0}.x < -1.0f || {0}.x > 1.0f ||\n\t\t{0}.y < -1.0f || {0}.y > 1.0f ||\n\t\t{0}.z < 0.0f || {0}.z > 1.0f)\n\t\treturn {1}.{2};",
                    Vectors.ShadowProjection, Structs.Material, Param.Material.Ambient));
                //transform clip space coords to texture space coords (-1:1 to 0:1)
                sb.AppendLine(string.Format("\t{0}.x = {0}.x / 2 + 0.5;", Vectors.ShadowProjection));
                sb.AppendLine(string.Format("\t{0}.y = {0}.y / -2 + 0.5;", Vectors.ShadowProjection));
                sb.AppendLine(string.Format("\t{0}.z -= {1};", Vectors.ShadowProjection, Floats.ShadowBias));
                //PCF sampling for shadow map
                sb.AppendLine("\tfloat sum = 0;");
                sb.AppendLine("\tfloat x,y;");
                //perform PCF filtering on a 4 x 4 texel neighborhood
                sb.AppendLine("\tfor (y = -1.5; y <= 1.5; y += 1.0)");
                sb.AppendLine("\t{");
                sb.AppendLine("\t\tfor (x = -1.5; x <= 1.5; x += 1.0)");
                sb.AppendLine("\t\t{");
                sb.AppendLine(string.Format("\t\t\tsum += {0}.SampleCmpLevelZero({1}, {2}.xy + {3}, {2}.z);",
                    Textures.ShadowMap, Samplers.ShadowMap, Vectors.ShadowProjection, texOffset.Method.Call("x", "y")));
                sb.AppendLine("\t\t}");
                sb.AppendLine("\t}");
                sb.AppendLine("\treturn sum / 16.0f;");
                sb.AppendLine("}");

                return sb.ToString();
            }
        }

        //public override string Signature
        //{
        //    get
        //    {
        //        return string.Format("{0} {1}({2} {3}, {4} {5}, {6} {7}, {8} {9})",
        //          Mapper.Map(ReturnType),
        //          Name,
        //          Mapper.Map(CustomType.Material), Structs.Material,
        //          Mapper.Map(Type.Float4), Vectors.vShadowProjection,
        //          Mapper.Map(Type.Texture2D), Textures.tShadowMap,
        //          Mapper.Map(Type.SamplerComparisonState), Samplers.sShadowMap);
        //    }
        //}
    }
}
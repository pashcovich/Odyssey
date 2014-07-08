using System.Windows.Media;
using Odyssey.Engine;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Shaders;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes;
using Odyssey.Tools.ShaderGenerator.Shaders.Structs;
using System.Text;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Methods
{
    public class PhongLighting : MethodBase
    {
        public PhongLighting()
        {
            Name = "PhongLighting";

            // Default
            RegisterSignature(new MethodSignature(this, new TechniqueKey(ps: PixelShaderFlags.Diffuse | PixelShaderFlags.Specular),
                new[] { CustomType.PointLight, CustomType.Material, HLSLTypes.Float3, HLSLTypes.Float3, HLSLTypes.Float3 },
                new[] { Structs.Light, Structs.Material, Vectors.LightDirection, Vectors.ViewDirection, Vectors.Normal }, Type.Float4));

            // DiffuseMap
            RegisterSignature(new MethodSignature(this, new TechniqueKey(ps: PixelShaderFlags.Diffuse | PixelShaderFlags.Specular | PixelShaderFlags.DiffuseMap),
                new[] { CustomType.PointLight, CustomType.Material, HLSLTypes.Float3, HLSLTypes.Float3, HLSLTypes.Float3, HLSLTypes.Float4 },
                new[] { Structs.Light, Structs.Material, Vectors.LightDirection, Vectors.ViewDirection, Vectors.Normal, Colors.DiffuseMap }, Type.Float4));

            // CubeMap
            RegisterSignature(new MethodSignature(this, new TechniqueKey(ps: PixelShaderFlags.Diffuse | PixelShaderFlags.Specular | PixelShaderFlags.CubeMap),
                new[] { CustomType.PointLight, CustomType.Material, HLSLTypes.Float3, HLSLTypes.Float3, HLSLTypes.Float3, HLSLTypes.Float4 },
                new[] { Structs.Light, Structs.Material, Vectors.LightDirection, Vectors.ViewDirection, Vectors.Normal, Colors.DiffuseMap }, Type.Float4));

            // Shadows
            RegisterSignature(new MethodSignature(this, new TechniqueKey(ps: PixelShaderFlags.Diffuse | PixelShaderFlags.Specular | PixelShaderFlags.Shadows | PixelShaderFlags.ShadowMap, sm: ShaderModel.SM_5_0),
                new[] { CustomType.PointLight, CustomType.Material, HLSLTypes.Float3, HLSLTypes.Float3, HLSLTypes.Float3, HLSLTypes.Float4 },
                new[] { Structs.Light, Structs.Material, Vectors.LightDirection, Vectors.ViewDirection, Vectors.Normal, Vectors.ShadowProjection }, Type.Float4));
        }

        public override string Body
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("{");
                sb.AppendLine(string.Format("\tfloat3 L = -{0};", Vectors.LightDirection));
                sb.AppendLine(string.Format("\tfloat3 V = {0};", Vectors.ViewDirection));
                sb.AppendLine(string.Format("\tfloat3 N = {0};", Vectors.Normal));
                sb.AppendLine("\tfloat NdotL = saturate(dot(N, L));");
                if (!Shadows)
                    sb.AppendLine("\tfloat fSelfShadow = saturate(4 * NdotL);");
                sb.AppendLine(string.Format("\tfloat4 {0} = {1}.{2} * NdotL * {1}.{3} * {4}.{5};\n",
                    Colors.Diffuse, Structs.Material, Param.Material.kD, Param.Material.Diffuse,
                    Structs.Light, Param.Light.Diffuse));
                sb.AppendLine(string.Format("\tfloat4 {0} = {1}.{2} * {1}.{3};",
                    Colors.Ambient, Structs.Material, Param.Material.kA, Param.Material.Ambient));

                if (Specular)
                {
                    sb.AppendLine("\tfloat3 R = reflect(-L, N);");
                    sb.AppendLine("\tfloat RdotV = dot(R,V);");
                    sb.AppendLine(string.Format("\tfloat fSpecular = pow(max(RdotV, 0.0f), {0}.{1});",
                            Structs.Material, Param.Material.SpecularPower));
                    sb.AppendLine(string.Format("\tfloat4 {0} = {1}.{2} * fSpecular * {1}.{3};",
                        Colors.Specular, Structs.Material, Param.Material.kS, Param.Material.Specular));
                }

                if (Shadows)
                {
                    var shadowMethod = GetReference(Methods.PCFShadows);
                    sb.AppendLine(string.Format("\tfloat {0} = {1};", Floats.ShadowFactor, shadowMethod.Call()));
                }

                sb.AppendFormat("\tfloat4 color = {0} +", Colors.Ambient);

                sb.Append(Shadows ? string.Format(" + {0} * ", Floats.ShadowFactor) : " fSelfShadow *");

                if (DiffuseMap && Specular)
                    sb.AppendFormat("({0} * {1} + {2});", Colors.Diffuse, Colors.DiffuseMap, Colors.Specular);
                else if (Specular)
                    sb.AppendFormat("({0} + {1});", Colors.Diffuse, Colors.Specular);
                else
                    sb.AppendFormat("{0};", Colors.Diffuse);
                
                sb.AppendLine();
                sb.AppendLine("\treturn color;");
                sb.AppendLine("}");

                return sb.ToString();
            }
        }

        public bool CubeMap { get; internal set; }

        public bool DiffuseMap { get; internal set; }

        public bool Shadows { get; internal set; }

        public bool Specular { get; internal set; }

    }
}
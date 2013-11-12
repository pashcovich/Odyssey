using Odyssey.Engine;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes;
using Odyssey.Tools.ShaderGenerator.Shaders.Structs;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Methods
{
    public class PhongLighting : MethodBase
    {
        [SupportedType(Type.Struct)]
        public IStruct Light { get; set; }

        public MethodReference ShadowMethod { get; set; }

        public bool Specular { get; internal set; }
        public bool DiffuseMap { get; internal set; }
        public bool CubeMap { get; internal set; }
        public bool Shadows { get; internal set; }

        public PhongLighting()
        {
            Name = "PhongLighting";
            ReturnType = Type.Float4;
        }

        public override string Signature
        {
            get
            {

                string signature = string.Format("{0} {1}({2} {3}, {4} {5}, {6} {7}, {8} {9}, {10} {11}",
                        Mapper.Map(ReturnType),
                        Name,
                        Mapper.Map(Light.CustomType), Structs.light,
                        Mapper.Map(CustomType.Material), Structs.material,
                        Mapper.Map(Type.Float3), Vectors.vViewDirection,
                        Mapper.Map(Type.Float3), Vectors.vLightDirection,
                        Mapper.Map(Type.Float3), Vectors.vNormal);

                if (DiffuseMap)
                    signature += string.Format(", {0} {1}", CubeMap ? Mapper.Map(Type.Float3): Mapper.Map(Type.Float2), Vectors.vDiffuseMapCoordinates);
                else if (Shadows)
                    signature += string.Format(", {0} {1}", Mapper.Map(Type.Float4), Vectors.vShadowProjection);

                signature += ")";
                return signature;
            }
        }

        public override string Body
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("{");
                sb.AppendLine(string.Format("\tfloat3 L = -normalize({0});", Vectors.vLightDirection));
                sb.AppendLine(string.Format("\tfloat3 V = normalize({0});", Vectors.vViewDirection));
                sb.AppendLine(string.Format("\tfloat3 N = normalize({0});", Vectors.vNormal));
                sb.AppendLine("\tfloat NdotL = saturate(dot(N, L));");
                if (!Shadows)
                    sb.AppendLine("\tfloat fSelfShadow = saturate(4 * NdotL);");
                sb.AppendLine(string.Format("\tfloat4 cDiffuse = {0}.{1} * NdotL * {0}.{2} * {3}.{4};\n",
                    Structs.material, Param.Material.kD, Param.Material.Diffuse,
                    Structs.light, Param.Light.Diffuse));
                sb.AppendLine(string.Format("\tfloat4 cAmbient = {0}.{1} * {0}.{2};",
                    Structs.material, Param.Material.kA, Param.Material.Ambient));

                if (Specular)
                {
                    sb.AppendLine("\tfloat3 R = reflect(-L, N);");
                    sb.AppendLine("\tfloat RdotV = dot(R,V);");
                    sb.AppendLine(string.Format("\tfloat fSpecular = pow(max(RdotV, 0.0f), {0}.{1});",
                            Structs.material, Param.Material.SpecularPower));
                    sb.AppendLine(string.Format("\tfloat4 cSpecular = {0}.{1} * fSpecular * {0}.{2};",
                        Structs.material, Param.Material.kS, Param.Material.Specular));
                }
                if (DiffuseMap)
                    sb.AppendLine(string.Format("\tfloat4 cDiffuseMap = {0}.Sample({1}, {2});", 
                        Textures.tDiffuse, Samplers.sLinear, Vectors.vDiffuseMapCoordinates));
                if (Shadows)
                    sb.AppendLine(string.Format("\tfloat {0} = {1};", Floats.ShadowFactor, ShadowMethod.Call()));

                sb.Append("\tfloat4 color = cAmbient +");
                if (Shadows)
                    sb.Append(string.Format(" + {0} * ", Floats.ShadowFactor));
                else
                    sb.Append(" fSelfShadow *");

                if (DiffuseMap && Specular)
                    sb.Append("(cDiffuse * cDiffuseMap + cSpecular);");
                else if (Specular)
                    sb.Append("(cDiffuse + cSpecular);");
                else
                    sb.Append("cDiffuse;");

                sb.AppendLine();
                sb.AppendLine("\treturn color;");
                sb.AppendLine("}");

                return sb.ToString();
            }
        }

        public string Call(Struct light, Struct material, Vector viewDirection, Vector lightDirection, Vector normal, Vector shadowProjection = null )
        {
            Contract.Requires<ArgumentException>(light.CustomType == CustomType.PointLight);
            Contract.Requires<ArgumentException>(material.CustomType == CustomType.Material);
            Contract.Requires<ArgumentException>(viewDirection.Type == Type.Float3);
            Contract.Requires<ArgumentException>(lightDirection.Type == Type.Float3);
            Contract.Requires<ArgumentException>(normal.Type == Type.Float3);
            Contract.Requires<ArgumentException>(shadowProjection != null ? shadowProjection.Type == Type.Float4: true);
            
            List<string> arguments = new List<string>() {light.FullName, material.FullName, viewDirection.FullName, lightDirection.FullName, normal.FullName};
            if (Shadows)
                arguments.Add(shadowProjection.FullName);

            return Call(arguments.ToArray());
        }


        public string CallDiffuseMap(Struct light, Struct material, Vector viewDirection, Vector lightDirection, Vector normal, Vector diffuseMapCoordinates)
        {
            Contract.Requires<ArgumentException>(light.CustomType == CustomType.PointLight);
            Contract.Requires<ArgumentException>(material.CustomType == CustomType.Material);
            Contract.Requires<ArgumentException>(viewDirection.Type == Type.Float3);
            Contract.Requires<ArgumentException>(lightDirection.Type == Type.Float3);
            Contract.Requires<ArgumentException>(normal.Type == Type.Float3);
            Contract.Requires<ArgumentException>(diffuseMapCoordinates != null ? diffuseMapCoordinates.Type == Type.Float2 || diffuseMapCoordinates.Type == Type.Float3 : true);

            List<string> arguments = new List<string>() { light.FullName, material.FullName, viewDirection.FullName, lightDirection.FullName, normal.FullName, diffuseMapCoordinates.FullName };

            return Call(arguments.ToArray());
        }

    }
}

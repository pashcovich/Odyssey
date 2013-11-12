using Odyssey.Graphics.Materials;
using Odyssey.Tools.ShaderGenerator.Shaders.Methods;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Operators;
using Odyssey.Tools.ShaderGenerator.Shaders.Structs;
using Odyssey.Tools.ShaderGenerator.Shaders.Yaml;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Functions
{
    [YamlMapping(typeof(YamlPhongLightingNode))]
    [DataContract]
    public class PhongLightingNode : NodeBase
    {
        IVariable output;
        PhongLighting phongMethod;
        TexOffset texOffset;

        IMethod shadowMethod;

        [DataMember]
        [SupportedType(Type.Struct)]
        public INode Light { get; set; }

        [DataMember]
        [SupportedType(Type.Struct)]
        [PixelShader(PixelShaderFlags.Diffuse)]
        public INode Material { get; set; }

        [DataMember]
        [SupportedType(Type.Float3)]
        public INode ViewDirection { get; set; }

        [DataMember]
        [SupportedType(Type.Float3)]
        public INode LightDirection { get; set; }

        [DataMember]
        [SupportedType(Type.Float3)]
        [PixelShader(PixelShaderFlags.Specular)]
        public INode Normal { get; set; }

        [DataMember]
        [SupportedType(Type.Float4)]
        [VertexShader(VertexShaderFlags.TextureUV)]
        [PixelShader(PixelShaderFlags.DiffuseMap)]
        public TextureSampleNode DiffuseMapSamplerNode { get; set; }

        [DataMember]
        [SupportedType(Type.Float4)]
        [VertexShader(VertexShaderFlags.ShadowProjection)]
        [PixelShader(PixelShaderFlags.Shadows | PixelShaderFlags.ShadowMap)]
        public TextureSampleNode ShadowMapSamplerNode { get; set; }

        public override IEnumerable<IMethod> RequiredMethods
        {
            get
            {
                if (Shadows)
                {
                    yield return texOffset;
                    yield return shadowMethod;
                }
                yield return phongMethod;
            }
        }

        [DataMember]
        public bool Specular { get; set; }
        [DataMember]
        public bool DiffuseMap { get; set; }
        [DataMember]
        public bool Shadows { get; set; }
        [DataMember]
        public bool CubeMap { get; set; }

        [SupportedType(Type.Float4)]
        public override IVariable Output
        {
            get
            {
                if (output == null)
                {
                    string id = string.Empty;
                    Output = new Vector
                    {
                        Name = "cFinal",
                        Type = Type.Float4
                    };
                }
                return output;
            }
            set { output = value; }
        }

        public override IEnumerable<INode> DescendantNodes
        {
            get
            {
                yield return Light;
                foreach (var node in Light.DescendantNodes)
                    yield return node;

                yield return Material;
                foreach (var node in Material.DescendantNodes)
                    yield return node;

                yield return ViewDirection;
                foreach (var node in ViewDirection.DescendantNodes)
                    yield return node;

                yield return LightDirection;
                foreach (var node in LightDirection.DescendantNodes)
                    yield return node;

                yield return Normal;
                foreach (var node in Normal.DescendantNodes)
                    yield return node;

                if (DiffuseMap)
                {
                    yield return DiffuseMapSamplerNode;
                    foreach (var node in DiffuseMapSamplerNode.DescendantNodes)
                        yield return node;
                }

                if (Shadows)
                {
                    yield return ShadowMapSamplerNode;
                    foreach (var node in ShadowMapSamplerNode.DescendantNodes)
                        yield return node;
                }

            }
        }

        public PhongLightingNode()
        {
            IsVerbose = true;
        }

        public override void Validate(TechniqueKey key)
        {
            base.Validate(key);
            phongMethod = new PhongLighting
            {
                Light = (IStruct)Light.Output,
                Specular = Specular,
                DiffuseMap = DiffuseMap,
                Shadows = Shadows,
                CubeMap = CubeMap
            };

            if (Shadows)
            {
                texOffset = new TexOffset();
                shadowMethod = new PCFShadows
                {
                    TexOffset = texOffset
                };
                phongMethod.ShadowMethod = new MethodReference(shadowMethod, new[] { 
                    Material.Reference,
                    MethodBase.Vectors.vShadowProjection, 
                    ShadowMapSamplerNode.Texture.FullName,
                    ShadowMapSamplerNode.Sampler.FullName});
            }
        }

        public override string Operation()
        {
            return string.Format("\t{0} {1} = {2};\n", Mapper.Map(Output.Type), Output.FullName, Access());
        }

        public override string Access()
        {
            if (DiffuseMap)
                return phongMethod.CallDiffuseMap((Struct)Light.Output, (Struct)Material.Output, (Vector)ViewDirection.Output, (Vector)LightDirection.Output, (Vector)Normal.Output,
                    (Vector)DiffuseMapSamplerNode.Coordinates.Output);
            else if (Shadows)
                return phongMethod.Call((Struct)Light.Output, (Struct)Material.Output, (Vector)ViewDirection.Output, (Vector)LightDirection.Output, (Vector)Normal.Output,
                    (Vector)ShadowMapSamplerNode.Coordinates.Output);
            else
                return phongMethod.Call((Struct)Light.Output, (Struct)Material.Output, (Vector)ViewDirection.Output, (Vector)LightDirection.Output, (Vector)Normal.Output);

        }

    }
}

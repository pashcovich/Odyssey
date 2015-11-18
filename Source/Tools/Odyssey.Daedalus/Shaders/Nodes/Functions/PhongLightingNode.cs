using Odyssey.Daedalus.Shaders.Methods;
using Odyssey.Daedalus.Shaders.Nodes.Operators;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Shaders;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Odyssey.Daedalus.Shaders.Nodes.Functions
{
    [DataContract]
    public class PhongLightingNode : NodeBase
    {
        private IVariable output;
        private IMethod shadowMethod;

        protected IMethod LightingMethod { get; private set; }

        public PhongLightingNode()
        {
            IsVerbose = true;
        }

        [DataMember]
        public bool CubeMap { get; set; }

        [DataMember]
        public bool DiffuseMap { get; set; }

        [DataMember]
        [SupportedType(Type.Float4)]
        [VertexShader(VertexShaderFlags.TextureUV)]
        [PixelShader(PixelShaderFlags.DiffuseMap)]
        public TextureSampleNode DiffuseMapSample { get; set; }

        [DataMember]
        [SupportedType(Type.Struct)]
        public INode Light { get; set; }

        [DataMember]
        [SupportedType(Type.Float3)]
        public INode LightDirection { get; set; }

        [DataMember]
        [SupportedType(Type.Struct)]
        [PixelShader(PixelShaderFlags.Diffuse)]
        public INode Material { get; set; }

        [DataMember]
        [SupportedType(Type.Float3)]
        [PixelShader(PixelShaderFlags.Specular)]
        public INode Normal { get; set; }

        [SupportedType(Type.Float4)]
        public override IVariable Output
        {
            get
            {
                if (output != null) return output;

                Output = new Vector
                {
                    Name = "cFinal",
                    Type = Type.Float4
                };
                return output;
            }
            set
            {
                output = value;
            }
        }

        public override IEnumerable<IMethod> RequiredMethods
        {
            get
            {
                if (Shadows)
                {
                    foreach (var mRef in shadowMethod.MethodReferences)
                        yield return mRef.Method;
                    yield return shadowMethod;
                }

                yield return LightingMethod;
            }
        }

        [DataMember]
        [SupportedType(Type.Float4)]
        [VertexShader(VertexShaderFlags.ShadowProjection)]
        [PixelShader(PixelShaderFlags.Shadows | PixelShaderFlags.ShadowMap)]
        public TextureSampleNode ShadowMapSample { get; set; }

        [DataMember]
        public bool Shadows { get; set; }

        [DataMember]
        public bool Specular { get; set; }

        [DataMember]
        [SupportedType(Type.Float3)]
        public INode ViewDirection { get; set; }

        public override string Access()
        {
            if (DiffuseMap || CubeMap)
                return LightingMethod.Call(Light.Reference, Material.Reference, LightDirection.Reference,
                    ViewDirection.Reference, Normal.Reference, DiffuseMapSample.Reference);
            else if (Shadows)
                return LightingMethod.Call(Light.Reference, Material.Reference, LightDirection.Reference,
                    ViewDirection.Reference, Normal.Reference, ShadowMapSample.Reference);
            else
                return LightingMethod.Call(Light.Reference, Material.Reference, LightDirection.Reference,
                    ViewDirection.Reference, Normal.Reference);
        }

        public override void Validate(TechniqueKey key)
        {
            base.Validate(key);
            if (LightingMethod == null)
                LightingMethod = new PhongLighting();
            else
                LightingMethod.ClearReferences();

            LightingMethod.ActivateSignature(key);

            LightingMethod.SetFlag(SpecularFlag, Specular);
            LightingMethod.SetFlag(DiffuseMapFlag, DiffuseMap);
            LightingMethod.SetFlag(CubeMapFlag, CubeMap);
            LightingMethod.SetFlag(ShadowsFlag, Shadows);

            if (!Shadows) return;

            shadowMethod = new PCFShadows();
            shadowMethod.ActivateSignature(key);
            LightingMethod.AddReference(new MethodReference(shadowMethod, new[] {
                MethodBase.Vectors.ShadowProjection,
                ShadowMapSample.Texture.FullName,
                ShadowMapSample.Sampler.FullName}));
        }

        protected override void RegisterNodes()
        {
            AddNode(nameof(Light), Light);
            AddNode(nameof(LightDirection), LightDirection);
            AddNode(nameof(Material), Material);
            AddNode(nameof(Normal), Normal);
            AddNode(nameof(ViewDirection), ViewDirection);
            if (DiffuseMap)
                AddNode(nameof(DiffuseMapSample), DiffuseMapSample);
            if (Shadows)
                AddNode(nameof(ShadowMapSample), ShadowMapSample);
        }
    }
}
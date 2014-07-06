using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;
using Odyssey.Graphics.Effects;
using SharpDX.Serialization;

namespace Odyssey.Graphics.Shaders
{
    [DataContract(IsReference = true)]
    [KnownType(typeof(EngineReference))]
    [KnownType(typeof(VertexShaderFlags))]
    [DebuggerDisplay("{Name}")]
    public class ShaderDescription
    {
        [DataMember] readonly Dictionary<int, ConstantBufferDescription> cbReferences;
        [DataMember] readonly Dictionary<int, TextureDescription> textureReferences;
        [DataMember] readonly Dictionary<int, SamplerStateDescription> samplerReferences;

        [DataMember]
        public string Name { get; private set; }
        [DataMember]
        public ShaderType ShaderType { get; private set; }
        [DataMember]
        public FeatureLevel FeatureLevel { get; private set; }
        [DataMember]
        public byte[] ByteCode { get; private set; }

        [DataMember]
        public TechniqueMapping Technique { get; internal set; }

        public int TextureCount { get { return textureReferences.Count; } }
        public int SamplerCount { get { return samplerReferences.Count; } }

        public IEnumerable<ConstantBufferDescription> ConstantBuffers { get { return cbReferences.Values; } }
        public IEnumerable<TextureDescription> TextureReferences { get { return textureReferences.Values; } }
        public IEnumerable<SamplerStateDescription> SamplerReferences { get { return samplerReferences.Values; } }

        public ShaderDescription(string name, ShaderType shaderType, FeatureLevel featureLevel, byte[] bytecode,
            IEnumerable<ConstantBufferDescription> shaderReferences, IEnumerable<TextureDescription> textureReferences, IEnumerable<SamplerStateDescription> samplerReferences)
        {
            Name = name;
            ShaderType = shaderType;
            FeatureLevel = featureLevel;
            ByteCode = bytecode;
            cbReferences = new Dictionary<int, ConstantBufferDescription>();
            foreach (var cbRef in shaderReferences)
                cbReferences.Add(cbRef.Index, cbRef);

            this.textureReferences = new Dictionary<int, TextureDescription>();
            foreach (var tRef in textureReferences)
                this.textureReferences.Add(tRef.Index, tRef);

            this.samplerReferences = new Dictionary<int, SamplerStateDescription>();
            foreach (var sRef in samplerReferences)
                this.samplerReferences.Add(sRef.Index, sRef);
        }

        public TextureDescription GetTextureDescription(int index)
        {
            Contract.Requires<ArgumentException>(TextureCount > index);
            return textureReferences[index];
        }

        public SamplerStateDescription GetSamplerStateDescription(int index)
        {
            Contract.Requires<ArgumentException>(SamplerCount > index);
            return samplerReferences[index];
        }

        public bool Validate()
        {
            return ConstantBuffers.Aggregate(true, (current, cbDesc) => current & cbDesc.Validate());
        }

        public void Serialize(BinarySerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
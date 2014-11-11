using Odyssey.Graphics.Effects;
using Odyssey.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Odyssey.Graphics.Shaders
{
    [DebuggerDisplay("{Name}")]
    public class ShaderDescription : IDataSerializable
    {
        private byte[] byteCode;
        private Dictionary<int, ConstantBufferDescription> cbReferences;
        private FeatureLevel featureLevel;
        private string name;
        private Dictionary<int, SamplerStateDescription> samplerReferences;
        private ShaderType shaderType;
        private Dictionary<int, TextureDescription> textureReferences;

        public ShaderDescription()
        { }

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

        public byte[] ByteCode
        {
            get { return byteCode; }
            private set { byteCode = value; }
        }

        public IEnumerable<ConstantBufferDescription> ConstantBuffers { get { return cbReferences.Values; } }

        public FeatureLevel FeatureLevel
        {
            get { return featureLevel; }
            private set { featureLevel = value; }
        }

        public string Name
        {
            get { return name; }
            private set { name = value; }
        }

        public int SamplerCount { get { return samplerReferences.Count; } }

        public IEnumerable<SamplerStateDescription> SamplerReferences { get { return samplerReferences.Values; } }

        public ShaderType ShaderType
        {
            get { return shaderType; }
            private set { shaderType = value; }
        }

        public int TextureCount { get { return textureReferences.Count; } }

        public IEnumerable<TextureDescription> TextureReferences { get { return textureReferences.Values; } }

        public SamplerStateDescription GetSamplerStateDescription(int index)
        {
            Contract.Requires<ArgumentException>(SamplerCount > index);
            return samplerReferences[index];
        }

        public TextureDescription GetTextureDescription(int index)
        {
            Contract.Requires<ArgumentException>(TextureCount > index);
            return textureReferences[index];
        }

        public void Serialize(BinarySerializer serializer)
        {
            serializer.BeginChunk("FXDS");

            serializer.Serialize(ref name);
            serializer.SerializeEnum(ref featureLevel);
            serializer.SerializeEnum(ref shaderType);
            serializer.Serialize(ref byteCode);

            // Shader References
            serializer.BeginChunk("REFS");
            serializer.Serialize(ref cbReferences, serializer.Serialize, (ref ConstantBufferDescription cb) => serializer.Serialize(ref cb));
            serializer.Serialize(ref textureReferences, serializer.Serialize);
            serializer.Serialize(ref samplerReferences, serializer.Serialize);
            serializer.EndChunk();

            serializer.EndChunk();
        }

        public bool Validate()
        {
            return ConstantBuffers.Aggregate(true, (current, cbDesc) => current & cbDesc.Validate());
        }
    }
}
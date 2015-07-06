using Odyssey.Graphics.Effects;
using Odyssey.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Odyssey.Text.Logging;
using EngineReference = Odyssey.Engine.EngineReference;

namespace Odyssey.Graphics.Shaders
{
    [DataContract]
    public class ConstantBufferDescription : IDataSerializable
    {
        [DataMember]
        private Dictionary<string, string> metadata;

        [DataMember]
        private Dictionary<int, EngineReference> references;

        [DataMember]
        private ShaderType shaderType;

        [DataMember]
        private int index;

        [DataMember]
        private string name;

        private Dictionary<string, List<EngineReference>> parsedReferences;

        [DataMember]
        private CBUpdateType cbUpdateFrequency;

        public ConstantBufferDescription()
        {
            name = "Unknown";
            index = 0;
            cbUpdateFrequency = CBUpdateType.None;
            shaderType = ShaderType.None;
            references = new Dictionary<int, EngineReference>();
            metadata = new Dictionary<string, string>();
        }

        public ConstantBufferDescription(string name, int index, CBUpdateType cbUpdateFrequency, ShaderType shaderType,
            IEnumerable<EngineReference> engineReferences, IEnumerable<KeyValuePair<string, string>> metadata)
        {
            this.name = name;
            this.index = index;
            this.cbUpdateFrequency = cbUpdateFrequency;
            this.shaderType = shaderType;
            references = new Dictionary<int, EngineReference>();

            references = engineReferences.ToDictionary(rShader => rShader.Index, rShader => rShader);
            this.metadata = metadata.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public int Index { get { return index; } }

        public IEnumerable<KeyValuePair<string, string>> Metadata { get { return metadata; } }

        public string Name { get { return name; } }

        public IEnumerable<KeyValuePair<int, EngineReference>> References { get { return references; } }

        public ShaderType ShaderType { get { return shaderType; } }

        public CBUpdateType CbUpdateFrequency { get { return cbUpdateFrequency; } }

        public bool ContainsMetadata(string key)
        {
            return metadata.ContainsKey(key);
        }

        public string Get(string key)
        {
            return metadata[key];
        }

        public void Serialize(BinarySerializer serializer)
        {
            serializer.Serialize(ref name);
            serializer.Serialize(ref index);
            serializer.SerializeEnum(ref shaderType);
            serializer.SerializeEnum(ref cbUpdateFrequency);
            serializer.Serialize(ref references, serializer.Serialize, (ref EngineReference r) => serializer.Serialize(ref r));
            serializer.Serialize(ref metadata, serializer.Serialize, serializer.Serialize);
        }

        public bool TryGetValue(string key, out string value)
        {
            return metadata.TryGetValue(key, out value);
        }

        public bool Validate()
        {
            bool test = true;
            if (parsedReferences == null)
            {
                LogEvent.Engine.Error("'{0}' no references have been parsed", name);
                foreach (var reference in references)
                    LogEvent.Engine.Error("\tMissing: '{0}'", reference.Value);
                return references.Count == 0;
            }

            foreach (var kvp in parsedReferences)
            {
                var missingReferences = kvp.Value == null
                    ? references.Values
                    : references.Values.Except(kvp.Value);
                foreach (EngineReference reference in missingReferences)
                {
                    LogEvent.Engine.Error("[{0}:{1}] missing from [{2}]: {3}, {4}", kvp.Key, reference, Name, Index, CbUpdateFrequency);
                    test = false;
                }
            }

            return test;
        }

        internal bool IsParsed(string technique, EngineReference reference)
        {
            return parsedReferences != null && parsedReferences.ContainsKey(technique) && parsedReferences[technique].Contains(reference);
        }

        internal void MarkAsParsed(string technique, EngineReference reference)
        {
            if (parsedReferences == null)
                parsedReferences = new Dictionary<string, List<EngineReference>>();
            if (!parsedReferences.ContainsKey(technique))
                parsedReferences.Add(technique, new List<EngineReference>());
            parsedReferences[technique].Add(reference);
        }

        internal void ClearParsed()
        {
            parsedReferences.Clear();
        }
    }
}
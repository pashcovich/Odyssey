using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;
using Odyssey.Graphics.Effects;
using Odyssey.Utilities.Logging;

namespace Odyssey.Graphics.Shaders
{
    [DataContract]
    public class ConstantBufferDescription
    {
        [DataMember]
        private readonly string name;

        [DataMember]
        private readonly int index;

        [DataMember]
        private readonly UpdateType updateFrequency;

        [DataMember]
        private readonly Dictionary<int, EngineReference> references;

        [DataMember] private readonly ShaderType shaderType;

        [DataMember]
        private readonly Dictionary<string, string> metadata;

        private Dictionary<string, List<EngineReference>> parsedReferences;

        public string Name { get { return name; } }

        public int Index { get { return index; } }

        public ShaderType ShaderType { get { return shaderType; } }

        public UpdateType UpdateFrequency { get { return updateFrequency; } }

        public IEnumerable<KeyValuePair<int, EngineReference>> References { get { return references; } }

        public IEnumerable<KeyValuePair<string, string>> Metadata { get { return metadata; } }

        public ConstantBufferDescription(string name, int index, UpdateType updateFrequency, ShaderType shaderType, 
            IEnumerable<ShaderReference> engineReferences, IEnumerable<KeyValuePair<string, string>> metadata)
        {
            Contract.ForAll(engineReferences, r => r.Type == ReferenceType.Engine);
            this.name = name;
            this.index = index;
            this.updateFrequency = updateFrequency;
            this.shaderType = shaderType;
            references = new Dictionary<int, EngineReference>();

            references = engineReferences.ToDictionary(rShader => rShader.Index, rShader => (EngineReference)rShader.Value);
            this.metadata = metadata.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public bool ContainsMetadata(string key)
        {
            return metadata.ContainsKey(key);
        }

        public string Get(string key)
        {
            return metadata[key];
        }

        public bool TryGetValue(string key, out string value)
        {
            return metadata.TryGetValue(key, out value);
        }

        internal void MarkAsParsed(string technique, EngineReference reference)
        {
            if (parsedReferences == null)
                parsedReferences = new Dictionary<string, List<EngineReference>>();
            if (!parsedReferences.ContainsKey(technique))
                parsedReferences.Add(technique, new List<EngineReference>());
            parsedReferences[technique].Add(reference);
        }

        internal bool IsParsed(string technique, EngineReference reference)
        {
            return parsedReferences != null && parsedReferences.ContainsKey(technique) && parsedReferences[technique].Contains(reference);
        }

        public bool Validate()
        {
            bool test = true;
            if (parsedReferences == null)
                return references.Count == 0;

            foreach (var kvp in parsedReferences)
            {
                var missingReferences = kvp.Value == null
                    ? references.Values
                    : references.Values.Except(kvp.Value);
                foreach (EngineReference reference in missingReferences)
                {
                    LogEvent.Engine.Error("[{0}:{1}] missing from [{2}]: {3}, {4}", kvp.Key, reference, Name, Index, UpdateFrequency);
                    test = false;
                }
            }
            

            
            
            return test;
        }
    }
}
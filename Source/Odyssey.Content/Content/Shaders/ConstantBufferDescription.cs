using Odyssey.Graphics;
using Odyssey.Graphics.Materials;
using Odyssey.Utilities.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;

namespace Odyssey.Content.Shaders
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

        [DataMember]
        private readonly Dictionary<string, string> metadata;

        private List<EngineReference> parsedReferences;

        public string Name { get { return name; } }

        public int Index { get { return index; } }

        public UpdateType UpdateFrequency { get { return updateFrequency; } }

        public IEnumerable<KeyValuePair<int, EngineReference>> References { get { return references; } }

        public IEnumerable<KeyValuePair<string, string>> Metadata { get { return metadata; } }

        public ConstantBufferDescription(string name, int index, UpdateType updateFrequency, IEnumerable<ShaderReference> engineReferences, IEnumerable<KeyValuePair<string, string>> metadata)
        {
            Contract.ForAll(engineReferences, r => r.Type == ReferenceType.Engine);
            this.name = name;
            this.index = index;
            this.updateFrequency = updateFrequency;
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

        public void MarkAsParsed(EngineReference reference)
        {
            if (parsedReferences == null)
                parsedReferences = new List<EngineReference>();
            parsedReferences.Add(reference);
        }

        public bool Validate()
        {
            var missingReferences = (parsedReferences == null)
                ? references.Values
                : references.Values.Except(parsedReferences);

            bool test = true;
            foreach (EngineReference reference in missingReferences)
            {
                LogEvent.Engine.Error("[{0}] missing from [{1}]: {2}, {3}", reference, Name, Index, UpdateFrequency);
                test = false;
            }
            return test;
        }
    }
}
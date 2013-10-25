using Odyssey.Graphics.Materials;
using ShaderGenerator.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;

namespace Odyssey.Content.Shaders
{
    [DataContract]
    public class ConstantBufferDescription : IEnumerable<KeyValuePair<int, EngineReference>>
    {
        [DataMember]
        public int Index { get; private set; }
        [DataMember]
        public UpdateFrequency UpdateFrequency { get; private set; }
        [DataMember]
        Dictionary<int, EngineReference> references;

        public ConstantBufferDescription(int index, UpdateFrequency updateFrequency, IEnumerable<ShaderReference> engineReferences)
        {
            Contract.Requires<ArgumentException>(engineReferences.All(r => r.Type == ReferenceType.Engine));
            Index = index;
            UpdateFrequency = updateFrequency;
            references = new Dictionary<int, EngineReference>();

            foreach (var reference in engineReferences)
                references.Add(reference.Index, (EngineReference)reference.Value);
        }


        IEnumerator<KeyValuePair<int, EngineReference>> IEnumerable<KeyValuePair<int, EngineReference>>.GetEnumerator()
        {
            return references.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return references.GetEnumerator();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Content;
using SharpDX;

namespace Odyssey.Engine
{
    [ContentReader(typeof(ReferenceReader))]
    public class EngineReferenceCollection : IReferenceService
    {
        readonly Dictionary<string, List<EngineReference>> engineReferences;

        public EngineReferenceCollection(IEnumerable<EngineReference> references)
        {
            engineReferences = (from r in references
                group r by r.Type
                into refGroup
                select new {refGroup.Key, refGroup}).ToDictionary(r => r.Key, r => r.refGroup.ToList());
        }

        public bool Contains(string type, string value)
        {
            return engineReferences.ContainsKey(type) && engineReferences[type].Any(r => string.Equals(r.Value, value));
        }
        public IEnumerable<EngineReference> Select(string referenceGroup)
        {
            return engineReferences[referenceGroup];
        }
    }
}

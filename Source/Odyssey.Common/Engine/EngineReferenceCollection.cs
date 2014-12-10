#region Using Directives

using System.Collections.Generic;
using System.Linq;
using Odyssey.Content;

#endregion

namespace Odyssey.Engine
{
    [ContentReader(typeof (ReferenceReader))]
    public class EngineReferenceCollection : IReferenceService
    {
        private readonly Dictionary<string, List<EngineReference>> engineReferences;

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
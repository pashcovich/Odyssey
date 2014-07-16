using System.Collections;
using System.Collections.Generic;

namespace Odyssey.Engine
{
    public interface IReferenceService
    {
        bool Contains(string type, string value);
        IEnumerable<EngineReference> Select(string referenceGroup);
    }
}
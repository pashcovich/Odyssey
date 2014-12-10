#region Using Directives

using System.Collections.Generic;

#endregion

namespace Odyssey.Engine
{
    public interface IReferenceService
    {
        bool Contains(string type, string value);
        IEnumerable<EngineReference> Select(string referenceGroup);
    }
}
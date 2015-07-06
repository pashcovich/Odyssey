using System.Collections.Generic;

namespace Odyssey.Epos.Nodes
{
    public interface ICollectionService<TIndex, TValue> : IDictionary<TIndex, TValue>
    {
    }

    public interface ILightService : ICollectionService<int, LightNode>
    { }

}

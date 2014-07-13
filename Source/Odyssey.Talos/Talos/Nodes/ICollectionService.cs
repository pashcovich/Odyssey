using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Talos.Nodes
{
    public interface ICollectionService<TIndex, TValue> : IDictionary<TIndex, TValue>
    {
    }

    public interface ILightService : ICollectionService<int, LightNode>
    { }

}

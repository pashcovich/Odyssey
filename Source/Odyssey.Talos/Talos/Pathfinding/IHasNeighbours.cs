using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Talos.Pathfinding
{
    interface IHasNeighbours<out TNode>
    {
        IEnumerable<TNode> Neighbours { get; }
    }
}

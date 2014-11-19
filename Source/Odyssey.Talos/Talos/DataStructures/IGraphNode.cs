using System.Collections.Generic;

namespace Odyssey.Talos.DataStructures
{
    public interface IGraphNode
    {
        string Name { get; set; }
        IEnumerable<Connection> Connections { get; }
        void AddDirected(Connection c);

    }
}

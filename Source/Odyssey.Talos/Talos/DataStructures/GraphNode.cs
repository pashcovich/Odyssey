using System.Collections.Generic;
using System.Linq;

namespace Odyssey.Talos.DataStructures
{
    public class GraphNode<T> : IGraphNode
    {
        private readonly T item;
        private readonly List<Connection> connections;

        public string Name { get; set; }

        public IEnumerable<Connection> Connections
        {
            get { return connections; }
        }

        public T Item { get { return item; }}

        public GraphNode(T item)
        {
            this.item = item;
            connections = new List<Connection>();
        }

        public void AddDirected(Connection c)
        {
            connections.Add(c);
        }
    }


}

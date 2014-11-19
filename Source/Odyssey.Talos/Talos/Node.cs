using System.Linq;
using Odyssey.Talos.DataStructures;
using Odyssey.Talos.Pathfinding;

namespace Odyssey.Talos
{
    public class Node : GraphNode<string>, IHasNeighbours<Node>
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Node(string item, object n, double lat, double lg) : base(item)
        {
            Name = item;
            Latitude = lat;
            Longitude = lg;
        }



        public System.Collections.Generic.IEnumerable<Node> Neighbours
        {
            get { return Connections.Select(n => (Node)n.To); }
        }
    }
}

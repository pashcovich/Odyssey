namespace Odyssey.Talos.DataStructures
{
    public class Connection : IConnection
    {
        private readonly IGraphNode from;
        private readonly IGraphNode to;

        public IGraphNode From
        {
            get { return from; }
        }

        public IGraphNode To
        {
            get { return to; }
        }

        public float Cost { get; set; }

        public Connection(IGraphNode from, IGraphNode to, float cost)
        {
            this.from = from;
            this.to = to;
            Cost = cost;
        }
    }
}

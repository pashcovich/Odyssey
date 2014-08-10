namespace Odyssey.Graphics
{
    public class ShapeMeshDescription
    {
        public string Id { get; internal set;}
        public int ArrayIndex { get; internal set; }
        public VertexPositionColor[] Vertices { get; internal set; }
        public int[] Indices { get; internal set; }
        public int Primitives { get { return Indices.Length / 3; } }

        public ShapeMeshDescription()
        {
        }

    }
}

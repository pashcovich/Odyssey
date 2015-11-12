using Odyssey.Engine;
using SharpDX;
using SharpDX.Direct3D;

namespace Odyssey.Graphics.Models
{
    public class GridMesh
    {
        public const string Key = "Grid";

        private readonly Vector3 topLeft;
        private readonly float cellWidth;
        private readonly int rows;
        private readonly int columns;
        private readonly Vector2 tileFactor;
        public Vector2 TileFactor { get { return tileFactor; } }

        protected GridMesh(Vector3 topLeft, float cellWidth, int rows, int columns, float tileX=1, float tileY=1)
        {
            this.topLeft = topLeft;
            this.cellWidth = cellWidth;
            this.rows = rows;
            this.columns = columns;
            tileFactor = new Vector2(tileX, tileY);
        }

        public void GenerateMesh(out VertexPositionNormalTexture[] vertices, out int[] indices)
        {
            var mb = new MeshBuilder();
            mb.BuildSquareGrid(topLeft, cellWidth, rows, columns);
            vertices = mb.GetVertices();
            indices = mb.GetIndices();
        }

        public static Model New(DirectXDevice device, Vector3 topLeftVertex, float cellWidth, int rows = 4, int columns = 4, float tileX = 1.0f, float tileY = 1.0f,
            PrimitiveTopology primitiveTopology = PrimitiveTopology.TriangleList, ModelOperation modelOperations = ModelOperation.None)
        {
            var quad = new GridMesh(topLeftVertex, cellWidth, rows, columns, tileX, tileY);
            VertexPositionNormalTexture[] vertices;
            int[] indices;
            quad.GenerateMesh(out vertices, out indices);
            return GeometricPrimitive.New(device, Key, vertices, indices, primitiveTopology, modelOperations);
        }
    }
}

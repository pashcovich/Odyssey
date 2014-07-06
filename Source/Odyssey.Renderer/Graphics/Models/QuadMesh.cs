using System.Collections.Generic;
using Odyssey.Engine;
using Odyssey.Graphics.Meshes;
using SharpDX;
using SharpDX.Direct3D;

namespace Odyssey.Graphics.Models
{
    public class QuadMesh
    {
        public const string Key = "Quad";

        private readonly Vector3 topLeft;
        private readonly float width;
        private readonly float height;
        private readonly Vector2 tileFactor;
        public Vector2 TileFactor { get { return tileFactor; } }

        protected QuadMesh(Vector3 topLeft, float width, float height, float tileX, float tileY)
        {
            this.topLeft = topLeft;
            this.width = width;
            this.height = height;
            tileFactor = new Vector2(tileX, tileY);
        }

        public void GenerateMesh(out VertexPositionNormalTexture[] vertices, out int[] indices)
        {
            Vector3 normal = Vector3.UnitZ;
            vertices = new []
            {
                new VertexPositionNormalTexture(new Vector3(topLeft.X, topLeft.Y, topLeft.Z),
                    normal, new Vector2(0.0f, 0.0f)),
                new VertexPositionNormalTexture(new Vector3(topLeft.X + width, topLeft.Y, topLeft.Z),
                    normal, new Vector2(1.0f, 0.0f)*TileFactor),
                new VertexPositionNormalTexture(new Vector3(topLeft.X, topLeft.Y + height, topLeft.Z),
                    normal, new Vector2(0.0f, 1.0f)*TileFactor),
                new VertexPositionNormalTexture(new Vector3(topLeft.X + width, topLeft.Y + height, topLeft.Z),
                    normal, new Vector2(1.0f, 1.0f)*TileFactor),
            };
            indices = new []
            {
                0, 1, 2,
                1, 3,2
            };;
        }

        public static Model New(DirectXDevice device, Vector3 topLeftVertex, float width, float height, float tileX = 1.0f, float tileY = 1.0f,
            PrimitiveTopology primitiveTopology = PrimitiveTopology.TriangleList, ModelOperation modelOperations = ModelOperation.None)
        {
            var quad = new QuadMesh(topLeftVertex, width, height, tileX, tileY);
            VertexPositionNormalTexture[] vertices;
            int[] indices;
            quad.GenerateMesh(out vertices, out indices);
            return GeometricPrimitive.New(device, Key, vertices, indices, primitiveTopology, modelOperations);
        }
    }
}

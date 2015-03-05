using Odyssey.Engine;
using SharpDX.Mathematics;
using SharpDX.Direct3D;

namespace Odyssey.Graphics.Models
{
    public class CubeMesh : IPrimitiveGenerator<VertexPositionNormalTexture>
    {
        internal const int CubeFaceCount = 6;

        internal static readonly Vector3[] FaceNormals =
        {
            new Vector3(0, 0, 1),
            new Vector3(0, 0, -1),
            new Vector3(1, 0, 0),
            new Vector3(-1, 0, 0),
            new Vector3(0, 1, 0),
            new Vector3(0, -1, 0),
        };

        private static readonly Vector2[] textureCoordinates = new Vector2[4]
                {
                    new Vector2(1, 0),
                    new Vector2(1, 1),
                    new Vector2(0, 1),
                    new Vector2(0, 0),
                };

        protected Vector2 TileFactor { get; private set; }
        protected Vector3 Radius { get; private set; }

        public CubeMesh(float width, float height, float depth, float tileX, float tileY)
        {
            TileFactor = new Vector2(tileX, tileY);
            Radius = new Vector3(width, height, depth)/2;
        }

        public virtual void GenerateMesh(out VertexPositionNormalTexture[] vertices, out int[] indices)
        {
            indices = new int[6 * CubeFaceCount];
            vertices = new VertexPositionNormalTexture[4 * CubeFaceCount];

            int vertexCount = 0;
            int indexCount = 0;
            // Create each face in turn.
            for (int i = 0; i < CubeFaceCount; i++)
            {
                Vector3 normal = FaceNormals[i];

                // Get two vectors perpendicular both to the face normal and to each other.
                Vector3 basis = (i >= 4) ? Vector3.UnitZ : Vector3.UnitY;

                Vector3 side1;
                Vector3.Cross(ref normal, ref basis, out side1);

                Vector3 side2;
                Vector3.Cross(ref normal, ref side1, out side2);

                // Six indices (two triangles) per face.
                int vbase = i * 4;
                indices[indexCount++] = (vbase + 0);
                indices[indexCount++] = (vbase + 2);
                indices[indexCount++] = (vbase + 1);

                indices[indexCount++] = (vbase + 0);
                indices[indexCount++] = (vbase + 3);
                indices[indexCount++] = (vbase + 2);

                // Four vertices per face.
                vertices[vertexCount++] = new VertexPositionNormalTexture((normal - side1 - side2) * Radius, normal, textureCoordinates[0]);
                vertices[vertexCount++] = new VertexPositionNormalTexture((normal - side1 + side2) * Radius, normal, textureCoordinates[1]);
                vertices[vertexCount++] = new VertexPositionNormalTexture((normal + side1 + side2) * Radius, normal, textureCoordinates[2]);
                vertices[vertexCount++] = new VertexPositionNormalTexture((normal + side1 - side2) * Radius, normal, textureCoordinates[3]);
            }
        }

        public static Model New(DirectXDevice device, 
            float width = 1.0f, float height = 1.0f, float depth = 1.0f, 
            float tileX = 1.0f, float tileY = 1.0f,
            PrimitiveTopology primitiveTopology = PrimitiveTopology.TriangleList,
            ModelOperation modelOperations= ModelOperation.None)
        {
            var cube = new CubeMesh(width,height,depth, tileX, tileY);
            VertexPositionNormalTexture[] vertices;
            int[] indices;
            cube.GenerateMesh(out vertices, out indices);

            return GeometricPrimitive.New(device, "Cube", vertices, indices, PrimitiveTopology.TriangleList, modelOperations);
            
        }
    }
}

using Odyssey.Engine;
using Odyssey.Graphics.Meshes;
using SharpDX.Direct3D11;
using SharpDX.Mathematics;
using SharpDX.Direct3D;

namespace Odyssey.Graphics.Models
{
    public class BoxCubeMapMesh : IPrimitiveGenerator<VertexPositionNormalTextureCube>
    {
        public BoxCubeMapMesh(float width, float height, float depth, float tileX, float tileY, float tileZ)
        {
            TileFactor = new Vector3(tileX, tileY, tileZ);
            Radius = new Vector3(width, height, depth)/2;
        }

        protected Vector3 TileFactor { get; private set; }
        protected Vector3 Radius { get; private set; }

        public virtual void GenerateMesh(out VertexPositionNormalTextureCube[] vertices, out int[] indices)
        {
            indices = new int[6 * CubeMesh.CubeFaceCount];
            vertices = new VertexPositionNormalTextureCube[4* CubeMesh.CubeFaceCount];

            int vertexCount = 0;
            int indexCount = 0;
            // Create each face in turn.
            for (int i = 0; i < CubeMesh.CubeFaceCount; i++)
            {
                Vector3 normal = CubeMesh.FaceNormals[i];

                // Get two vectors perpendicular both to the face normal and to each other.
                Vector3 basis = (i >= 4) ? Vector3.UnitZ : Vector3.UnitY;

                Vector3 side1;
                Vector3.Cross(ref normal, ref basis, out side1);

                Vector3 side2;
                Vector3.Cross(ref normal, ref side1, out side2);

                // Six indices (two triangles) per face.
                int vbase = i * 4;
                indices[indexCount++] = (vbase + 0);
                indices[indexCount++] = (vbase + 1);
                indices[indexCount++] = (vbase + 2);

                indices[indexCount++] = (vbase + 0);
                indices[indexCount++] = (vbase + 2);
                indices[indexCount++] = (vbase + 3);

                // Four vertices per face.
                vertices[vertexCount++] = new VertexPositionNormalTextureCube((normal - side1 - side2) * Radius, normal, (normal - side1 - side2));
                vertices[vertexCount++] = new VertexPositionNormalTextureCube((normal - side1 + side2) * Radius, normal, (normal - side1 + side2));
                vertices[vertexCount++] = new VertexPositionNormalTextureCube((normal + side1 + side2) * Radius, normal, (normal + side1 + side2));
                vertices[vertexCount++] = new VertexPositionNormalTextureCube((normal + side1 - side2) * Radius, normal, (normal + side1 - side2));
            }
 
        }

        public static Model New(DirectXDevice device, float width = 1.0f, float height = 1.0f, float depth = 1.0f, float tileX = 1.0f, float tileY = 1.0f, float tileZ = 1.0f, 
            PrimitiveTopology primitiveTopology = PrimitiveTopology.TriangleList,
            ResourceUsage usage = ResourceUsage.Default,
            ModelOperation modelOperations= ModelOperation.None)
        {
            var cube = new BoxCubeMapMesh(width, height, depth, tileX, tileY, tileZ);
            VertexPositionNormalTextureCube[] vertices;
            int[] indices;
            cube.GenerateMesh(out vertices, out indices);

            return GeometricPrimitive<VertexPositionNormalTextureCube>.New(device, "CubeUVW", vertices, indices, primitiveTopology, usage, modelOperations);
        }
    }
}

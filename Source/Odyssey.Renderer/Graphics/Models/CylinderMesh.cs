using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Engine;
using SharpDX;
using SharpDX.Direct3D;

namespace Odyssey.Graphics.Models
{
    /// <summary>
    /// A Cylinder primitive.
    /// </summary>
    public class CylinderMesh : IPrimitiveGenerator<VertexPositionNormalTexture>
    {
        private readonly float diameter;
        private readonly int tessellation;
        private readonly float height;
        private readonly Vector2 tileFactor;

        public CylinderMesh(float diameter, float height, int tessellation, float tileX, float tileY)
        {
            this.diameter = diameter;
            this.height = height;
            this.tessellation = tessellation;
            tileFactor = new Vector2(tileX, tileY);
        }

        public void GenerateMesh(out VertexPositionNormalTexture[] vertices, out int[] indices)
        {
            var vertexList = new List<VertexPositionNormalTexture>();
            var indexList = new List<int>();

            float halfHeight = height/2;

            var topOffset = Vector3.UnitY * halfHeight;

            float radius = diameter / 2;
            int stride = tessellation + 1;

            // Create a ring of triangles around the outside of the cylinder.
            for (int i = 0; i <= tessellation; i++)
            {
                var normal = MeshBuilder.GetCircleVector(i, tessellation);

                var sideOffset = normal * radius;

                var textureCoordinate = new Vector2((float)i / tessellation, 0) * tileFactor;

                vertexList.Add(new VertexPositionNormalTexture(sideOffset + topOffset, normal, textureCoordinate));
                vertexList.Add(new VertexPositionNormalTexture(sideOffset - topOffset, normal,
                    textureCoordinate + Vector2.UnitY));

                indexList.Add(i * 2);
                indexList.Add(i * 2 + 1);
                indexList.Add((i * 2 + 2) % (stride * 2));

                indexList.Add(i * 2 + 1);
                indexList.Add((i * 2 + 3) % (stride * 2));
                indexList.Add((i * 2 + 2) % (stride * 2));
                
            }

            // Create flat triangle fan caps to seal the top and bottom.
            MeshHelper.CreateCylinderCap(vertexList, indexList, tessellation, halfHeight, radius, tileFactor, true);
            MeshHelper.CreateCylinderCap(vertexList, indexList, tessellation, halfHeight, radius, tileFactor, false);

            vertices = vertexList.ToArray();
            indices = indexList.ToArray();
        }

        /// <summary>
        /// Creates a cylinder primitive.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="height">The height.</param>
        /// <param name="diameter">The diameter.</param>
        /// <param name="tessellation">The tessellation.</param>
        /// <param name="tileX">Horizontal texture coordinate scale factor.</param>
        /// <param name="tileY">Vertical texture coordinate scale factor.</param>
        /// <param name="modelOperations">Operations to perform on the model.</param>
        /// <returns>A cylinder primitive.</returns>
        public static Model New(DirectXDevice device,
            float diameter, float height, int tessellation = 8,
            float tileX = 1.0f, float tileY = 1.0f,
            ModelOperation modelOperations = ModelOperation.None)
        {
            var cylinder = new CylinderMesh(diameter, height, tessellation, tileX, tileY);
            VertexPositionNormalTexture[] vertices;
            int[] indices;
            cylinder.GenerateMesh(out vertices, out indices);

            return GeometricPrimitive.New(device, "Cylinder", vertices, indices, PrimitiveTopology.TriangleList, modelOperations);
        }


    }
}

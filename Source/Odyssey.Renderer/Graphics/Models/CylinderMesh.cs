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

        // Helper computes a point on a unit circle, aligned to the x/z plane and centered on the origin.
        private static Vector3 GetCircleVector(int i, int tessellation)
        {
            var angle = (float) (i*2.0*Math.PI/tessellation);
            var dx = (float) Math.Sin(angle);
            var dz = (float) Math.Cos(angle);

            return new Vector3(dx, 0, dz);
        }

        // Helper creates a triangle fan to close the end of a cylinder.
        private static void CreateCylinderCap(List<VertexPositionNormalTexture> vertices, List<int> indices,
            int tessellation, float height, float radius, Vector2 tileFactor, bool isTop)
        {
            // Create cap indices.
            for (int i = 0; i < tessellation - 2; i++)
            {
                int i1 = (i + 1)%tessellation;
                int i2 = (i + 2)%tessellation;

                if (isTop)
                {
                    SharpDX.Utilities.Swap(ref i1, ref i2);
                }

                int vbase = vertices.Count;
                indices.Add(vbase);
                indices.Add(vbase + i1);
                indices.Add(vbase + i2);
            }

            // Which end of the cylinder is this?
            var normal = Vector3.UnitY;
            var textureScale = new Vector2(-0.5f);

            if (!isTop)
            {
                normal = -normal;
                textureScale.X = -textureScale.X;
            }

            // Create cap vertices.
            for (int i = 0; i < tessellation; i++)
            {
                var circleVector = GetCircleVector(i, tessellation);
                var position = (circleVector*radius) + (normal*height);
                var textureCoordinate = new Vector2(circleVector.X*textureScale.X + 0.5f,
                    circleVector.Z*textureScale.Y + 0.5f) * tileFactor;

                vertices.Add(new VertexPositionNormalTexture(position, normal, textureCoordinate));
            }
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
                var normal = GetCircleVector(i, tessellation);

                var sideOffset = normal * radius;

                var textureCoordinate = new Vector2((float)i / tessellation, 0) * tileFactor;

                vertexList.Add(new VertexPositionNormalTexture(sideOffset + topOffset, normal, textureCoordinate));
                vertexList.Add(new VertexPositionNormalTexture(sideOffset - topOffset, normal,
                    textureCoordinate + Vector2.UnitY));

                indexList.Add(i * 2);
                indexList.Add((i * 2 + 2) % (stride * 2));
                indexList.Add(i * 2 + 1);

                indexList.Add(i * 2 + 1);
                indexList.Add((i * 2 + 2) % (stride * 2));
                indexList.Add((i * 2 + 3) % (stride * 2));
            }

            // Create flat triangle fan caps to seal the top and bottom.
            CreateCylinderCap(vertexList, indexList, tessellation, halfHeight, radius, tileFactor, true);
            CreateCylinderCap(vertexList, indexList, tessellation, halfHeight, radius, tileFactor, false);

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
        /// <returns>A cylinder primitive.</returns>
        public static Model New(DirectXDevice device,
            float diameter, float height, int tessellation = 8,
            float tileX = 1.0f, float tileY = 1.0f,
            PrimitiveTopology primitiveTopology = PrimitiveTopology.TriangleList,
            ModelOperation modelOperations = ModelOperation.None)
        {
            CylinderMesh cylinder = new CylinderMesh(diameter, height, tessellation, tileX, tileY);
            VertexPositionNormalTexture[] vertices;
            int[] indices;
            cylinder.GenerateMesh(out vertices, out indices);

            return GeometricPrimitive.New(device, "Cylinder", vertices, indices, PrimitiveTopology.TriangleList, modelOperations);
        }


    }
}

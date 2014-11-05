using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Engine;
using Odyssey.Geometry;
using Odyssey.Geometry.Primitives;
using SharpDX;
using SharpDX.Direct3D;

namespace Odyssey.Graphics.Models
{
    public class EllipseMesh : IPrimitiveGenerator<VertexPositionNormalTexture>
    {
        private readonly Ellipse ellipse;
        private readonly float innerRadiusRatio;
        private readonly int tessellation;
        private readonly Vector2 tileFactor;

        private EllipseMesh(float semiMajorAxis, float semiMinorAxis, int tessellation, float innerRadiusRatio = 0, float tileX = 1, float tileY = 1)
        {
            ellipse = new Ellipse(semiMajorAxis, semiMinorAxis);
            this.innerRadiusRatio = innerRadiusRatio;
            this.tessellation = tessellation;
            tileFactor = new Vector2(tileX, tileY);
        }

        public void GenerateMesh(out VertexPositionNormalTexture[] vertices, out int[] indices)
        {
            int slices = tessellation;
            int rings = 1;
            var ringOffsets = new float[rings+1];
            ringOffsets[0] = innerRadiusRatio;
            float delta = (1-innerRadiusRatio)/rings;
            for (int i = 1; i < rings; i++)
                ringOffsets[i] = ringOffsets[i - 1] + delta;
            ringOffsets[rings] = 1.0f;

            Vector3[] vertexArray = innerRadiusRatio > 0
                ? CreateRingMesh(ellipse, ringOffsets, slices, Matrix.Identity, out indices)
                : CreateMesh(ellipse, ringOffsets, slices, Matrix.Identity, out indices);
            vertices = vertexArray.Select(v => new VertexPositionNormalTexture(v, Vector3.UnitZ, new Vector2(v.X, v.Y)*tileFactor)).ToArray();
        }

        internal static Vector3[] CreateRingMesh(Ellipse ellipse, float[] ringOffsets, int slices, Matrix transform, out int[] indices)
        {
            int rings = ringOffsets.Length;
            var vertices = new Vector3[rings * slices];
            indices = new int[3 * slices * 2 * (rings - 1)];

            var innerRingVertices = Ellipse.CalculateVertices(ellipse.Center, ellipse.RadiusX * ringOffsets[0], ellipse.RadiusY * ringOffsets[0], slices).ToArray();
            for (int i = 0; i < slices; i++)
            {
                vertices[i] = innerRingVertices[i].ToVector3();
            }

            int indexCount = 0;
            for (int r = 1; r < rings; r++)
            {
                // Other rings vertices
                var outerRingVertices = Ellipse.CalculateVertices(ellipse.Center, ellipse.RadiusX * ringOffsets[r], ellipse.RadiusY * ringOffsets[r], slices).ToArray();
                for (int i = 0; i < slices; i++)
                {
                    vertices[(r * slices) + i] = outerRingVertices[i].ToVector3();
                }

                // Other rings indices
                int baseIndex = (r - 1) * slices;

                for (int i = 0; i < slices; i++)
                {
                    // current ring

                    // first face
                    indices[indexCount] = baseIndex + i;
                    indices[indexCount + 2] = baseIndex + i + slices;
                    indices[indexCount + 1] = baseIndex + i + slices + 1;

                    // second face
                    indices[indexCount + 3] = baseIndex + i;
                    indices[indexCount + 5] = baseIndex + i + slices + 1;
                    indices[indexCount + 4] = baseIndex + i + 1;
                    indexCount += 6;
                }
                // Wrap faces
                indices[indexCount - 4] = r * slices;
                indices[indexCount - 5] = (r - 1) * slices;
                indices[indexCount - 1] = (r + 1) * slices - 1;
            }

            if (!transform.IsIdentity)
            {
                for (int i = 0; i < vertices.Length; i++)
                {
                    var vertex = vertices[i];
                    vertices[i] = Vector3.Transform(vertex, transform).ToVector3();
                }
            }
            return vertices;

        }
        internal static Vector3[] CreateMesh(Ellipse ellipse, float[] ringOffsets, int slices, Matrix transform, out int[] indices)
        {
            int rings = ringOffsets.Length;
            Vector3[] vertices = new Vector3[((rings - 1) * slices) + 1];

            vertices[0] = ellipse.Center.ToVector3();

            // First ring vertices
            // ringOffsets[0] is assumed to be the center
            // ringOffsets[1] the first ring
            var innerRingVertices = Ellipse.CalculateVertices(ellipse.Center, ellipse.RadiusX * ringOffsets[1], ellipse.RadiusY * ringOffsets[1], slices).ToArray();
            for (int i = 0; i < slices; i++)
            {
                vertices[i + 1] = innerRingVertices[i].ToVector3();
            }

            indices = new int[3 * slices * ((2 * (rings - 2)) + 1)];

            TriangulateEllipseFirstRing(slices, ref indices);

            if (rings == 1)
            {
                return vertices;
            }

            int indexCount = 0;
            int baseIndex = 3 * slices;
            for (int r = 1; r < rings - 1; r++)
            {
                // Other rings vertices
                var outerRingVertices = Ellipse.CalculateVertices(ellipse.Center, ellipse.RadiusX * ringOffsets[r + 1], ellipse.RadiusY * ringOffsets[r + 1], slices).ToArray();
                for (int i = 0; i < slices; i++)
                {
                    vertices[(r * slices) + i + 1] = outerRingVertices[i].ToVector3();
                }

                // Other rings indices
                int j = r * slices;
                int k = (r - 1) * slices;

                for (int i = 0; i < slices; i++)
                {
                    // current ring

                    // first face
                    indices[baseIndex + indexCount] = j + i + 2;
                    indices[baseIndex + indexCount + 1] = j + i + 1;
                    indices[baseIndex + indexCount + 2] = k + i + 1;
                    // second face
                    indices[baseIndex + indexCount + 3] = j + i + 2;
                    indices[baseIndex + indexCount + 4] = k + i + 1;
                    indices[baseIndex + indexCount + 5] = k + i + 2;
                    indexCount += 6;
                }
                // Wrap faces
                indices[baseIndex + indexCount - 3] = 1 + (r - 1) * slices;
                indices[baseIndex + indexCount - 6] = r * slices + 1;
            }
            SharpDX.Utilities.Swap(ref indices[baseIndex + indexCount - 1], ref indices[baseIndex + indexCount - 2]);
            if (!transform.IsIdentity)
            {
                for (int i = 0; i < vertices.Length; i++)
                {
                    var vertex = vertices[i];
                    vertices[i] = Vector3.Transform(vertex, transform).ToVector3();
                }
            }

            return vertices;
        }

        static void TriangulateEllipseFirstRing(int slices, ref int[] indices, int startIndex = 0)
        {
            // First ring indices
            for (int i = 0; i < slices; i++)
            {
                indices[startIndex + (3 * i)] = 0;
                indices[startIndex + (3 * i) + 1] = i + 2;
                indices[startIndex + (3 * i) + 2] = i + 1;
            }
            indices[startIndex + 3 * slices - 2] = 1;
            SharpDX.Utilities.Swap(ref indices[startIndex + 3 * slices - 2], ref indices[startIndex + 3 * slices - 1]);

        }

        public static Model New(DirectXDevice device, float semiMajorAxis=1.0f, float semiMinorAxis = 1.0f, int tessellation = 64, float innerRadiusRatio = 0f, float tileX = 1.0f,
            float tileY = 1.0f, ModelOperation modelOperations = ModelOperation.None)
        {
            Contract.Requires<ArgumentException>(tessellation >= 3, "tessellation must be >= 3");
            var ellipse = new EllipseMesh(semiMajorAxis, semiMinorAxis, tessellation, innerRadiusRatio, tileX, tileY);
            VertexPositionNormalTexture[] vertices;
            int[] indices;
            ellipse.GenerateMesh(out vertices, out indices);

            return GeometricPrimitive.New(device, "Ellipse", vertices, indices, PrimitiveTopology.TriangleList, modelOperations);
        }
    }
}

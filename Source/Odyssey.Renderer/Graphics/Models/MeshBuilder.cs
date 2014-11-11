using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SharpDX.Mathematics;

namespace Odyssey.Graphics.Models
{
    public class MeshBuilder
    {
        private readonly List<Vector3> vertices;
        private readonly List<Vector2> textureUVs;
        private readonly List<Vector3> normals;
        private readonly List<int> indices;

        public MeshBuilder()
        {
            vertices = new List<Vector3>();
            textureUVs = new List<Vector2>();
            normals = new List<Vector3>();
            indices = new List<int>();
            TileFactor = new Vector2(1);
        }

        public Vector2 TileFactor { get; set; }

        public int VertexCount { get { return vertices.Count; } }
        public int IndexCount { get { return indices.Count; } }

        public VertexPositionNormalTexture[] GetVertices()
        {
            Contract.Requires<InvalidOperationException>(VertexCount > 3, "Not enough vertices.");
            var array = new VertexPositionNormalTexture[vertices.Count];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = new VertexPositionNormalTexture(vertices[i], normals[i], textureUVs[i]);
            }
            return array;
        }

        public int[] GetIndices()
        {
            Contract.Requires<InvalidOperationException>(IndexCount > 3, "Not enough indices.");
            return indices.ToArray();
        }

        public void AddTriangle(int i0, int i1, int i2)
        {
            indices.Add(i0);
            indices.Add(i1);
            indices.Add(i2);
        }

        // Helper computes a point on a unit circle, aligned to the x/z plane and centered on the origin.
        internal static Vector3 GetCircleVector(int i, int tessellation)
        {
            var angle = (float)(i * 2.0 * Math.PI / tessellation);
            var dx = (float)Math.Sin(angle);
            var dz = (float)Math.Cos(angle);

            return new Vector3(dx, 0, dz);
        }

        public void BuildRing(int tessellation, Vector3 centre, float radius, float v, bool buildTriangles)
        {
            for (int i = 0; i <= tessellation; i++)
            {
                Vector3 unitPosition = GetCircleVector(i, tessellation);

                vertices.Add(centre + unitPosition * radius);
                normals.Add(unitPosition);
                textureUVs.Add(new Vector2((float)i / tessellation, v) * TileFactor);

                if (i > 0 && buildTriangles)
                {
                    int baseIndex = vertices.Count - 1;

                    int vertsPerRow = tessellation + 1;

                    int index0 = baseIndex;
                    int index1 = baseIndex - 1;
                    int index2 = baseIndex - vertsPerRow;
                    int index3 = baseIndex - vertsPerRow - 1;

                    AddTriangle(index0, index1, index2);
                    AddTriangle(index2, index1, index3);
                }
            }
        }

        public void BuildCap(int tessellation, Vector3 centre, float radius, bool reverseDirection)
        {
            Vector3 normal = reverseDirection ? Vector3.Down : Vector3.Up;

            //one vertex in the center:
            vertices.Add(centre);
            normals.Add(normal);
            textureUVs.Add(new Vector2(0.5f, 0.5f) * TileFactor);

            int centreVertexIndex = vertices.Count - 1;

            //vertices around the edge:
            for (int i = 0; i <= tessellation; i++)
            {
                Vector3 unitPosition = GetCircleVector(i, tessellation);

                vertices.Add(centre + unitPosition * radius);
                normals.Add(normal);

                Vector2 uv = new Vector2(unitPosition.X + 1.0f, unitPosition.Z + 1.0f) * 0.5f * TileFactor;
                textureUVs.Add(uv);

                //build a triangle:
                if (i > 0)
                {
                    int baseIndex = vertices.Count - 1;

                    if (reverseDirection)
                        AddTriangle(centreVertexIndex, baseIndex, baseIndex - 1);
                    else
                        AddTriangle(centreVertexIndex, baseIndex - 1, baseIndex);
                }
            }
        }
    }
}

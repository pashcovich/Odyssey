using System;
using System.Diagnostics.Contracts;
using Odyssey.Geometry.Primitives;
using Odyssey.Extensions;
using SharpDX;
using System.Linq;

namespace Odyssey.Graphics.Models
{
    public static class ModelEditor
    {
        public static VertexPositionNormalTextureBarycentric[] ConvertToBarycentricVertices(VertexPositionNormalTexture[] vertices,
            int[] indices)
        {
            var b0 = new Vector3(1, 0, 0);
            var b1 = new Vector3(0, 1, 0);
            var b2 = new Vector3(0, 0, 1);
            var barycentricVertices =
                new VertexPositionNormalTextureBarycentric[indices.Length];
            for (var i = 0; i < indices.Length; i += 3)
            {
                var i0 = indices[i];
                var i1 = indices[i + 1];
                var i2 = indices[i + 2];
                var v0 = vertices[i0];
                var v1 = vertices[i1];
                var v2 = vertices[i2];

                barycentricVertices[i] = new VertexPositionNormalTextureBarycentric(v0.Position, v0.Normal, v0.TextureUV, b0);
                barycentricVertices[i + 1] = new VertexPositionNormalTextureBarycentric(v1.Position, v1.Normal, v1.TextureUV, b1);
                barycentricVertices[i + 2] = new VertexPositionNormalTextureBarycentric(v2.Position, v2.Normal, v2.TextureUV, b2);
            }
            return barycentricVertices;
        }

        public static VertexPositionNormalTextureBarycentric[] ConvertToBarycentricEdgeNormalVertices(
            VertexPositionNormalTexture[] vertices, int[] indices)
        {
            const float maxEdgeWidth = 100;
            var maxBarycentric = new Vector3(maxEdgeWidth)+1;
            var barycentricVertices = ConvertToBarycentricVertices(vertices, indices);
            var faceCount = indices.Length / 3;
            var faces = new Face[faceCount];
            var triangles = new Triangle3[faceCount];
            for (var f = 0; f < faceCount; f++)
            {
                var i = f * 3;
                var i0 = i;
                var i1 = i + 1;
                var i2 = i + 2;

                var v0 = barycentricVertices[i0];
                var v1 = barycentricVertices[i1];
                var v2 = barycentricVertices[i2];
                faces[f] = new Face(i0, i1, i2);
                triangles[f] = new Triangle3(v0.Position,v1.Position,v2.Position);
            }
            var tolerance = new Vector3(MathUtil.ZeroTolerance);
            for (var i = 0; i < faces.Length; i++)
            {
                var f1 = faces[i];
                for (var j = 0; j < faces.Length; j++)
                {
                    var f2 = faces[j];
                    if (f1 == f2)
                        continue;
                    var t1 = triangles[i];
                    var t2 = triangles[j];
                    var n1 = triangles[i].CalculateNormal();
                    var n2 = triangles[j].CalculateNormal();

                    if (!Vector3.NearEqual(n1, n2, tolerance))
                        continue;

                    UndirectedEdge edge1, edge2;

                    if (!Triangle3.FindOverlappingEdges(t1, t2, f1, f2, out edge1, out edge2))
                        continue;

                    var iE1 = f1.RemainingIndex(edge1);
                    var biE1 = barycentricVertices[iE1].Barycentric;
                    var iE2 = f2.RemainingIndex(edge2);
                    var biE2 = barycentricVertices[iE2].Barycentric;

                    barycentricVertices[edge1.Item1].Barycentric += 2*biE1;
                    barycentricVertices[edge2.Item2].Barycentric += biE2;

                }
            }
            return barycentricVertices;
        }

        public static VertexPositionNormalTextureBarycentric[] ConvertToBarycentricEdgeVertices(
            VertexPositionNormalTexture[] vertices, int[] indices)
        {
            const float maxEdgeWidth = 100;
            var barycentricVertices = ConvertToBarycentricVertices(vertices, indices);
            var faces = indices.Length / 3;
            for (var f = 0; f < faces / 2; f++)
            {
                var i = f * 6;
                var i0 = i;
                var i1 = i + 1;
                var i2 = i + 2;
                var i3 = i + 3;
                var i4 = i + 4;
                var i5 = i + 5;

                var v0 = barycentricVertices[i0];
                var v1 = barycentricVertices[i1];
                var v2 = barycentricVertices[i2];
                var v3 = barycentricVertices[i3];
                var v4 = barycentricVertices[i4];
                var v5 = barycentricVertices[i5];

                var f1 = new Face(i, i1, i2);
                var f2 = new Face(i3, i4, i5);
                var t1 = new Triangle3(v0.Position, v1.Position, v2.Position);
                var t2 = new Triangle3(v3.Position, v4.Position, v5.Position);

                UndirectedEdge edge1, edge2;

                if (!Triangle3.FindOverlappingEdges(t1, t2, f1, f2, out edge1, out edge2))
                    continue;

                var iE1 = f1.RemainingIndex(edge1);
                var biE1 = barycentricVertices[iE1].Barycentric;
                var iE2 = f2.RemainingIndex(edge2);
                var biE2 = barycentricVertices[iE2].Barycentric;

                barycentricVertices[edge1.Item1].Barycentric += maxEdgeWidth * biE1;
                barycentricVertices[edge2.Item2].Barycentric += maxEdgeWidth * biE2;
            }
            return barycentricVertices;
        }

        public static VertexPositionNormalTextureTangent[] CalculateTangentArray(VertexPositionNormalTexture[] vertices, int[] indices)
        {
            var tan1 = new Vector3[vertices.Length];
            var tan2 = new Vector3[vertices.Length];

            var tangents = new Vector4[vertices.Length];

            for (var i = 0; i < indices.Length; i += 3)
            {
                var i1 = indices[i];
                var i2 = indices[i + 1];
                var i3 = indices[i + 2];

                var v1 = vertices[i1].Position;
                var v2 = vertices[i2].Position;
                var v3 = vertices[i3].Position;

                var w1 = vertices[i1].TextureUV;
                var w2 = vertices[i2].TextureUV;
                var w3 = vertices[i3].TextureUV;

                var x1 = v2.X - v1.X;
                var x2 = v3.X - v1.X;
                var y1 = v2.Y - v1.Y;
                var y2 = v3.Y - v1.Y;
                var z1 = v2.Z - v1.Z;
                var z2 = v3.Z - v1.Z;

                var s1 = w2.X - w1.X;
                var s2 = w3.X - w1.X;
                var t1 = w2.Y - w1.Y;
                var t2 = w3.Y - w1.Y;

                var r = 1.0f / (s1 * t2 - s2 * t1);

                var sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
                var tdir = new Vector3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);

                tan1[i1] += sdir;
                tan1[i2] += sdir;
                tan1[i3] += sdir;

                tan2[i1] += tdir;
                tan2[i2] += tdir;
                tan2[i3] += tdir;
            }

            var newVertices = new VertexPositionNormalTextureTangent[vertices.Length];

            for (var i = 0; i < vertices.Count(); i++)
            {
                var n = vertices[i].Normal;
                var t = tan1[i];

                var tmp = Vector3.Normalize(t - n * Vector3.Dot(n, t));
                var w = (Vector3.Dot(Vector3.Cross(n, t), tan2[i]) < 0.0f) ? -1.0f : 1.0f;

                tangents[i] = new Vector4(tmp.X, tmp.Y, tmp.Z, w);

                var p = vertices[i].Position;
                var tx = vertices[i].TextureUV;
                newVertices[i] = new VertexPositionNormalTextureTangent(p, n, tx, tangents[i]);
            }

            return newVertices;
        }

        /// <summary>
        /// Randomizes the order in which primitives are listed in the array. It works by
        /// reordering groups of consecutive indices.
        /// </summary>
        /// <param name="indices">The initial array of indices.</param>
        /// <param name="polygonFaceCount">The number of faces forming a polygon.</param>
        /// <returns>An array of indices where primitives are listed in a random order.</returns>
        public static int[] RandomizePrimitiveOrder(int[] indices, int polygonFaceCount = 1)
        {
            Contract.Requires<ArgumentNullException>(indices != null, "indices");
            Contract.Requires<ArgumentException>(indices.Length > 0 && indices.Length % 3 ==0, "Index count must be non-zero and a multiple of three");
            Contract.Requires<ArgumentException>(polygonFaceCount > 0, "A polygon must be formed by at least one face");

            var primitiveOrder = System.Linq.Enumerable.Range(0, indices.Length/ (3*polygonFaceCount)).ToArray();
            var randomizedOrder = primitiveOrder.RandomSubset(primitiveOrder.Length).ToArray();

            var newIndices = new int[indices.Length];
            for (var i = 0; i < randomizedOrder.Length; i++)
            {
                var baseIndex = randomizedOrder[i] * 3 * polygonFaceCount;
                var baseDestIndex = i * 3 * polygonFaceCount;

                for (var j = 0; j < polygonFaceCount; j++)
                {

                    baseIndex += j*3;
                    baseDestIndex += j*3;
                    newIndices[baseDestIndex] = indices[baseIndex];
                    newIndices[baseDestIndex + 1] = indices[baseIndex + 1];
                    newIndices[baseDestIndex + 2] = indices[baseIndex + 2];
                }
            }
            return newIndices;
        }
    }
}
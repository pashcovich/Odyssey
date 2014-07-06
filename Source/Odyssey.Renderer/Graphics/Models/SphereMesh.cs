using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Odyssey.Engine;
using Odyssey.Graphics.Meshes;
using SharpDX;
using SharpDX.Direct3D;

namespace Odyssey.Graphics.Models
{
    public class SphereMesh : IPrimitiveGenerator<VertexPositionNormalTexture>
    {
        private readonly int tessellation;
        private readonly float diameter;
        private readonly int verticalSegments;
        private readonly int horizontalSegments;
        private readonly Vector2 tileFactor;

        protected SphereMesh(float tileX, float tileY, int tessellation, float diameter)
        {
            this.tessellation = tessellation;
            this.diameter = diameter;
            verticalSegments = tessellation;
            horizontalSegments = tessellation * 2;
            tileFactor = new Vector2(tileX, tileY);
        }

        VertexPositionNormalTexture[] GenerateVertices()
        {
            var vertices = new VertexPositionNormalTexture[(verticalSegments + 1) * (horizontalSegments + 1)];

            float radius = diameter / 2;

            int vertexCount = 0;
            // Create rings of vertices at progressively higher latitudes.
            for (int i = 0; i <= verticalSegments; i++)
            {
                float v = 1.0f - (float)i / verticalSegments;

                var latitude = (float)((i * Math.PI / verticalSegments) - Math.PI / 2.0);
                var dy = (float)Math.Sin(latitude);
                var dxz = (float)Math.Cos(latitude);

                // Create a single ring of vertices at this latitude.
                for (int j = 0; j <= horizontalSegments; j++)
                {
                    float u = (float)j / horizontalSegments;

                    var longitude = (float)(j * 2.0 * Math.PI / horizontalSegments);
                    var dx = (float)Math.Sin(longitude);
                    var dz = (float)Math.Cos(longitude);

                    dx *= dxz;
                    dz *= dxz;

                    var normal = new Vector3(dx, dy, dz);
                    var textureCoordinate = new Vector2(u, v);

                    vertices[vertexCount++] = new VertexPositionNormalTexture(normal * radius, normal, textureCoordinate);
                }
            }

            return vertices;
        }

        int[] GenerateIndices()
        {
            var indices = new int[(verticalSegments) * (horizontalSegments + 1) * 6];

            // Fill the index buffer with triangles joining each pair of latitude rings.
            int stride = horizontalSegments + 1;

            int indexCount = 0;
            for (int i = 0; i < verticalSegments; i++)
            {
                for (int j = 0; j <= horizontalSegments; j++)
                {
                    int nextI = i + 1;
                    int nextJ = (j + 1) % stride;

                    indices[indexCount++] = (ushort)(i * stride + j);
                    indices[indexCount++] = (ushort)(nextI * stride + j);
                    indices[indexCount++] = (ushort)(i * stride + nextJ);

                    indices[indexCount++] = (ushort)(i * stride + nextJ);
                    indices[indexCount++] = (ushort)(nextI * stride + j);
                    indices[indexCount++] = (ushort)(nextI * stride + nextJ);
                }
            }

            return indices;
        }

        public void GenerateMesh(out VertexPositionNormalTexture[] vertices, out int[] indices)
        {
            vertices = GenerateVertices();
            indices = GenerateIndices();
        }

        /// <summary>
        /// Creates a sphere primitive.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="diameter">The diameter.</param>
        /// <param name="tessellation">The tessellation.</param>
        /// <param name="toLeftHanded">if set to <c>true</c> vertices and indices will be transformed to left handed. Default is true.</param>
        /// <returns>A sphere primitive.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">tessellation;Must be >= 3</exception>
        public static Model New(DirectXDevice device, float diameter = 2.0f, int tessellation = 16, float tileX = 1.0f,
            float tileY = 1.0f, PrimitiveTopology primitiveTopology = PrimitiveTopology.TriangleList, ModelOperation modelOperations = ModelOperation.None)
        {
            Contract.Requires<ArgumentException>(tessellation >= 3, "tessellation must be >= 3");
            SphereMesh sphere = new SphereMesh(tileX, tileY, tessellation, diameter);
            VertexPositionNormalTexture[] vertices;
            int[] indices;
            sphere.GenerateMesh(out vertices, out indices);

            return GeometricPrimitive.New(device, "Sphere", vertices, indices, primitiveTopology, modelOperations);
        }

    }
    }


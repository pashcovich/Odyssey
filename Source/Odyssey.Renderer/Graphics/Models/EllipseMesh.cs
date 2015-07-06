using System;
using System.Diagnostics.Contracts;
using System.Linq;
using Odyssey.Engine;
using Odyssey.Geometry.Extensions;
using Odyssey.Geometry.Primitives;
using SharpDX.Mathematics;
using SharpDX.Direct3D;
using Odyssey.Graphics.Drawing;

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
            const int rings = 1;
            var ringOffsets = new float[rings+1];
            ringOffsets[0] = innerRadiusRatio;
            float delta = (1-innerRadiusRatio)/rings;
            for (int i = 1; i < rings; i++)
                ringOffsets[i] = ringOffsets[i - 1] + delta;
            ringOffsets[rings] = 1.0f;

            Vector3[] vertexArray = innerRadiusRatio > 0
                ? Designer.CreateRingMesh(ellipse, ringOffsets, slices, Matrix.Identity, out indices)
                : Designer.CreateMesh(ellipse, ringOffsets, slices, Matrix.Identity, out indices);
            vertices = vertexArray.Select(v => new VertexPositionNormalTexture(v, Vector3.UnitZ, new Vector2(v.X, v.Y)*tileFactor)).ToArray();
        }

        public static Model New(DirectXDevice device, float semiMajorAxis = 1.0f, float semiMinorAxis = 1.0f, int tessellation = 64, float innerRadiusRatio = 0f, float tileX = 1.0f,
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

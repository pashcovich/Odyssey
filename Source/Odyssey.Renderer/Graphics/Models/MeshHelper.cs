using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Geometry.Primitives;
using SharpDX;

namespace Odyssey.Graphics.Models
{
    public static class MeshHelper
    {


        internal static Vector3 ComputeFaceNormal(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            return new Triangle(v1,v2,v3).CalculateNormal();
        }

        // Helper creates a triangle fan to close the end of a cylinder.
        internal static void CreateCylinderCap(List<VertexPositionNormalTexture> vertices, List<int> indices,
            int tessellation, float height, float radius, Vector2 tileFactor, bool isTop)
        {
            // Create cap indices.
            for (int i = 0; i < tessellation - 2; i++)
            {
                int i1 = (i + 1) % tessellation;
                int i2 = (i + 2) % tessellation;

                if (isTop)
                {
                    SharpDX.Utilities.Swap(ref i1, ref i2);
                }

                int vbase = vertices.Count;
                indices.Add(vbase);
                indices.Add(vbase + i2);
                indices.Add(vbase + i1);
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
                var circleVector = MeshBuilder.GetCircleVector(i, tessellation);
                var position = (circleVector * radius) + (normal * height);
                var textureCoordinate = new Vector2(circleVector.X * textureScale.X + 0.5f,
                    circleVector.Z * textureScale.Y + 0.5f) * tileFactor;

                vertices.Add(new VertexPositionNormalTexture(position, normal, textureCoordinate));
            }
        }
    }
}

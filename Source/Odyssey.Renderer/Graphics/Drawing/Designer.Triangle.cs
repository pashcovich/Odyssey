using System;
using System.Diagnostics.Contracts;
using System.Linq;
using Odyssey.Geometry.Extensions;
using Odyssey.Geometry.Primitives;
using SharpDX.Mathematics;

namespace Odyssey.Graphics.Drawing
{
    public partial class Designer
    {
        private delegate Color4[] TriangleColorShader(IColorResource color, int heightSegments);

        public void FillTriangle(Triangle triangle)
        {
            float[] heightSegments;
            var shader = ChooseShader(Color,  out heightSegments);

            int[] indices;
            Vector3[] vertices = CreateIsoscelesTriangleMesh(triangle, Transform, out indices);
            Color4[] colors = shader(Color, heightSegments.Length);

            var vertexArray = new VertexPositionColor[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
                vertexArray[i] = new VertexPositionColor() { Position = vertices[i], Color = colors[i] };

            shapes.Add(new ShapeMeshDescription() { Vertices = vertexArray, Indices = indices });
        }

        public void DrawTriangle(Triangle triangle,float strokeThickness)
        {
            Vector2[] vertices = triangle.Vertices;
            FillClosedPolyline(vertices, strokeThickness);
        }

        private static Vector3[] CreateIsoscelesTriangleMesh(Triangle triangle, Matrix transform, out int[] indices)
        {
            indices = new[] {0, 1, 2};
            var vertices = triangle.Vertices.Select(p => new Vector3(p.X, p.Y, 0)).ToArray();
            return CheckTransform(transform, vertices);
        }


        static TriangleColorShader ChooseShader(IColorResource color, out float[] heightSegments)
        {
            TriangleColorShader shader;
            switch (color.Type)
            {
                case ColorType.SolidColor:
                    heightSegments = new[] { 0f, 1f };
                    shader = Uniform;
                    break;

                    default:
                    throw new NotSupportedException(string.Format("Triangle does not support this shade type: {0}", color.Type));
            }
            return shader;
        }

        static Color4[] Uniform(IColorResource color, int heightSegments)
        {
            var solidColor = (SolidColor)color;
            return new[] { solidColor.Color, solidColor.Color, solidColor.Color };
        }
    }
}

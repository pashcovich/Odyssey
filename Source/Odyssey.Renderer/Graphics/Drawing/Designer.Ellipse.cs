using System;
using System.Collections.Generic;
using System.Linq;
using Odyssey.Geometry;
using Odyssey.Geometry.Extensions;
using Odyssey.Geometry.Primitives;
using SharpDX;

namespace Odyssey.Graphics.Drawing
{
    public partial class Designer
    {
        internal const int EllipseTessellation = 64;

        private delegate Color4[] EllipseColorShader(Ellipse ellipse, IColorResource color, int numVertex, int slices);

        public void FillEllipseArc(Ellipse ellipse, float radFrom, float radTo, int slices)
        {
            float[] offsets;
            EllipseColorShader shader = ChooseEllipseShader(Color, out offsets);
            Color4[] colors = shader(ellipse, Color, (offsets.Length - 1) * (slices) + 2, slices);
            int[] indices;
            Vector3[] vertices = CreateArcMesh(ellipse, offsets, radFrom, radTo, slices, Transform, out indices);

            var vertexArray = new VertexPositionColor[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
                vertexArray[i] = new VertexPositionColor() { Position = vertices[i], Color = colors[i] };

            shapes.Add(new ShapeMeshDescription() { Vertices = vertexArray, Indices = indices });
        }

        public void DrawEllipseArc(Ellipse ellipse, float radFrom, float radTo, int slices, float lineWidth, float strokeWidth)
        {
            float[] offsets;
            EllipseColorShader shader = ChooseEllipseShader(Color, out offsets);
            int[] indices;
            var vertexList = CreateArcMesh(ellipse, offsets, radFrom, radTo, slices, Matrix.Identity, out indices);
           
            DrawClosedPolyline(vertexList.Select(v => v.XY()), strokeWidth);
        }

        public void FillEllipse(Ellipse ellipse, int slices = EllipseTessellation)
        {
            float[] offsets;
            EllipseColorShader shader = ChooseEllipseShader(Color, out offsets);
            Color4[] colors = shader(ellipse, Color, (offsets.Length - 1) * (slices) + 1, slices);
            int[] indices;
            Vector3[] vertices = CreateMesh(ellipse, offsets, slices, Transform, out indices);
            
            var vertexArray = new VertexPositionColor[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
                vertexArray[i] = new VertexPositionColor() { Position = vertices[i], Color = colors[i] };

            shapes.Add(new ShapeMeshDescription() { Vertices = vertexArray, Indices = indices });
        }

        public void DrawEllipse(Ellipse ellipse, float ringWidth, int slices = 64)
        {
            float innerRadiusRatio = 1 - (ringWidth / ellipse.RadiusX);
            float[] offsets;

            var solidColor = Color as SolidColor;
            if (solidColor != null)
            {
                Color = RadialGradient.New(Color.Name,
                    new[]
                    {
                        new GradientStop(SharpDX.Color.Transparent, 0),
                        new GradientStop(solidColor.Color, innerRadiusRatio),
                        new GradientStop(solidColor.Color, 1)
                    });
            }

            EllipseColorShader shader = ChooseEllipseOutlineShader(innerRadiusRatio, Color, out offsets);
            Color4[] colors = shader(ellipse, Color, offsets.Length * slices, slices);
            int[] indices;
            Vector3[] vertices = CreateRingMesh(ellipse, offsets, slices, Transform, out indices);

            var vertexArray = new VertexPositionColor[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
                vertexArray[i] = new VertexPositionColor() { Position = vertices[i], Color = colors[i] };

            shapes.Add(new ShapeMeshDescription() { Vertices = vertexArray, Indices = indices });
        }


        #region Color Shaders
        private static Color4[] EllipseRadial(Ellipse ellipse, IColorResource color, int numVertex, int slices)
        {
            var gradient = (IGradient)color;
            Color4[] colors = new Color4[numVertex];
            colors[0] = gradient.GradientStops[0].Color;
            int k = 1;
            for (int i = 1; i < colors.Length; i++)
            {
                if (i > 1 && (i - 1) % slices == 0)
                    k++;
                colors[i] = gradient.GradientStops[k].Color;
            }

            return colors;
        }

        private static Color4[] EllipseUniform(Ellipse ellipse, IColorResource color, int numVertex, int slices)
        {
            var solidColor = (SolidColor) color;
            Color4[] colors = new Color4[numVertex];
            for (int i = 0; i < numVertex; i++)
                colors[i] = solidColor.Color;
            return colors;
        }

        private static Color4[] EllipseOutlineRadial(Ellipse ellipse, IColorResource color, int numVertex, int slices)
        {
            var gradient = (IGradient)color;
            Color4[] colors = new Color4[numVertex];
            int k = 0;

            for (int i = 0; i < colors.Length; i++)
            {
                if (i > 0 && i % slices == 0)
                    k++;
                colors[i] = gradient.GradientStops[k].Color;
            }

            return colors;
        }

        private static Color4[] EllipseVertical(Ellipse ellipse, IColorResource color, int numVertex, int slices)
        {
            var gradient = (IGradient) color;
            // Only one ellipse ring is supported
            Color4[] colors = new Color4[numVertex];
            float verticalAxis = ellipse.VerticalAxis;

            var vertices = ellipse.CalculateVertices(slices).ToArray();

            for (int i = 0; i < vertices.Length; i++)
            {
                float r = Math.Abs(vertices[i].Y - ellipse.Center.Y - ellipse.RadiusY) / verticalAxis;
                colors[i + 1] = gradient.GradientStops.Evaluate(r);
            }

            // Color of the center vertex is equal to the color of one of the sides
            colors[0] = colors[1];

            return colors;
        }

        private static Color4[] EllipseHorizontal(Ellipse ellipse, IColorResource color, int numVertex, int slices)
        {
            var gradient = (IGradient)color;
            // Only one ellipse ring is supported
            Color4[] colors = new Color4[numVertex];
            float horizontalAxis = ellipse.HorizontalAxis;

            var vertices = ellipse.CalculateVertices(slices).ToArray();

            for (int i = 0; i < vertices.Length; i++)
            {
                float r = Math.Abs(vertices[i].X - ellipse.Center.X - ellipse.RadiusX) / horizontalAxis;
                colors[i + 1] = gradient.GradientStops.Evaluate(r);
            }

            // Color of the center vertex is equal to the color of the top or bottom vertex
            colors[0] = colors[slices / 4 + 1];

            return colors;
        } 
        #endregion

        static EllipseColorShader ChooseEllipseShader(IColorResource color, out float[] ringOffsets)
        {
            EllipseColorShader shader;
            switch (color.Type)
            {
                case ColorType.SolidColor:
                    ringOffsets = new[] {0f, 1f};
                    shader = EllipseUniform;
                    break;

                case ColorType.LinearGradient:
                    var gLinear = (LinearGradient)color;

                    if (gLinear.GradientStops.Count > 2)
                        throw new NotSupportedException("Ellipse supports only two linear gradient stops");

                    ringOffsets = gLinear.GradientStops.Select(g => g.Offset).ToArray();
                    Vector2 direction = gLinear.EndPoint -gLinear.StartPoint;
                    if (direction == Vector2.UnitY)
                        shader = EllipseVertical;
                    else if (direction == Vector2.UnitX)
                        shader = EllipseHorizontal;
                    else
                        throw new NotSupportedException("Non axis-aligned gradient are not supported");
                    break;

                case ColorType.RadialGradient:
                    ringOffsets = ((IGradient)color).GradientStops.Select(g => g.Offset).ToArray();
                    shader = EllipseRadial;
                    break;

                default:
                    throw new NotSupportedException(String.Format("Gradient `{0}' is not supported by Ellipse", color.Type));
            }
            return shader;
        }

        static EllipseColorShader ChooseEllipseOutlineShader(float innerRadiusRatio, IColorResource color, out float[] ringOffsets)
        {

            EllipseColorShader shader;
            switch (color.Type)
            {
                case ColorType.SolidColor:
                case ColorType.RadialGradient:
                    ringOffsets = ((IGradient)color).GradientStops.Select(g => (float)MathHelper.ConvertRange(0, 1, innerRadiusRatio, 1, g.Offset)).ToArray();
                    shader = EllipseOutlineRadial;
                    break;

                default:
                    throw new NotSupportedException(String.Format("Gradient `{0}' is not supported by EllipseOutline", color.Type));
            }
            return shader;
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
                    indices[indexCount + 1] = baseIndex + i + slices;
                    indices[indexCount + 2] = baseIndex + i + slices + 1;

                    // second face
                    indices[indexCount + 3] = baseIndex + i;
                    indices[indexCount + 4] = baseIndex + i + slices + 1;
                    indices[indexCount + 5] = baseIndex + i + 1;
                    indexCount += 6;
                }
                // Wrap faces
                indices[indexCount - 2] = (r + 1) * slices - 1;
                indices[indexCount - 5] = r * slices;
                indices[indexCount - 4] = (r - 1) * slices;

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

        internal static Vector3[] CreateArcMesh(Ellipse ellipse, float[] ringOffsets, float radFrom, float radTo, int slices, Matrix transform, out int[] indices)
        {
            int rings = ringOffsets.Length;
            var vertices = new Vector3[((rings - 1) * slices) + 2];

            vertices[0] = ellipse.Center.ToVector3();
            var innerRingVertices = Ellipse.CalculateArcVertices(ellipse.Center, ellipse.RadiusX * ringOffsets[1], ellipse.RadiusY * ringOffsets[1], radFrom, radTo, slices).ToArray();
            for (int i = 0; i <= slices; i++)
            {
                vertices[i + 1] = innerRingVertices[i].ToVector3();
            }
            indices = new int[3 * slices];
            //First ring indices
            for (int i = 0; i < slices; i++)
            {
                indices[(3 * i)] = 0;
                indices[(3 * i) + 2] = i + 2;
                indices[ (3 * i) + 1] = i + 1;
            }
            return CheckTransform(transform, vertices);
        }

        internal static Vector3[] CreateMesh(Ellipse ellipse, float[] ringOffsets, int slices, Matrix transform, out int[] indices)
        {
            int rings = ringOffsets.Length;
            var vertices = new Vector3[((rings - 1) * slices) + 1];

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
                return CheckTransform(transform, vertices);
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
                    indices[baseIndex + indexCount + 2] = j + i + 1;
                    indices[baseIndex + indexCount + 1] = k + i + 1;
                    // second face
                    indices[baseIndex + indexCount + 3] = j + i + 2;
                    indices[baseIndex + indexCount + 5] = k + i + 1;
                    indices[baseIndex + indexCount + 4] = k + i + 2;

                    indexCount += 6;
                }
                // Wrap faces
                indices[baseIndex + indexCount - 3] = 1 + (r - 1) * slices;
                indices[baseIndex + indexCount - 6] = r * slices + 1;
                SharpDX.Utilities.Swap(ref indices[baseIndex + indexCount - 1], ref indices[baseIndex + indexCount - 2]);

            }

            return CheckTransform(transform, vertices);
        }

        static void TriangulateEllipseFirstRing(int slices, ref int[] indices, int startIndex = 0)
        {
            //First ring indices
            for (int i = 0; i < slices; i++)
            {
                indices[startIndex + (3 * i)] = 0;
                indices[startIndex + (3 * i) + 2] = i + 2;
                indices[startIndex + (3 * i) + 1] = i + 1;
            }
            indices[startIndex + 3 * slices - 1] = 1;
        }
    }
}

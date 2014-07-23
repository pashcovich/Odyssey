using System;
using System.Diagnostics.Contracts;
using System.Linq;
using Odyssey.Geometry.Primitives;
using SharpDX;
using Odyssey.Geometry;

namespace Odyssey.Graphics.Shapes
{
    public partial class Designer
    {
        private delegate Color4[] EllipseColorShader(Ellipse ellipse, GradientStopCollection gradientStops, int numVertex, int slices);

        public void FillEllipse(Ellipse ellipse, IGradient gradient, int slices = 64)
        {
            float[] offsets = gradient.GradientStops.Select(g => g.Offset).ToArray();
            int segments = offsets.Length;

            EllipseColorShader shader = ChooseEllipseShader(gradient);
            
            Color4[] colors = shader(ellipse, gradient.GradientStops, (segments - 1) * (slices) + 1, slices);
            int[] indices;
            VertexPositionColor[] vertices = CreateEllipseMesh(ellipse, slices, colors, out indices, offsets);

            shapes.Add(new ShapeMeshDescription() { Vertices = vertices, Indices = indices });
        }

        public void DrawEllipse(Ellipse ellipse, float ringWidth, IGradient gradient, int slices = 64)
        {
            float innerRadiusRatio = 1 - (ringWidth/ellipse.RadiusX); 
            float[] offsets = gradient.GradientStops.Select(g => (float)MathHelper.ConvertRange(0,1, innerRadiusRatio, 1, g.Offset)).ToArray();
            int segments = offsets.Length;
            EllipseColorShader shader = ChooseEllipseOutlineShader(gradient);
            Color4[] colors = shader(ellipse, gradient.GradientStops, segments * slices, slices);
            int[] indices;
            VertexPositionColor[] vertices = CreateEllipseRing(ellipse, slices, colors, out indices, offsets);
            shapes.Add(new ShapeMeshDescription() { Vertices = vertices, Indices = indices });
        }

        private static Color4[] EllipseRadial(Ellipse ellipse, GradientStopCollection gradientStops, int numVertex, int slices)
        {
            Color4[] colors = new Color4[numVertex];
            colors[0] = gradientStops[0].Color;
            int k = 1;
            for (int i = 1; i < colors.Length; i++)
            {

                if (i > 1 && (i - 1)%slices == 0)
                    k++;
                colors[i] = gradientStops[k].Color;
            }

            return colors;
        }

        private static Color4[] EllipseUniform(Ellipse ellipse, GradientStopCollection gradientStops, int numVertex, int slices)
        {
            Color4[] colors = new Color4[numVertex];
            for (int i = 0; i < numVertex; i++)
                colors[i] = gradientStops[0].Color;
            return colors;
        }

        private static Color4[] EllipseOutlineRadial(Ellipse ellipse, GradientStopCollection gradientStops, int numVertex, int slices)
        {
            Color4[] colors = new Color4[numVertex];
            int k = 0;

            for (int i = 0; i < colors.Length; i++)
            {
                if (i>0 && i % slices == 0)
                    k++;
                colors[i] = gradientStops[k].Color;
            }

            return colors;
        }

        private static Color4[] EllipseVertical(Ellipse ellipse, GradientStopCollection gradientStops, int numVertex, int slices)
        {
            // Only one ellipse ring is supported
            Color4[] colors = new Color4[numVertex];
            float verticalAxis = ellipse.VerticalAxis;

            var vertices = ellipse.CalculateVertices(slices).ToArray();

            for (int i = 0; i < vertices.Length; i++)
            {
                float r = Math.Abs(vertices[i].Y - ellipse.Center.Y - ellipse.RadiusY)/verticalAxis;
                colors[i+1] = gradientStops.Evaluate(r);
            }

            // Color of the center vertex is equal to the color of one of the sides
            colors[0] = colors[1];

            return colors;
        }

        private static Color4[] EllipseHorizontal(Ellipse ellipse, GradientStopCollection gradientStops, int numVertex, int slices)
        {
            // Only one ellipse ring is supported

            Color4[] colors = new Color4[numVertex];
            float horizontalAxis = ellipse.HorizontalAxis;

            var vertices = ellipse.CalculateVertices(slices).ToArray();

            for (int i = 0; i < vertices.Length; i++)
            {
                float r = Math.Abs(vertices[i].X - ellipse.Center.X - ellipse.RadiusX) / horizontalAxis;
                colors[i + 1] = gradientStops.Evaluate(r);
            }

            // Color of the center vertex is equal to the color of the top or bottom vertex
            colors[0] = colors[slices/4+1];

            return colors;
        }


        static VertexPositionColor[] CreateEllipseRing(Ellipse ellipse, int slices, Color4[] colors, out int[] indices, float[] ringOffsets)
        {
            int rings = ringOffsets.Length;
            VertexPositionColor[] vertices = new VertexPositionColor[rings * slices];
            indices = new int[3 * slices * 2 * (rings - 1)];

            var innerRingVertices = Ellipse.CalculateVertices(ellipse.Center, ellipse.RadiusX * ringOffsets[0], ellipse.RadiusY * ringOffsets[0], slices).ToArray();
            for (int i = 0; i < slices; i++)
            {
                vertices[i] = new VertexPositionColor(innerRingVertices[i].ToVector3(), colors[i]);
            }
            
            int indexCount = 0;
            for (int r = 1; r < rings; r++)
            {
                // Other rings vertices
                var outerRingVertices = Ellipse.CalculateVertices(ellipse.Center, ellipse.RadiusX * ringOffsets[r], ellipse.RadiusY * ringOffsets[r], slices).ToArray();
                for (int i = 0; i < slices; i++)
                {
                    vertices[(r * slices) + i] = new VertexPositionColor(outerRingVertices[i].ToVector3(), colors[(r * slices) + i]);
                }

                // Other rings indices
                int baseIndex = (r-1)*slices;

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
                indices[indexCount - 4] = r *slices;
                indices[indexCount - 5] = (r -1)* slices;
                indices[indexCount - 1] = (r + 1)*slices - 1;
            }
            

            return vertices;

        }
        static VertexPositionColor[] CreateEllipseMesh(Ellipse ellipse, int slices, Color4[] colors,
            out int[] indices, float[] ringOffsets)
        {
            int rings = ringOffsets.Length;
            VertexPositionColor[] vertices = new VertexPositionColor[((rings - 1) * slices) + 1];

            vertices[0] = new VertexPositionColor(ellipse.Center.ToVector3(), colors[0]);

            // First ring vertices
            // ringOffsets[0] is assumed to be the center
            // ringOffsets[1] the first ring
            var innerRingVertices = Ellipse.CalculateVertices(ellipse.Center, ellipse.RadiusX * ringOffsets[1], ellipse.RadiusY* ringOffsets[1], slices).ToArray();
            for (int i = 0; i < slices; i++)
            {
                vertices[i + 1] = new VertexPositionColor(innerRingVertices[i].ToVector3(), colors[i + 1]);
            }

            indices = new int[3 * slices * ((2 * (rings - 2)) + 1)];

            TriangulateEllipseFirstRing(slices, ref indices);

            int indexCount = 0;
            int baseIndex = 3 * slices;
            for (int r = 1; r < rings - 1; r++)
            {
                // Other rings vertices
                var outerRingVertices = Ellipse.CalculateVertices(ellipse.Center, ellipse.RadiusX * ringOffsets[r+1], ellipse.RadiusY * ringOffsets[r+1], slices).ToArray();
                for (int i = 0; i < slices; i++)
                {
                    vertices[(r * slices) + i + 1] = new VertexPositionColor(outerRingVertices[i].ToVector3(), colors[(r * slices) + i + 1]);
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
                indices[baseIndex + indexCount - 3] = 1 + (r - 1)*slices;
                indices[baseIndex + indexCount - 6] = r*slices + 1;
            }
            SharpDX.Utilities.Swap(ref indices[baseIndex+indexCount -1], ref indices[baseIndex+indexCount-2]);
            return vertices;
        }

        static void TriangulateEllipseFirstRing(int slices, ref int[] indices, int startIndex = 0)
        {
            // First ring indices
            for (int i = 0; i < slices; i++)
            {
                indices[startIndex + (3 * i)] = 0;
                indices[startIndex + (3 * i) + 1] = (ushort)(i + 2);
                indices[startIndex + (3 * i) + 2] = (ushort)(i + 1);
            }
            indices[(slices * 3) - 2] = 1;
        }

        static EllipseColorShader ChooseEllipseShader(IGradient gradient)
        {
            EllipseColorShader shader;
            switch (gradient.Type)
            {
                case GradientType.Uniform:
                    shader = EllipseUniform;
                    break;

                case GradientType.Linear:
                    var gLinear = (LinearGradient)gradient;

                    if (gLinear.GradientStops.Count > 2)
                        throw new NotSupportedException("Ellipse supports only two linear gradient stops");

                    Vector2 direction = gLinear.EndPoint -gLinear.StartPoint;
                    if (direction == Vector2.UnitY)
                        shader = EllipseVertical;
                    else if (direction == Vector2.UnitX)
                        shader = EllipseHorizontal;
                    else
                        throw new NotSupportedException("Non axis-aligned gradient are not supported");
                    break;

                case GradientType.Radial:
                    shader = EllipseRadial;
                    break;

                default:
                    throw new NotSupportedException(string.Format("Gradient `{0}' is not supported by Ellipse", gradient.Type));
            }
            return shader;
        }

        static EllipseColorShader ChooseEllipseOutlineShader(IGradient gradient)
        {
            EllipseColorShader shader;
            switch (gradient.Type)
            {
                case GradientType.Uniform:
                    shader = EllipseUniform;
                    break;
                case GradientType.Radial:
                    shader = EllipseOutlineRadial;
                    break;

                default:
                    throw new NotSupportedException(string.Format("Gradient `{0}' is not supported by EllipseOutline", gradient.Type));
            }
            return shader;
        }
    }
}

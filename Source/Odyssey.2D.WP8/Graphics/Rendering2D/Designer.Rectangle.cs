using Odyssey.Graphics.Rendering2D.Shapes;
using Odyssey.UserInterface.Style;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Graphics.Rendering2D
{
    public partial class Designer
    {
        public void FillRectangle(RectangleF rectangle, IGradient gradient)
        {
            Contract.Requires<ArgumentException>(!rectangle.IsEmpty);
            Contract.Requires<ArgumentException>(gradient != null);

            float[] widthSegments;
            float[] heightSegments;

            switch (gradient.Type)
            {
                default:
                case GradientType.Uniform:
                    widthSegments = new float[] { 0, 1 };
                    heightSegments = new float[] { 0, 1 };
                    break;

                case GradientType.LinearVerticalGradient:
                    widthSegments = (from gs in gradient.GradientStops
                               select gs.Offset).ToArray();
                    heightSegments = new float[] { 0, 1 };
                    break;
            }

            ushort[] indices;
            Vector4[] vertices = CreateRectangleMesh(rectangle, widthSegments, heightSegments, out indices);
            Color4[] colors = new Color4[vertices.Length];
            for (int i = 0; i < colors.Length; i++ )
                colors[i] = Color.BlueViolet;

            VertexPositionColor[] vertexArray = (from vertex in vertices
                                               from color in colors
                                               select new VertexPositionColor() { Position = vertex, Color = color }).ToArray();

            shapes.Add(new ShapeMeshDescription() { Vertices = vertexArray, Indices = indices });
        }

        public Vector4[] CreateRectangleMesh(RectangleF rectangle, float[] widthSegments, float[] heightSegments, out ushort[] indices)
        {
            Contract.Requires<ArgumentException>(widthSegments != null);
            Contract.Requires<ArgumentException>(heightSegments != null);
            Contract.Requires<ArgumentException>(widthSegments.Length >= 2);
            Contract.Requires<ArgumentException>(heightSegments.Length >= 2);

            int rows = widthSegments.Length - 1;
            int columns = heightSegments.Length - 1;

            Vector4[] vertices = new Vector4[(1 + rows) * (1 + columns)];
            indices = new ushort[rows * columns * 6];
            Vector4 topLeftVertex = new Vector4(rectangle.X, rectangle.Y, 0, 1.0f);

            int vertexCount = 0, indexCount = 0;
            float x = rectangle.X;
            float y = rectangle.Y;
            float z = 0;
            float width = rectangle.Width;
            float height = rectangle.Height;

            // Compute vertices, one row at a time
            for (int i = 0; i < rows + 1; i++)
            {
                for (int j = 0; j < columns + 1; j++)
                {
                    float hOffset = widthSegments[j] * width;
                    float vOffset = heightSegments[i] * height;

                    vertices[vertexCount] = new Vector4(x + hOffset, y - vOffset, z, 1.0f);

                    if (i < rows && j < columns)
                    {
                        indices[indexCount] = (ushort)(vertexCount + 1);
                        indices[indexCount + 1] = (ushort)(vertexCount);
                        indices[indexCount + 2] = (ushort)(vertexCount + columns + 1);

                        indices[indexCount + 3] = (ushort)(indices[indexCount + 2] + 1);
                        indices[indexCount + 4] = indices[indexCount];
                        indices[indexCount + 5] = indices[indexCount + 2];

                        indexCount += 6;
                    }

                    vertexCount++;
                }
            }

            return vertices;
        }
    }
}

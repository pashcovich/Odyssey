using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Geometry;
using SharpDX;

namespace Odyssey.Graphics.Drawing
{
    public partial class Designer
    {
        public void DrawPolyLine(IEnumerable<Vector3> points, float strokeThickness, IGradient gradient)
        {
            Vector3[] pointArray = points as Vector3[] ?? points.ToArray();
            for (int i = 0; i < pointArray.Length -1 ; i++)
            {
                Vector3 p0 = pointArray[i];
                Vector3 p1 = pointArray[i + 1];
                float d = Vector3.Distance(p0, p1);

                float xDiff = p1.X - p0.X;
                float yDiff = p1.Y - p0.Y;
                float angle;
                if (MathHelper.IsCloseToZero(xDiff))
                    angle = 0;
                else
                    angle = (float)Math.Atan2(yDiff, xDiff);
                Matrix transform = Matrix.RotationYawPitchRoll(0, 0, -angle) * Matrix.Translation(p0);

                FillRectangle(new RectangleF(-strokeThickness/2, 0, strokeThickness, d), gradient, transform);
            }
        }
    }
}

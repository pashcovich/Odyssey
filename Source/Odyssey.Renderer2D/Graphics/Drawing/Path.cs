using System;
using SharpDX.Mathematics;

namespace Odyssey.Graphics.Drawing
{
    public class Path : Shape
    {
        public PathGeometry Data { get; set; }

        public override bool Contains(Vector2 cursorLocation)
        {
            return false;
        }

        public override void Render()
        {
            Device.Transform = Transform;
            if (Fill != null)
            {
                Fill.Transform = Matrix3x2.Scaling(Width, Height) * Transform;
                Device.FillGeometry(Data, Fill);
            }
            if (Stroke != null)
                Device.DrawGeometry(Data, Stroke,StrokeThickness);
            Device.Transform = Matrix3x2.Identity;
        }

        protected override void OnInitializing(EventArgs e)
        {
            if (Data == null)
                throw new InvalidOperationException("'Data' cannot be null");
            ToDispose(Data).Initialize();
        }
    }
}
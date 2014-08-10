using Odyssey.UserInterface.Controls;
using SharpDX;

namespace Odyssey.Graphics.Shapes
{
    public class Rectangle : Shape
    {
        public override bool Contains(Vector2 cursorLocation)
        {
            return BoundingRectangle.Contains(cursorLocation);
        }

        public override void Render()
        {
            Fill.Transform = Matrix3x2.Scaling(Width, Height) * Transform;
            Device.FillRectangle(this, Fill);
            Device.DrawRectangle(this, Stroke, StrokeThickness);
        }
    }
}
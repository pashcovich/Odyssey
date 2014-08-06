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
            Device.FillRectangle(this, Fill);
            Device.DrawRectangle(this, Stroke, StrokeThickness);
        }

        protected override void OnInitializing(ControlEventArgs e)
        {
            foreach (var resource in ShapeInitializer.CreateResources(Device, this))
                ToDispose(resource);
        }
    }
}
using Odyssey.UserInterface.Controls;
using SharpDX;
using SharpDX.Direct2D1;

namespace Odyssey.Graphics
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
            Device.DrawRectangle(this, Stroke);
        }

        protected override void OnInitializing(ControlEventArgs e)
        {
            base.OnInitializing(e);
            var initializer = new ShapeInitializer(Device);
            initializer.Initialize(this);
            foreach (var resource in initializer.CreatedResources)
                ToDispose(resource);
        }
    }
}
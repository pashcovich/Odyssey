using Odyssey.UserInterface.Controls;
using Odyssey.UserInterface.Style;
using SharpDX.Direct2D1;
using Brush = Odyssey.Graphics.Shapes.Brush;

namespace Odyssey.Graphics
{
    public class Rectangle : RectangleBase, IShapeD2D
    {
        public Brush Fill { get; set; }

        public Brush Stroke { get; set; }

        public override void Render()
        {
            Device.FillRectangle(this, Fill);
            Device.DrawRectangle(this, Stroke);
        }

        protected override void OnInitializing(ControlEventArgs e)
        {
            base.OnInitializing(e);
            var initializer = new ShapeInitializer<Rectangle>(Device);
            initializer.Initialize(this);
            foreach (var resource in initializer.CreatedResources)
                ToDispose(resource);
        }
    }
}
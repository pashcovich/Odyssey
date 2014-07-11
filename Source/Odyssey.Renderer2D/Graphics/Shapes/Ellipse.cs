using Odyssey.UserInterface.Controls;
using SharpDX;
using SharpDX.Direct2D1;
using System;

namespace Odyssey.Graphics
{
    public class Ellipse : Shape
    {
        private SharpDX.Direct2D1.Ellipse ellipse;

        public override bool Contains(Vector2 cursorLocation)
        {
            return BoundingRectangle.Contains(cursorLocation);
        }

        public override void Render()
        {
            var context = (DeviceContext)Device;
            context.FillEllipse(ellipse, Fill);
            context.DrawEllipse(ellipse, Stroke);
        }

        protected override void OnInitializing(ControlEventArgs e)
        {
            base.OnInitializing(e);
            var initializer = new ShapeInitializer(Device);
            initializer.Initialize(this);

            foreach (var resource in initializer.CreatedResources)
                ToDispose(resource);
        }

        protected override void OnPositionChanged(EventArgs e)
        {
            base.OnPositionChanged(e);
            ellipse.Point = new SharpDX.Vector2(BoundingRectangle.X, BoundingRectangle.Y);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            ellipse = new SharpDX.Direct2D1.Ellipse(BoundingRectangle.Center, BoundingRectangle.Width / 2, BoundingRectangle.Height / 2);
        }
    }
}
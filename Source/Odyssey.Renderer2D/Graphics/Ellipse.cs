using Odyssey.UserInterface.Controls;
using SharpDX.Direct2D1;
using System;
using Brush = Odyssey.Graphics.Shapes.Brush;

namespace Odyssey.Graphics
{
    public class Ellipse : EllipseBase, IShapeD2D
    {
        private SharpDX.Direct2D1.Ellipse ellipse;

        public Brush Fill { get; set; }

        public Brush Stroke { get; set; }

        public override void Render()
        {
            var context = (DeviceContext)Device;
            context.FillEllipse(ellipse, Fill);
            context.DrawEllipse(ellipse, Stroke);
        }

        protected override void OnInitializing(ControlEventArgs e)
        {
            base.OnInitializing(e);
            var initializer = new ShapeInitializer<Ellipse>(Device);
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
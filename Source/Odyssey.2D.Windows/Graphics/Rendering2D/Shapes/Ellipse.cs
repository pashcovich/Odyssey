using Odyssey.Engine;
using Odyssey.UserInterface.Controls;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Graphics.Rendering2D.Shapes
{
    public class Ellipse : EllipseBase, IShapeD2D
    {
        public Brush Fill { get; set; }
        public Brush Stroke { get; set; }

        SharpDX.Direct2D1.Ellipse ellipse;

        public override void Initialize(IDirectXProvider directX)
        {
            DeviceContext d2dContext = directX.Direct2D.Context;

            switch (FillShader.Type)
            {
                default:
                    Stroke = ToDispose(new SolidColorBrush(d2dContext, StrokeShader.GradientStops[0].Color));
                    Fill = ToDispose(new SolidColorBrush(d2dContext, FillShader.GradientStops[0].Color));
                    break;
            }

            OnInitialized(new ControlEventArgs(this));
        }

        public override void UpdateExtents(SharpDX.RectangleF rectangle)
        {
            base.UpdateExtents(rectangle);
            ellipse = new SharpDX.Direct2D1.Ellipse(BoundingRectangle.Center, BoundingRectangle.Width/2, BoundingRectangle.Height/2);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            ellipse = new SharpDX.Direct2D1.Ellipse(BoundingRectangle.Center, BoundingRectangle.Width/2, BoundingRectangle.Height/2);
        }

        protected override void OnPositionChanged(EventArgs e)
        {
            base.OnPositionChanged(e);
            ellipse.Point = new SharpDX.Vector2(BoundingRectangle.X, BoundingRectangle.Y);
        }
        
        public override void Render(IDirectXTarget target)
        {
            DeviceContext context = target.Direct2D.Context;
            context.FillEllipse(ellipse, Fill);
            context.DrawEllipse(ellipse, Stroke);
         
        }
    }
}

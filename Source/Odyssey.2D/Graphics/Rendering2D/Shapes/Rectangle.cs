using Odyssey.Engine;
using Odyssey.UserInterface;
using Odyssey.UserInterface.Controls;
using SharpDX;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Graphics.Rendering2D.Shapes
{
    public class Rectangle : Shape
    {
        RectangleF rectangleF;

        public Rectangle(string id)
            : base(id)
        { }

        public override void Initialize(IDirectXProvider directX)
        {
            DeviceContext d2dContext = directX.Direct2D.Context;
            
            switch (Fill.Type)
            {
                default:
                    StrokeBrush = ToDispose(new SolidColorBrush(d2dContext, Stroke.GradientStops[0].Color));
                    FillBrush = ToDispose(new SolidColorBrush(d2dContext, Fill.GradientStops[0].Color));
                    break;
            }
        }

        public override void Render(IDirectXTarget target)
        {
            DeviceContext context = target.Direct2D.Context;
            context.FillRectangle(rectangleF, FillBrush);
            context.DrawRectangle(rectangleF, StrokeBrush);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
 	        base.OnSizeChanged(e);
            rectangleF = (RectangleF)this;
        }

        protected override void OnPositionChanged(EventArgs e)
        {
            base.OnPositionChanged(e);
            rectangleF = (RectangleF)this;
        }

        public override bool IntersectTest(Vector2 cursorLocation)
        {
            return rectangleF.Contains(cursorLocation);
        }

        public override void UpdateExtents(RectangleF rectangle)
        {
            AbsolutePosition = new Vector2(rectangle.X, rectangle.Y);
            Width = rectangle.Width;
            Height = rectangle.Height;
            rectangleF = rectangle;
        }

        


    }
}

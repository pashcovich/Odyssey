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
    public class Rectangle : RectangleBase, IShapeD2D
    {
        Brush IShapeD2D.FillBrush
        {
            get { return FillBrush; }
        }
        Brush IShapeD2D.StrokeBrush
        {
            get { return StrokeBrush; }
        }

        protected Brush FillBrush { get; set; }
        protected Brush StrokeBrush { get; set; }

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
            context.FillRectangle(BoundingRectangle, FillBrush);
            context.DrawRectangle(BoundingRectangle, StrokeBrush);
        }
    }
}
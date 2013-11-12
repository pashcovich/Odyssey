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
        public Brush Fill { get; set; }
        public Brush Stroke { get; set; }

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

        public override void Render(IDirectXTarget target)
        {
            DeviceContext context = target.Direct2D.Context;
            context.FillRectangle(BoundingRectangle, Fill);
            context.DrawRectangle(BoundingRectangle, Stroke);
        }
    }
}
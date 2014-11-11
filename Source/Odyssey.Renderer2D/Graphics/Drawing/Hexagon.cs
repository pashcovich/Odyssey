using System;
using Odyssey.Geometry.Primitives;
using Odyssey.UserInterface.Controls;
using SharpDX.Mathematics;

namespace Odyssey.Graphics.Drawing
{
    public class Hexagon : PolygonBase
    {
        private const int Sides = 6;
        protected override Polygon Polygon { get; set; }

        protected override void OnInitializing(EventArgs e)
        {
            base.OnInitializing(e);
            Polygon = Polygon.New(new Vector2(Width/2, Height/2), Width/2, Sides);
            PolygonGeometry = ToDispose(PolygonGeometry.New(string.Format("Poly.{0}", Name), Device, Polygon, FigureBegin.Filled));
        }

        protected override void OnPositionChanged(EventArgs e)
        {
            base.OnPositionChanged(e);
            Polygon = Polygon.New(BoundingRectangle.Center, BoundingRectangle.Width/2, Sides);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            Polygon = Polygon.New(BoundingRectangle.Center, BoundingRectangle.Width/2, Sides);
        }
    }
}
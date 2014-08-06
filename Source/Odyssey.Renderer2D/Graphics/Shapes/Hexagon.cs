using Odyssey.Graphics.Shapes;
using Odyssey.UserInterface.Controls;
using System;

namespace Odyssey.Graphics
{
    public class Hexagon : PolygonBase
    {
        private const int Sides = 6;
        private PolygonGeometry polygonGeometry;

        protected override Geometry.Primitives.Polygon Polygon { get; set; }

        public override void Render()
        {
            Device.FillGeometry(polygonGeometry, Fill);
            Device.DrawGeometry(polygonGeometry, Stroke);
        }

        protected override void OnInitializing(ControlEventArgs e)
        {
            base.OnInitializing(e);
            polygonGeometry = ToDispose(PolygonGeometry.New(string.Format("PL.{0}", Name), Device, BoundingRectangle.Center, BoundingRectangle.Width / 2, Sides,
                FigureBegin.Filled));

            var initializer = new ShapeInitializer(Device);
            initializer.Initialize(this);
            foreach (var resource in initializer.CreatedResources)
                ToDispose(resource);
        }

        protected override void OnPositionChanged(EventArgs e)
        {
            base.OnPositionChanged(e);
            Polygon = Geometry.Primitives.Polygon.New(BoundingRectangle.Center, BoundingRectangle.Width, Sides);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            Polygon = Geometry.Primitives.Polygon.New(BoundingRectangle.Center, BoundingRectangle.Width, Sides);
        }
    }
}
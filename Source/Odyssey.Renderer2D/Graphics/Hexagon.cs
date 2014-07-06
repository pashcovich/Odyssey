using System;
using Odyssey.Graphics.Shapes;
using Odyssey.UserInterface.Controls;
using Brush = Odyssey.Graphics.Shapes.Brush;


namespace Odyssey.Graphics
{
    public class Hexagon : PolygonBase, IShapeD2D
    {
        private PolygonGeometry polygonGeometry;

        public Brush Fill { get; set; }
        public Brush Stroke { get; set; }

        protected const int Sides = 6;

        public override void Render()
        {
            Device.FillGeometry(polygonGeometry, Fill);
            Device.DrawGeometry(polygonGeometry, Stroke);
        }

        protected override void OnInitializing(ControlEventArgs e)
        {
            base.OnInitializing(e);
            polygonGeometry = ToDispose(PolygonGeometry.New(Device, BoundingRectangle.Center, BoundingRectangle.Width/2, Sides,
                FigureBegin.Filled));

            var initializer = new ShapeInitializer<Hexagon>(Device);
            initializer.Initialize(this);
            foreach (var resource in initializer.CreatedResources)
                ToDispose(resource);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            Polygon = Geometry.Primitives.Polygon.New(BoundingRectangle.Center, BoundingRectangle.Width, Sides);
        }

        protected override void OnPositionChanged(EventArgs e)
        {
            base.OnPositionChanged(e);
            Polygon = Geometry.Primitives.Polygon.New(BoundingRectangle.Center, BoundingRectangle.Width, Sides);
        }

        protected override Geometry.Primitives.Polygon Polygon { get; set; }
    }
}

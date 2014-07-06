using Odyssey.Graphics.Shapes;
using Odyssey.UserInterface.Controls;
using Odyssey.UserInterface.Style;
using SharpDX;
using SharpDX.Direct2D1;
using Brush = Odyssey.Graphics.Shapes.Brush;
using FigureBegin = Odyssey.Graphics.Shapes.FigureBegin;
using FigureEnd = Odyssey.Graphics.Shapes.FigureEnd;

namespace Odyssey.Graphics
{
    public class CutCornerRectangle : CutCornerRectangleBase, IShapeD2D
    {
        private PolyLine shape;
        private Matrix3x2 transform;

        public Brush Fill { get; set; }

        public Brush Stroke { get; set; }

        protected PolyLine Shape { get { return shape; } set { shape = value; } }

        protected Matrix3x2 Transform { get { return transform; } }

        public override void Render()
        {
            Device.SetTransform(transform);
            Device.FillGeometry(shape, Fill);
            Device.DrawGeometry(shape, Stroke);
        }

        protected override void Measure()
        {
            transform = Matrix3x2.Translation(AbsolutePosition);
        }

        protected override void OnInitializing(ControlEventArgs e)
        {
            base.OnInitializing(e);
            Vector2[] points =
            {
                new Vector2(CutCornerLength, 0),
                new Vector2(Width - CutCornerLength, 0),
                new Vector2(Width, CutCornerLength),
                new Vector2(Width, Height - CutCornerLength),
                new Vector2(Width - CutCornerLength, Height),
                new Vector2(CutCornerLength, Height),
                new Vector2(0, Height - CutCornerLength),
                new Vector2(0, CutCornerLength)
            };

            shape = ToDispose(PolyLine.New(Device, points, FigureBegin.Filled, FigureEnd.Closed));

            var initializer = new ShapeInitializer<CutCornerRectangle>(Device);
            initializer.Initialize(this);
            foreach (var resource in initializer.CreatedResources)
                ToDispose(resource);
        }
    }
}
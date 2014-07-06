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
    public class DiagonalCutCornerRectangle : CutCornerRectangle
    {

        protected override void OnInitializing(ControlEventArgs e)
        {
            base.OnInitializing(e);
            Vector2[] points =
            {
                new Vector2(0, 0),
                new Vector2(Width - CutCornerLength, 0),
                    new Vector2(Width, Height - CutCornerLength),
                    new Vector2(Width, Height),
                    new Vector2(CutCornerLength, Height),
                    new Vector2(0, CutCornerLength)
            };
            
            Shape = PolyLine.New(Device, points, FigureBegin.Filled, FigureEnd.Closed);

            var initializer = new ShapeInitializer<DiagonalCutCornerRectangle>(Device);
            initializer.Initialize(this);
            foreach (var resource in initializer.CreatedResources)
                ToDispose(resource);

        }

    }

}

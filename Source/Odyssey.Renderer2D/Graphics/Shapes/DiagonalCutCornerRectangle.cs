using Odyssey.UserInterface.Controls;
using SharpDX;

namespace Odyssey.Graphics.Shapes
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

            var initializer = new ShapeInitializer(Device);
            initializer.Initialize(this);
            foreach (var resource in initializer.CreatedResources)
                ToDispose(resource);
        }
    }
}
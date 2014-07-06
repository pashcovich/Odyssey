#region Using Directives

using Odyssey.UserInterface;
using Odyssey.UserInterface.Controls;
using Odyssey.UserInterface.Style;
using SharpDX;

#endregion Using Directives

namespace Odyssey.Graphics
{
    public abstract class ShapeBase : UIElement, IShape
    {
        public static Color4 DefaultFillColor = Color.MidnightBlue;
        public static Color4 DefaultStrokeColor = Color.DimGray;

        public IGradient FillShader { get; set; }

        RectangleF IShape.BoundingRectangle
        {
            get { return BoundingRectangle; }
        }

        public IGradient StrokeShader { get; set; }

        public static TShape FromControl<TShape>(Control control, string shapeName)
            where TShape : UIElement, IShape, new()
        {
            TShape shape = new TShape()
            {
                Name = shapeName,
                Width = control.Width,
                Height = control.Height,
                AbsolutePosition = control.AbsolutePosition,
                Margin = control.Margin,
                Parent = control
            };

            return shape;
        }
    }
}
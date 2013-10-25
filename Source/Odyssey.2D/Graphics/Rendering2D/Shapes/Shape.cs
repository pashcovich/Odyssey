using Odyssey.Engine;
using Odyssey.UserInterface;
using Odyssey.UserInterface.Style;
using SharpDX;
using SharpDX.Direct2D1;
using System.Collections.Generic;

namespace Odyssey.Graphics.Rendering2D.Shapes
{
    public abstract class Shape : UIElement, IRenderable
    {
        public IGradient Fill { get; set; }
        public IGradient Stroke { get; set; }

        protected Brush FillBrush { get; set; }
        protected Brush StrokeBrush { get; set; }

        public abstract void Initialize(IDirectXProvider directX);
        public abstract void Render(IDirectXTarget target);

        public abstract void UpdateExtents(RectangleF rectangle);

        public Shape(string id) : base (id)
        { }

    }
}

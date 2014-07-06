using Odyssey.Engine;
using Odyssey.Graphics;
using SharpDX;
using System;
using System.Linq;
using Rectangle = Odyssey.Graphics.Rectangle;

namespace Odyssey.UserInterface.Controls
{
    public class Button : ButtonBase
    {
        public Button()
        {
            Label = ToDispose(new Label());
        }

        public override bool Contains(Vector2 cursorLocation)
        {
            return BoundingRectangle.Contains(cursorLocation);
        }

        public override void Render()
        {
            foreach (IShape shape in ActiveStyle)
                shape.Render();
            base.Render();
        }

        protected override void OnInitializing(ControlEventArgs e)
        {
            base.OnInitializing(e);
            Rectangle rEnabled = ToDispose(ShapeBase.FromControl<Rectangle>(this,
                    string.Format("{0}_{1}_rectangle", Name, ControlStatus.Enabled)));
            Rectangle rHighlighted = ToDispose(ShapeBase.FromControl<Rectangle>(this,
                    string.Format("{0}_{1}_rectangle", Name, ControlStatus.Highlighted)));

            ShapeMap.Add(ControlStatus.Enabled, new[] { rEnabled });

            if (Description.Highlighted != null)
                ShapeMap.Add(ControlStatus.Highlighted, new[] { rHighlighted });
        }
    }
}
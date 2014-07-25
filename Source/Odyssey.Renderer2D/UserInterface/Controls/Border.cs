using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Graphics.Shapes;
using SharpDX;
using Rectangle = Odyssey.Graphics.Shapes.Rectangle;

namespace Odyssey.UserInterface.Controls
{
    public class Border : ContentControl
    {
        protected const string ControlTag = "Border";

        public Border() : base(ControlTag, ControlTag)
        {
        }

        public override bool Contains(Vector2 cursorLocation)
        {
            return BoundingRectangle.Contains(cursorLocation);
        }

        protected override void OnInitializing(ControlEventArgs e)
        {
            base.OnInitializing(e);
            Rectangle rEnabled = ToDispose(Shape.FromControl<Rectangle>(this,
                string.Format("{0}_{1}_rectangle", Name, ControlStatus.Enabled)));

            ShapeMap.Add(ControlStatus.Enabled, new[] { rEnabled });

        }

        public override void Render()
        {
            foreach (IShape shape in ActiveStyle)
                shape.Render();
            base.Render();
        }
    }
}

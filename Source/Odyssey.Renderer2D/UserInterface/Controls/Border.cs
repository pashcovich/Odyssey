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

        public override void Render()
        {
            foreach (IShape shape in VisualState[ActiveStatus])
                shape.Render();
            base.Render();
        }
    }
}

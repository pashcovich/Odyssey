using Odyssey.Graphics;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rectangle = Odyssey.Graphics.Rectangle;

namespace Odyssey.UserInterface.Controls
{
    public class StackPanel : StackPanelBase
    {
        public override bool Contains(SharpDX.Vector2 cursorLocation)
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

            Rectangle rEnabled = ToDispose(ShapeBase.FromControl<Rectangle>(this, string.Format("{0}_{1}_rectangle", Name, ControlStatus.Enabled)));
            ShapeMap.Add(ControlStatus.Enabled, new[] { rEnabled });
        }
    }
}
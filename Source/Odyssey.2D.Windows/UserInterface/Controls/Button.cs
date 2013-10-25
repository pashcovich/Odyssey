using Odyssey.Graphics.Rendering2D.Shapes;
using Odyssey.Style;
using SharpDX;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rectangle = Odyssey.Graphics.Rendering2D.Shapes.Rectangle;

namespace Odyssey.UserInterface.Controls
{
    public class Button : ButtonBase
    {
        public Button()
        {
            Label = ToDispose(new Label() { TextDescriptionClass = this.TextDescriptionClass });
        }

        public override void Initialize(Engine.IDirectXProvider directX)
        {
            base.Initialize(directX);
            if (string.Equals(ControlDescriptionClass, "Empty"))
                return;

            Rectangle rEnabled = ToDispose(new Rectangle()
            {
                Id = string.Format("{0}_{1}_rectangle", Id, ControlStatus.Enabled),
                BoundingRectangle = BoundingRectangle
            });
            Rectangle rHighlighted= ToDispose(new Rectangle()
            {   
                Id = string.Format("{0}_{1}_rectangle", Id, ControlStatus.Highlighted),
                BoundingRectangle = BoundingRectangle 
            });

            ShapeMap.BeginDesign();
            ShapeMap.Add(ControlStatus.Enabled, new[] { rEnabled });
            
            if (Description.Highlighted != null)
                ShapeMap.Add(ControlStatus.Highlighted, new[] { rHighlighted});
            ShapeMap.EndDesign(directX);

            ActiveStyle = ShapeMap.GetShapes(ControlStatus.Enabled).ToArray();
        }

        public override bool Contains(SharpDX.Vector2 cursorLocation)
        {
            return BoundingRectangle.Contains(cursorLocation);
        }

        public override void Render(Engine.IDirectXTarget target)
        {
            
            foreach (IShape shape in ActiveStyle)
                shape.Render(target);
            base.Render(target);
        }
    }
}
using Odyssey.Engine;
using Odyssey.UserInterface.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct2D1;
using Odyssey.UserInterface.Style;
using Odyssey.UserInterface;
using SharpDX;
using Odyssey.Graphics.Rendering2D.Shapes;
using Rectangle = Odyssey.Graphics.Rendering2D.Shapes.Rectangle;
using Odyssey.Graphics.Rendering2D;


namespace Odyssey.UserInterface.Controls
{
    public delegate void DrawMethod(IDirectXTarget target);

    public class RenderableControl : Control, IRenderable
    {
        public Color4 Background { get; set; }
        public Color4 BorderColor { get; set; }

        protected Dictionary<ControlStatus, Shape[]> Styles { get; private set; }
        protected Shape[] ActiveStyle { get; set; }

        public RenderableControl(string id, string descriptionClass) : base(id, descriptionClass)
        {
            Styles = new Dictionary<ControlStatus, Shape[]>();
        }

        public virtual void Initialize(IDirectXProvider directX)
        {
            
            if (string.Equals(ControlDescriptionClass, "Empty"))
                return;
            
            Styles.Add(ControlStatus.Enabled, new [] { ToDispose(new Rectangle(string.Format("{0}_{1}_rectangle", Id, ControlStatus.Enabled)))});
            Styles[ControlStatus.Enabled][0].UpdateExtents((RectangleF)this);
            Styles[ControlStatus.Enabled][0].Fill = Description.Enabled;
            Styles[ControlStatus.Enabled][0].Stroke = Description.Border;
            Styles[ControlStatus.Enabled][0].Initialize(directX);

            if (Description.Highlighted != null)
            {
                Styles.Add(ControlStatus.Highlighted, new[] { ToDispose(new Rectangle(string.Format("{0}_{1}_rectangle", Id, ControlStatus.Highlighted))) });
                Styles[ControlStatus.Highlighted][0].UpdateExtents((RectangleF)this);
                Styles[ControlStatus.Highlighted][0].Fill = Description.Highlighted;
                Styles[ControlStatus.Highlighted][0].Stroke = Description.Border;
                Styles[ControlStatus.Highlighted][0].Initialize(directX);
            }

            ActiveStyle = Styles[ControlStatus.Enabled];
        }


        public virtual void Render(IDirectXTarget target)
        {
            DeviceContext context = target.Direct2D.Context;
            ActiveStyle[0].Render(target);
        }

        public override bool IntersectTest(Vector2 cursorLocation)
        {
            return ActiveStyle[0].IntersectTest(cursorLocation);
        }

    }
}

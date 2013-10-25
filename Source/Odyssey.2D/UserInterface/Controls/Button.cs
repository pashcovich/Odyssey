using Odyssey.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.UserInterface.Controls
{
    public class Button : RenderableControl
    {
        private const string ControlTag = "Button";
        private static int count;

        private Label label;

        public string Text
        {
            get { return label.Text; }
            set
            {
                if (!string.Equals(label.Text, value))
                    label.Text = value;
            }
        }

        public Button() : base(ControlTag + ++count, ControlTag)
        {
            label = ToDispose(new Label()
            {
                TextDescriptionClass = ControlTag
            });
        }

        public override void Initialize(IDirectXProvider directX)
        {
            base.Initialize(directX);
            label.Parent = this;
            label.Initialize(directX);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            label.Arrange();
        }

        protected override void OnPositionChanged(EventArgs e)
        {
            base.OnPositionChanged(e);
            label.Arrange();
        }

        protected override void OnPointerEnter(Devices.PointerEventArgs e)
        {
            base.OnPointerEnter(e);
            ActiveStyle = Styles[ControlStatus.Highlighted];
        }

        protected override void OnPointerExited(Devices.PointerEventArgs e)
        {
            base.OnPointerExited(e);
            ActiveStyle = Styles[ControlStatus.Enabled];
        }

        public override void Render(Engine.IDirectXTarget target)
        {
            base.Render(target);
            label.Render(target);
        }
    }
}

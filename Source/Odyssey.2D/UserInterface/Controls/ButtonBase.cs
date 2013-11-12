using Odyssey.Devices;
using Odyssey.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.UserInterface.Controls
{
    public abstract class ButtonBase : Control
    {
        private const string ControlTag = "Button";
        private static int count;

        protected ButtonBase()
            : base(ControlTag)
        {
        }

        public string Text
        {
            get
            {
                return Label.Text;
            }
            set
            {
                if (!string.Equals(Label.Text, value))
                    Label.Text = value;
            }
        }
        protected LabelBase Label { get; set; }

        public override void Initialize(IDirectXProvider directX)
        {
            Label.Parent = this;
            Label.Initialize(directX);
        }

        public override void Render(IDirectXTarget target)
        {
            Label.Render(target);
        }

        protected override void OnPointerEnter(PointerEventArgs e)
        {
            base.OnPointerEnter(e);
            ActiveStyle = ShapeMap.GetShapes(ControlStatus.Highlighted).ToArray();
        }

        protected override void OnPointerExited(PointerEventArgs e)
        {
            base.OnPointerExited(e);
            ActiveStyle = ShapeMap.GetShapes(ControlStatus.Enabled).ToArray();
        }

        protected override void OnPositionChanged(EventArgs e)
        {
            base.OnPositionChanged(e);
            Label.Arrange();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            Label.Arrange();
        }
    }
}
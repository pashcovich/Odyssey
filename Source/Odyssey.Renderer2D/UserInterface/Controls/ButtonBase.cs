#region Using Directives

using Odyssey.Interaction;
using System;
using System.Linq;

#endregion Using Directives

namespace Odyssey.UserInterface.Controls
{
    public abstract class ButtonBase : Control
    {
        protected const string ControlTag = "Button";

        protected ButtonBase()
            : this(ControlTag)
        {
        }

        protected ButtonBase(string controlDescriptionClass, string textDescriptionClass = ControlTag)
            : base(controlDescriptionClass, textDescriptionClass)
        {
        }

        public override UIElement Parent
        {
            get { return base.Parent; }
            internal set
            {
                base.Parent = value;
                Label.Parent = value;
            }
        }

        public string Text
        {
            get { return Label.Text; }
            set
            {
                if (!string.Equals(Label.Text, value))
                    Label.Text = value;
            }
        }

        protected LabelBase Label { get; set; }

        public override void Render()
        {
            Label.Render();
        }

        protected internal override void OnPointerExited(PointerEventArgs e)
        {
            base.OnPointerExited(e);
            ActiveStyle = ShapeMap.GetShapes(ControlStatus.Enabled).ToArray();
        }

        protected override void OnInitializing(ControlEventArgs e)
        {
            Label.Parent = this;

            base.OnInitializing(e);
            Label.Initialize();
        }

        protected override void OnLayoutUpdated(EventArgs e)
        {
            base.OnLayoutUpdated(e);
            Label.Width = Width;
            Label.Height = Height;
            Label.Layout();
        }

        protected override void OnPointerEnter(PointerEventArgs e)
        {
            base.OnPointerEnter(e);
            ActiveStyle = ShapeMap.GetShapes(ControlStatus.Highlighted).ToArray();
        }

        protected override void OnTextDefinitionChanged(EventArgs e)
        {
            base.OnTextDefinitionChanged(e);
            if (!string.Equals(Label.TextDescriptionClass, TextDescriptionClass))
                Label.TextDescriptionClass = TextDescriptionClass;
        }
    }
}
#region Using Directives

using Odyssey.Interaction;
using System;
using System.Linq;

#endregion Using Directives

namespace Odyssey.UserInterface.Controls
{
    public abstract class ButtonBase : ContentControl
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

        protected override void OnInitializing(ControlEventArgs e)
        {
            base.OnInitializing(e);
            if (Content == null)
            {
                Content = new Label() {Text = Name};
                Content.Initialize();
            }
        }

        protected internal override void OnPointerExited(PointerEventArgs e)
        {
            base.OnPointerExited(e);
            ActiveStyle = ShapeMap.GetShapes(ControlStatus.Enabled).ToArray();
        }

        
        protected override void OnPointerEnter(PointerEventArgs e)
        {
            base.OnPointerEnter(e);
            ActiveStyle = ShapeMap.GetShapes(ControlStatus.Highlighted).ToArray();
        }

        
    }
}
#region Using Directives

using System;
using Odyssey.Interaction;
using Odyssey.UserInterface.Data;
using Odyssey.UserInterface.Style;
using SharpDX;

#endregion

namespace Odyssey.UserInterface.Controls
{
    public abstract class ButtonBase : ContentControl
    {
        protected const string ControlTag = "Button";

        protected ButtonBase()
            : this(ControlTag)
        {
        }

        protected ButtonBase(string controlStyleClass, string textStyleClass = TextStyle.Default)
            : base(controlStyleClass, textStyleClass)
        {
        }

        protected override void OnPointerExited(PointerEventArgs e)
        {
            base.OnPointerExited(e);
            ActiveStatus = ControlStatus.Enabled;
        }

        protected override void OnInitializing(EventArgs e)
        {
            base.OnInitializing(e);
            if (Content == null && Template == null)
            {
                Content = new TextBlock { Text = Name, TextStyleClass = TextStyleClass };
                Content.Initialize();
            }
        }

        protected override Vector3 MeasureOverride(Vector3 availableSizeWithoutMargins)
        {
            if (Content != null)
                Content.Measure(availableSizeWithoutMargins);
            return base.MeasureOverride(availableSizeWithoutMargins);
        }

        protected override Vector3 ArrangeOverride(Vector3 availableSizeWithoutMargins)
        {
            if (Content != null)
                Content.Arrange(availableSizeWithoutMargins);
            return base.ArrangeOverride(availableSizeWithoutMargins);
        }

        protected override void OnPointerEnter(PointerEventArgs e)
        {
            base.OnPointerEnter(e);
            ActiveStatus = ControlStatus.Highlighted;
        }
    }
}
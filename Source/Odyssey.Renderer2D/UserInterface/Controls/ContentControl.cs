#region Using Directives

using System;
using Odyssey.UserInterface.Style;

#endregion

namespace Odyssey.UserInterface.Controls
{
    public abstract class ContentControl : Control
    {
        private UIElement content;

        protected ContentControl(string controlStyleClass, string textStyleClass = UserInterface.Style.TextStyle.Default)
            : base(controlStyleClass, textStyleClass)
        {
        }

        public UIElement Content
        {
            get { return content; }
            set
            {
                if (content == value)
                    return;
                Children.Remove(content);
                SetParent(value, this);
                content = ToDispose(value);
            }
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            if (Content!=null)
                Content.BringToFront();
        }

        protected internal override UIElement Copy()
        {
            var contentControl = (ContentControl) base.Copy();
            contentControl.Content = Content;
            return contentControl;
        }
    }
}
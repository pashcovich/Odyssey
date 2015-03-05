#region Using Directives

using System;

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
                content = ToDispose(value);
                Children.Add(content);
            }
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            if (Content!=null)
                Content.BringToFront();
        }
    }
}
using System;
using Odyssey.UserInterface.Events;
using SharpDX.Mathematics;

namespace Odyssey.UserInterface.Controls
{
    public abstract class ContentControl : Control, IContentControl
    {
        private UIElement content;

        protected ContentControl(string controlStyleClass, string textStyleClass)
            : base(controlStyleClass, textStyleClass)
        {
        }

        protected ContentControl(string controlStyleClass) : base(controlStyleClass) { }

        public UIElement Content
        {
            get { return content; }
            set
            {
                if (content == value)
                    return;

                Controls.Remove(content);
                content = ToDispose(value);
                Controls.Add(content);
            }
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            Content.BringToFront();
        }

    }
}
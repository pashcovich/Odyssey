﻿using System;
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
                content = ToDispose(value);
                content.Parent = this;
            }
        }

        public override bool DesignMode
        {
            get
            {
                return base.DesignMode;
            }

            protected internal set
            {
                base.DesignMode = value;
                if (Content != null)
                    Content.DesignMode = value;
            }
        }

        public override void Render()
        {
            base.Render();
            if (Content != null)
                Content.Render();
        }

        protected override void OnInitializing(EventArgs e)
        {
            base.OnInitializing(e);
            if (Content != null)
                Content.Initialize();
        }

        protected override Vector3 ArrangeOverride(Vector3 availableSizeWithoutMargins)
        {
            Content.Arrange(availableSizeWithoutMargins);
            return base.ArrangeOverride(availableSizeWithoutMargins);
        }

        protected override Vector3 MeasureOverride(Vector3 availableSizeWithoutMargins)
        {
            Content.Measure(availableSizeWithoutMargins);
            return base.MeasureOverride(availableSizeWithoutMargins);
        }

        protected override void OnSizeChanged(SizeChangedEventArgs e)
        {
            base.OnSizeChanged(e);
            if (Content != null)
            {
                Content.Width = Width;
                Content.Height = Height;
            }
        }
    }
}
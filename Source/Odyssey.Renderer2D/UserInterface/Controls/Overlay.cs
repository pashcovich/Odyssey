#region License

// Copyright © 2013-2014 Avengers UTD - Adalberto L. Simeone
//
// The Odyssey Engine is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License Version 3 as published by
// the Free Software Foundation.
//
// The Odyssey Engine is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details at http://gplv3.fsf.org/

#endregion License

#region Using Directives

using Odyssey.Engine;
using Odyssey.Graphics;
using Odyssey.Interaction;
using Odyssey.UserInterface.Style;
using SharpDX;
using System;
using System.Diagnostics.Contracts;
using System.Linq;
using SharpDX.Direct2D1;

#endregion Using Directives

namespace Odyssey.UserInterface.Controls
{
    public sealed class Overlay : ContainerControl, IOverlay
    {
        private const string ControlTag = "Overlay";
        private const string DefaultControlTheme = "DefaultTheme";
        private const string DefaultTextTheme = "DefaultText";
        private readonly Direct2DDevice device;
        private readonly IServiceRegistry services;
        private readonly IStyleService styleService;
        private UIElement captureElement;

        #region Properties

        public string ControlTheme { get; private set; }

        public IStyleService StyleService
        {
            get { return styleService; }
        }

        public string TextTheme { get; private set; }

        public new Direct2DDevice Device { get { return device; } }

        internal UIElement CaptureElement
        {
            get { return captureElement; }
            set
            {
                if (value != null)
                    value.HasCaptured = true;
                else if (captureElement != null)
                {
                    captureElement.HasCaptured = false;
                    captureElement.OnPointerCaptureChanged(UserInterfaceManager.LastPointerEvent);
                }
                captureElement = value;
            }
        }

        internal UIElement EnteredElement { get; set; }
        internal UIElement FocusedElement { get; set; }

        public IServiceRegistry Services { get { return services; } }

        #endregion Properties

        public Overlay(IServiceRegistry services)
            : base("Empty")
        {
            Contract.Requires<ArgumentNullException>(services != null, "services");

            this.services = services;
            device = services.GetService<IDirect2DService>().Direct2DDevice;

            IsInside = true;
            IsFocusable = true;

            TextTheme = DefaultTextTheme;
            ControlTheme = DefaultControlTheme;

            styleService = services.GetService<IStyleService>();
            EnteredElement = this;
            FocusedElement = this;
            Overlay = this;
        }

        public void BeginDesign()
        {
            DesignMode = true;
        }

        public override bool Contains(Vector2 cursorLocation)
        {
            return true;
        }

        public void Display(bool clear = false)
        {
            Device.Target = Device.BackBuffer;

            DeviceContext context = Device;

            context.BeginDraw();
            if (clear)
                context.Clear(Color.Transparent);
            Render();
            context.EndDraw();
        }

        public void EndDesign()
        {
            DesignMode = false;
        }

        public override void Render()
        {
            foreach (var control in Controls.Where(control => control.IsVisible))
                control.Render();
        }

        protected override void Dispose(bool disposeManagedResources)
        {
            base.Dispose(disposeManagedResources);
            if (disposeManagedResources)
            {
                foreach (UIElement uiElement in Controls)
                    uiElement.Dispose();
            }
            IsInited = false;
        }

        protected override void Arrange()
        {
            return;
        }

        protected override void OnInitialized(ControlEventArgs e)
        {
            base.OnInitialized(e);
            Layout();
            IsInited = true;
        }

        void IOverlay.ProcessPointerMovement(PointerEventArgs e)
        {
            //Proceeds with the rest
            foreach (UIElement control in TreeTraversal.PostOrderInteractionVisit(this))
            {
                e.Handled = control.ProcessPointerMovement(e);
                if (e.Handled)
                {
                    break;
                }
            }

            if (e.Handled) return;

            ProcessPointerMovement(e);
            e.Handled = true;
        }

        void IOverlay.ProcessPointerPress(PointerEventArgs e)
        {
            //Proceeds with the rest
            foreach (UIElement control in TreeTraversal.PostOrderInteractionVisit(this))
            {
                e.Handled = control.ProcessPointerPressed(e);
                if (e.Handled)
                    break;
            }

            if (e.Handled)
                return;

            ProcessPointerPressed(e);
            e.Handled = true;
        }

        void IOverlay.ProcessPointerRelease(PointerEventArgs e)
        {
            //Proceeds with the rest
            foreach (UIElement control in TreeTraversal.PostOrderInteractionVisit(this))
            {
                e.Handled = control.ProcessPointerRelease(e);
                if (e.Handled)
                    break;
            }

            if (e.Handled)
                return;

            ProcessPointerRelease(e);
            e.Handled = true;
        }
    }
}
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

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Odyssey.Content;
using Odyssey.Core;
using Odyssey.Engine;
using Odyssey.Interaction;
using Odyssey.UserInterface.Data;
using Odyssey.UserInterface.Style;
using SharpDX.Direct2D1;
using SharpDX.Mathematics;

#endregion

namespace Odyssey.UserInterface.Controls
{
    public sealed class Overlay : ContentControl, IOverlay, IDesktopOverlay
    {
        private const string DefaultControlTheme = "DefaultTheme";
        private const string DefaultTextTheme = "DefaultText";
        private readonly Direct2DDevice device;
        private readonly IServiceRegistry services;
        private readonly IStyleService styleService;
        private readonly Theme theme;
        private UIElement captureElement;

        #region Properties

        public Theme Theme
        {
            get { return theme; }
        }

        public string TextTheme { get; private set; }

        public new Direct2DDevice Device
        {
            get { return device; }
        }

        internal UIElement CaptureElement
        {
            get { return captureElement; }
            set
            {
                if (value != null)
                    value.IsPointerCaptured = true;
                else if (captureElement != null)
                {
                    captureElement.IsPointerCaptured = false;
                    captureElement.OnPointerCaptureChanged(UserInterfaceManager.LastPointerEvent);
                }
                captureElement = value;
            }
        }

        internal UIElement EnteredElement { get; set; }
        internal UIElement FocusedElement { get; set; }

        public IServiceRegistry Services
        {
            get { return services; }
        }

        #endregion Properties

        public Overlay(IServiceRegistry services)
            : base(VisualStyle.Empty)
        {
            Contract.Requires<ArgumentNullException>(services != null, "services");

            this.services = services;
            device = services.GetService<IDirect2DService>().Direct2DDevice;

            IsInside = true;
            IsFocusable = true;

            TextTheme = DefaultTextTheme;
            styleService = ToDispose(services.GetService<IStyleService>());
            theme = styleService.GetTheme(DefaultControlTheme);

            EnteredElement = this;
            FocusedElement = this;
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
            Initialize();
            DesignMode = false;
        }

        public override void Render()
        {
            foreach (var control in TreeTraversal.VisibleControls(Content))
                control.Render();
        }

        protected override void Dispose(bool disposeManagedResources)
        {
            base.Dispose(disposeManagedResources);
            if (disposeManagedResources)
            {
                foreach (UIElement uiElement in TreeTraversal.PreOrderVisit(Content))
                    uiElement.Dispose();
            }
            IsInited = false;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            Layout(new Vector3(float.PositiveInfinity));
            IsInited = true;
        }

        protected override Vector3 MeasureOverride(Vector3 availableSizeWithoutMargins)
        {
            var settings = Services.GetService<IDirectXDeviceSettings>();
            var screenSize = new Vector3(settings.PreferredBackBufferWidth, settings.PreferredBackBufferHeight, 0);
            Content.Measure(screenSize);
            return screenSize;
        }

        public override void Update(ITimeService time)
        {
            var controlArray = TreeTraversal.PreOrderVisit(Content).ToArray();
            for (int i = 1; i < controlArray.Length; i++)
            {
                var control = controlArray[i];
                control.Update(time);
            }
        }

        #region IResourceProvider

        protected override IEnumerable<IResource> Resources
        {
            get { return TreeTraversal.PreOrderVisit(this); }
        }

        protected override bool ContainsResource(string resourceName)
        {
            return TreeTraversal.PreOrderVisit(Content).Any(c => string.Equals(c.Name, resourceName));
        }

        protected override TResource GetResource<TResource>(string resourceName)
        {
            return TreeTraversal.PreOrderVisit(Content).First(c => string.Equals(c.Name, resourceName)) as TResource;
        }

        #endregion

        #region Input Handling Methods

        void IDesktopOverlay.ProcessKeyDown(KeyEventArgs e)
        {
            // If an element as captured the pointer, send the event to it first
            if (CaptureElement != null)
                e.Handled = CaptureElement.ProcessKeyDown(e);

            if (e.Handled)
                return;

            // Then, send it to the currently focused control
            if (FocusedElement != null)
                e.Handled = FocusedElement.ProcessKeyDown(e);

            if (e.Handled)
                return;

            //Proceeds with the rest
            foreach (UIElement control in TreeTraversal.PostOrderInteractionVisit(this))
            {
                e.Handled = control.ProcessKeyDown(e);
                if (e.Handled)
                    break;
            }

            if (e.Handled)
                return;

            e.Handled = ProcessKeyDown(e);
        }

        void IDesktopOverlay.ProcessKeyUp(KeyEventArgs e)
        {
            // If an element as captured the pointer, send the event to it first
            if (CaptureElement != null)
                e.Handled = CaptureElement.ProcessKeyUp(e);

            if (e.Handled)
                return;

            // Then, send it to the currently focused control
            if (FocusedElement != null)
                e.Handled = FocusedElement.ProcessKeyUp(e);

            e.Handled = ProcessKeyUp(e);
        }

        void IOverlay.ProcessPointerMovement(PointerEventArgs e)
        {
            // If an element as captured the pointer, send the event to it first
            if (CaptureElement != null)
                e.Handled = CaptureElement.ProcessPointerMovement(e);

            if (e.Handled)
                return;

            //Proceeds with the rest
            foreach (UIElement control in TreeTraversal.PostOrderInteractionVisit(this))
            {
                e.Handled = control.ProcessPointerMovement(e);
                if (e.Handled)
                    break;
            }

            if (e.Handled) return;

            ProcessPointerMovement(e);
            e.Handled = true;
        }

        void IOverlay.ProcessPointerPress(PointerEventArgs e)
        {
            // If an element as captured the pointer, send the event to it first
            if (CaptureElement != null)
                e.Handled = CaptureElement.ProcessPointerPressed(e);

            if (e.Handled)
                return;
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
            // If an element as captured the pointer, send the event to it first
            if (CaptureElement != null)
                e.Handled = CaptureElement.ProcessPointerRelease(e);

            if (e.Handled)
                return;

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

        #endregion
    }
}
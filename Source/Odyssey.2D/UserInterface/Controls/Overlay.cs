
using Odyssey.Devices;
using Odyssey.Engine;
using Odyssey.Graphics;
using Odyssey.Graphics.Rendering2D;
using Odyssey.UserInterface.Style;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Odyssey.UserInterface.Controls
{
    public class Overlay : ContainerControl, IRenderable
    {
        private const string ControlTag = "Overlay";
        private readonly SortedDictionary<Key, KeyBinding> keyBindings;
        //private readonly List<RenderStep> renderInfoList;
        private UIElement captureControl;

        #region Properties
        public OverlayDescription OverlayDescription { get; protected set; }


        internal UIElement CaptureControl
        {
            get { return captureControl; }
            set
            {
                if (value != null)
                    value.HasCaptured = true;
                else if (captureControl != null)
                {
                    captureControl.HasCaptured = false;
                    captureControl.OnPointerCaptureChanged(OdysseyUI.LastPointerEvent);
                }
                captureControl = value;
            }
        }

        internal UIElement FocusedControl { get; set; }
        internal UIElement EnteredControl { get; set; }
        internal UIElement PressedControl { get; set; }

        #endregion

        public static Overlay FromDescription(OverlayDescription description)
        {
            Overlay overlay = new Overlay
                          {
                              AbsolutePosition = Vector2.Zero,
                              Size = new Size(description.Width, description.Height),
                              OverlayDescription = description,
                              Depth = new Depth
                                          {
                                              WindowLayer = Depth.Background,
                                              ComponentLayer = Depth.Background,
                                              ZOrder = Depth.Background
                                          },
                          };

            return overlay;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Overlay"/> class.
        /// </summary>
        protected Overlay()
            : base(ControlTag, "Empty")
        {
            keyBindings = new SortedDictionary<Key, KeyBinding>();
            IsInside = true;
            IsFocusable = true;
            FocusedControl = EnteredControl = this;
        }

        public void SetBindings(params KeyBinding[] bindings)
        {
            foreach (KeyBinding binding in bindings)
            {
                if (!keyBindings.ContainsKey(binding.Key))
                    keyBindings.Add(binding.Key, binding);
            }
        }

        public override bool Contains(Vector2 cursorLocation)
        {
            return true;
        }


        public override void Initialize(IDirectXProvider directX)
        {
            foreach (IRenderable renderable in Controls)
                renderable.Initialize(directX);
        }

        public override void Render(IDirectXTarget target)
        {
            foreach (IControl control in Controls)
                if (control.IsVisible)
                    control.Render(target);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            KeyBinding binding = null;
            if (keyBindings.ContainsKey(e.KeyCode))
                binding = keyBindings[e.KeyCode];

            if (binding != null)
                binding.Apply(true);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            KeyBinding binding = null;
            if (keyBindings.ContainsKey(e.KeyCode))
                binding = keyBindings[e.KeyCode];

            if (binding != null)
                binding.Apply(false);
        }

        public void ProcessKeyEvents()
        {
            foreach (KeyValuePair<Key, KeyBinding> kvp in keyBindings)
            {
                KeyBinding kb = kvp.Value;

                if (kb.State) kb.Operation();
            }
        }

        public void BeginDesign()
        {
            OdysseyUI.CurrentOverlay = this;
            DesignMode = true;
        }

        public virtual void EndDesign(IDirectXProvider directX)
        {
            foreach (UIElement uiElement in Controls)
                uiElement.Arrange();

            Initialize(directX);

        }




    }
}

#region Using Directives

using Odyssey.Engine;
using Odyssey.UserInterface.Style;
using SharpDX;
using System;
using System.Diagnostics.Contracts;
using System.Linq;

#endregion Using Directives

namespace Odyssey.UserInterface.Controls
{
    public abstract class OverlayBase : ContainerControl
    {
        private const string ControlTag = "Overlay";
        private const string DefaultControlTheme = "DefaultTheme";
        private const string DefaultTextTheme = "DefaultText";
        private readonly Direct2DDevice device;
        private readonly IServiceRegistry services;
        private readonly IUserInterfaceState state;
        private readonly IStyleService styleService;
        private UIElement captureControl;

        #region Properties

        public string ControlTheme { get; set; }

        public OverlayDescription OverlayDescription { get; protected set; }

        public IUserInterfaceState State
        {
            get { return state; }
        }

        public IStyleService StyleService
        {
            get { return styleService; }
        }

        public string TextTheme { get; set; }

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
                    captureControl.OnPointerCaptureChanged(UserInterfaceManager.LastPointerEvent);
                }
                captureControl = value;
            }
        }

        internal Direct2DDevice Device { get { return device; } }

        protected IServiceRegistry Services
        {
            get { return services; }
        }

        #endregion Properties

        protected OverlayBase(IServiceRegistry services)
            : base("Empty")
        {
            Contract.Requires<ArgumentNullException>(services != null, "services");

            this.services = services;
            device = Services.GetService<IDirect2DService>().Direct2DDevice;

            IsInside = true;
            IsFocusable = true;

            TextTheme = DefaultTextTheme;
            ControlTheme = DefaultControlTheme;

            styleService = Services.GetService<IStyleService>();
            state = Services.GetService<IUserInterfaceState>();
            state.Entered = this;
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

        public abstract void Display(bool clear = false);

        public virtual void EndDesign()
        {
            DesignMode = false;
        }

        public new virtual void Initialize()
        {
            base.Initialize();
        }

        public override void Render()
        {
            foreach (var control in Controls.Where(control => control.IsVisible))
                control.Render();
        }

        public override void Unload()
        {
            foreach (UIElement uiElement in Controls)
                uiElement.Unload();
            IsInited = false;
        }

        protected override void Arrange()
        {
            return;
        }

        protected override void OnInitialized(ControlEventArgs e)
        {
            base.OnInitialized(e);
            IsInited = true;
        }
    }
}
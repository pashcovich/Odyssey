#region Using Directives

using Odyssey.Graphics;
using SharpDX.Windows;
using System;
using System.Drawing;
using System.Windows.Forms;

#endregion Using Directives

namespace Odyssey.Engine
{
    /// <summary>
    /// The implementation of <see cref="ApplicationWindow"/> for Desktop/WinForms platform.
    /// </summary>
    internal class DesktopApplicationWindow : ApplicationWindow
    {
        private RenderForm applicationForm;
        private bool isMouseCurrentlyHidden;
        private bool isMouseVisible;

        protected RenderLoop RenderLoop { get; set; }

        protected RenderForm ApplicationForm
        {
            get { return applicationForm; }
        }

        protected DesktopApplicationContext DesktopContext { get; set; }

        /// <summary>
        /// The render control associated with the current <see cref="ApplicationWindow"/>.
        /// </summary>
        public Control Control { get; set; }

        /// <summary>
        /// Gets the native window object associated with the current instance of <see cref="DesktopApplicationWindow"/>.
        /// </summary>
        public override object NativeWindow
        {
            get { return Control; }
        }

        /// <inheritdoc />
        public override bool IsMouseVisible
        {
            get { return isMouseVisible; }
            set
            {
                if (isMouseVisible != value)
                {
                    isMouseVisible = value;
                    if (isMouseVisible)
                    {
                        if (isMouseCurrentlyHidden)
                        {
                            Cursor.Show();
                            isMouseCurrentlyHidden = false;
                        }
                    }
                    else if (!isMouseCurrentlyHidden)
                    {
                        Cursor.Hide();
                        isMouseCurrentlyHidden = true;
                    }
                }
            }
        }

        /// <inheritdoc />
        public override bool Visible
        {
            get { return Control.Visible; }
            set { Control.Visible = value; }
        }

        /// <inheritdoc />
        public override bool AllowUserResizing
        {
            get { return (Control is RenderForm && ((RenderForm)Control).AllowUserResizing); }
            set
            {
                var form = Control as RenderForm;
                if (form != null)
                {
                    form.AllowUserResizing = value;
                }
            }
        }

        /// <inheritdoc />
        public override SharpDX.Rectangle ClientBounds
        {
            get { return new SharpDX.Rectangle(0, 0, Control.ClientSize.Width, Control.ClientSize.Height); }
        }

        /// <inheritdoc />
        public override DisplayOrientation CurrentOrientation
        {
            get { return DisplayOrientation.Default; }
        }

        /// <inheritdoc />
        public override bool IsMinimized
        {
            get
            {
                var form = Control as Form;
                if (form != null)
                {
                    return form.WindowState == FormWindowState.Minimized;
                }

                // Check for non-form control
                return false;
            }
        }

        /// <summary>
        /// For Desktop/WinForms platform the <see cref="ApplicationWindow.Run"/> call is blocking due to <see cref="SharpDX.Windows.RenderLoop"/> implementation.
        /// </summary>
        /// <returns>true</returns>
        internal override bool IsBlockingRun
        {
            get { return true; }
        }

        /// <inheritdoc />
        public override void BeginScreenDeviceChange(bool willBeFullScreen)
        {
        }

        /// <inheritdoc />
        public override void EndScreenDeviceChange(int clientWidth, int clientHeight)
        {
            if (applicationForm != null)
                applicationForm.ClientSize = new Size(clientWidth, clientHeight);
        }

        /// <inheritdoc />
        internal override bool CanHandle(ApplicationContext applicationContext)
        {
            return applicationContext.ContextType == ApplicationContextType.Desktop ||
                   applicationContext.ContextType == ApplicationContextType.DesktopHwndWpf;
        }

        /// <inheritdoc />
        internal override void Initialize(ApplicationContext applicationContext)
        {
            ApplicationContext = applicationContext;

            if (applicationContext.ContextType == ApplicationContextType.Desktop)
            {
                DesktopContext = (DesktopApplicationContext)ApplicationContext;
                Control = (Control)applicationContext.Control;
                InitializeFromWinForm();
            }
        }

        /// <inheritdoc />
        protected void InitializeFromWinForm()
        {
            // Setup the initial size of the window
            var width = ApplicationContext.RequestedWidth;
            if (width == 0)
            {
                width = Control is Form ? Global.DefaultBackBufferWidth : Control.ClientSize.Width;
            }

            var height = ApplicationContext.RequestedHeight;
            if (height == 0)
            {
                height = Control is Form ? Global.DefaultBackBufferHeight : Control.ClientSize.Height;
            }

            Control.ClientSize = new Size(width, height);

            Control.MouseEnter += HandleControlMouseEnter;
            Control.MouseLeave += HandleControlMouseLeave;

            applicationForm = Control as RenderForm;
            if (applicationForm != null)
            {
                applicationForm.AppActivated += OnActivated;
                applicationForm.AppDeactivated += OnDeactivated;
                applicationForm.UserResized += OnClientSizeChanged;
            }
            else
            {
                Control.Resize += OnClientSizeChanged;
            }
        }

        /// <inheritdoc />
        internal override void Run()
        {
            OnInitialization(this, EventArgs.Empty);

            try
            {
                // Use custom render loop
                Control.Show();
                using (RenderLoop = new RenderLoop(Control) { UseApplicationDoEvents = DesktopContext.UseApplicationDoEvents })
                {
                    while (RenderLoop.NextFrame())
                    {
                        if (Exiting)
                        {
                            if (Control != null)
                            {
                                Control.Dispose();
                                Control = null;
                            }
                            break;
                        }

                        OnTick(this, EventArgs.Empty);
                    }
                }
            }
            finally
            {
                OnShutdown(this, EventArgs.Empty);
            }
        }

        /// <inheritdoc />
        internal override void Resize(int width, int height)
        {
            Control.ClientSize = new Size(width, height);
        }

        /// <inheritdoc />
        internal override void Switch(ApplicationContext context)
        {
            // unbind event handlers from previous control
            Control.MouseEnter -= HandleControlMouseEnter;
            Control.MouseLeave -= HandleControlMouseLeave;

            applicationForm = Control as RenderForm;
            if (applicationForm != null)
            {
                applicationForm.AppActivated -= OnActivated;
                applicationForm.AppDeactivated -= OnDeactivated;
                applicationForm.UserResized -= OnClientSizeChanged;
            }
            else
            {
                Control.Resize -= OnClientSizeChanged;
            }

            // setup and bind event handlers to new control
            Initialize(context);

            Control.Show(); // Make sure the control is visible
            RenderLoop.Control = Control;
        }

        /// <inheritdoc />
        protected internal override void SetSupportedOrientations(DisplayOrientation orientations)
        {
            // Desktop doesn't have orientation (unless on Windows 8?)
        }

        /// <inheritdoc />
        protected override void SetTitle(string title)
        {
            var form = Control as Form;
            if (form != null)
            {
                form.Text = title;
            }
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposeManagedResources)
        {
            if (disposeManagedResources)
            {
                if (Control != null)
                {
                    if (Control.InvokeRequired)
                    {
                        Action disposeAction = delegate
                        {
                            if (!Control.IsDisposed)
                                Control.Dispose();
                        };
                        try
                        {
                            Control.Invoke(disposeAction);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    else
                        Control.Dispose();

                    Control = null;
                }

                applicationForm = null;
            }

            base.Dispose(disposeManagedResources);
        }

        /// <summary>
        /// Handles the <see cref="Control.MouseEnter"/> event to set cursor visibility, depending on <see cref="ApplicationWindow.IsMouseVisible"/> value.
        /// </summary>
        /// <param name="sender">Ignored.</param>
        /// <param name="e">Ignored.</param>
        private void HandleControlMouseEnter(object sender, EventArgs e)
        {
            if (!isMouseVisible && !isMouseCurrentlyHidden)
            {
                Cursor.Hide();
                isMouseCurrentlyHidden = true;
            }
        }

        /// <summary>
        /// Handles the <see cref="Control.MouseEnter"/> event to restore cursor visibility, depending on <see cref="ApplicationWindow.IsMouseVisible"/> value.
        /// </summary>
        /// <param name="sender">Ignored.</param>
        /// <param name="e">Ignored.</param>
        private void HandleControlMouseLeave(object sender, EventArgs e)
        {
            if (isMouseCurrentlyHidden)
            {
                Cursor.Show();
                isMouseCurrentlyHidden = false;
            }
        }
    }
}
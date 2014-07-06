#region Using Directives

using SharpDX.Windows;
using System;
using System.Threading;
using System.Windows.Controls;

#endregion Using Directives

namespace Odyssey.Engine
{
    internal class DesktopWpfApplicationWindow : DesktopApplicationWindow
    {
        private ManualResetEvent renderingThreadForWpfCanRun;
        private ManualResetEvent renderingThreadForWpfHwndHostReady;
        private IntPtr windowHandle;
        private object wpfBorderControl;

        protected new DesktopWpfApplicationContext DesktopContext { get; set; }

        internal override void Initialize(ApplicationContext applicationContext)
        {
            ApplicationContext = applicationContext;

            if (applicationContext.ContextType == ApplicationContextType.DesktopHwndWpf)
            {
                DesktopContext = (DesktopWpfApplicationContext) applicationContext;
                InitializeFromWpfControl(applicationContext.Control);
            }
        }

        private void InitializeFromWpfControl(object wpfControl)
        {
            wpfBorderControl = (Border) wpfControl;
            renderingThreadForWpfCanRun = new ManualResetEvent(false);
            renderingThreadForWpfHwndHostReady = new ManualResetEvent(false);

            // Start a new rendering thread for the WinForm part
            var thread = new Thread(RunWpfRenderLoop) {IsBackground = true};
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void RunWpfRenderLoop()
        {
            // Allocation of the RenderForm should be done on the same thread
            Control = new RenderForm("Odyssey Application") {TopLevel = false, Visible = false};
            InitializeFromWinForm();

            windowHandle = Control.Handle;

            // Notifies that the control is ready
            renderingThreadForWpfHwndHostReady.Set();

            // Wait for actual run
            renderingThreadForWpfCanRun.WaitOne();

            RunRenderLoop();
        }

        /// <inheritdoc />
        internal override void Run()
        {
            if (wpfBorderControl != null)
            {
                StartWpfRenderLoop();
            }
            else
            {
                RunRenderLoop();
            }
        }

        private void StartWpfRenderLoop()
        {
            // Wait for HwndHost ready
            renderingThreadForWpfHwndHostReady.WaitOne();

            // Create the toolkit HwndHost
            ((Border) wpfBorderControl).Child = new HwndHost(windowHandle);

            // WPF rendering is done through a separate host
            renderingThreadForWpfCanRun.Set();
        }

        private void RunRenderLoop()
        {
            OnInitialization(this, EventArgs.Empty);

            try
            {
                // Use custom render loop
                // Show the control for WinForm, let HwndHost show it for WPF
                if (wpfBorderControl == null)
                    Control.Show();

                using (RenderLoop = new RenderLoop(Control) {UseApplicationDoEvents = DesktopContext.UseApplicationDoEvents})
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
    }
}
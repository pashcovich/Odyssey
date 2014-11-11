using System;
using System.Windows.Forms;
using SharpDX.Windows;

namespace Odyssey.Engine
{
    /// <summary>
    /// A <see cref="DesktopApplicationContext"/> to use for rendering to an existing WinForm <see cref="Control"/>.
    /// </summary>
    public class DesktopApplicationContext : ApplicationContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DesktopApplicationContext" /> class with a default <see cref="RenderForm"/>.
        /// </summary>
        public DesktopApplicationContext()
            : this((Control)null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DesktopApplicationContext" /> class.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="requestedWidth">Width of the requested.</param>
        /// <param name="requestedHeight">Height of the requested.</param>
        public DesktopApplicationContext(Control control = null, int requestedWidth = 0, int requestedHeight = 0)
            : base(ApplicationContextType.Desktop, control ?? CreateDefaultControl())
        {
            RequestedWidth = requestedWidth;
            RequestedHeight = requestedHeight;
            var controlType = Control.GetType();
            if (!SharpDX.Utilities.IsTypeInheritFrom(controlType, "System.Windows.Forms.Control"))
                throw new ArgumentException("Control is not supported. Must inherit from System.Windows.Forms.Control (WinForm).");

            using (var g = (Control as Control).CreateGraphics())
            {
                DpiX = g.DpiX;
                DpiY = g.DpiY;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DesktopApplicationContext" /> class.
        /// </summary>
        /// <param name="windowHandle">The window handle.</param>
        /// <param name="requestedWidth">Width of the requested.</param>
        /// <param name="requestedHeight">Height of the requested.</param>
        public DesktopApplicationContext(IntPtr windowHandle, int requestedWidth = 0, int requestedHeight = 0)
            : base(ApplicationContextType.Desktop, System.Windows.Forms.Control.FromHandle(windowHandle))
        {
            RequestedWidth = requestedWidth;
            RequestedHeight = requestedHeight;
            using (var g = (Control as Control).CreateGraphics())
            {
                DpiX = g.DpiX;
                DpiY = g.DpiY;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the render loop should use the default <see cref="Application.DoEvents"/> instead of a custom window message loop lightweight for GC. Default is false
        /// </summary>
        /// <value><c>true</c> if use the default <see cref="Application.DoEvents"/>; otherwise, <c>false</c>.</value>
        public bool UseApplicationDoEvents { get; set; }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Control"/> to <see cref="DesktopApplicationContext"/>.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator DesktopApplicationContext(Control control)
        {
            return new DesktopApplicationContext(control);
        }

        private static object CreateDefaultControl()
        {
            return new RenderForm("Odyssey Application") { AllowUserResizing = false };
        }
    }
}
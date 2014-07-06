using SharpDX.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Odyssey.Engine
{
    public class DesktopWpfApplicationContext : ApplicationContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DesktopApplicationContext" /> class with a default <see cref="RenderForm"/>.
        /// </summary>
        public DesktopWpfApplicationContext()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DesktopApplicationContext" /> class.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="requestedWidth">Width of the requested.</param>
        /// <param name="requestedHeight">Height of the requested.</param>
        public DesktopWpfApplicationContext(object control = null, int requestedWidth = 0, int requestedHeight = 0)
            : base(ApplicationContextType.DesktopHwndWpf, control ?? CreateDefaultControl())
        {
            RequestedWidth = requestedWidth;
            RequestedHeight = requestedHeight;
            var controlType = Control.GetType();
            if (!SharpDX.Utilities.IsTypeInheritFrom(controlType, "System.Windows.Controls.Border"))
                throw new ArgumentException("Control is not supported. Must inherit from System.Windows.Controls.Border (WPF).");

            var dpiXProperty = typeof(SystemParameters).GetProperty("DpiX", BindingFlags.NonPublic | BindingFlags.Static);
            var dpiYProperty = typeof(SystemParameters).GetProperty("Dpi", BindingFlags.NonPublic | BindingFlags.Static);
            DpiX = (int)dpiXProperty.GetValue(null, null);
            DpiY = (int)dpiYProperty.GetValue(null, null);
        }

        private static object CreateDefaultControl()
        {
            return new RenderForm("Odyssey Application") { AllowUserResizing = false };
        }

        /// <summary>
        /// Gets or sets a value indicating whether the render loop should use the default <see cref="System.Windows.Forms.Application.DoEvents"/> instead of a custom window message loop lightweight for GC. Default is false
        /// </summary>
        /// <value><c>true</c> if use the default <see cref="System.Windows.Forms.Application.DoEvents"/>; otherwise, <c>false</c>.</value>
        public bool UseApplicationDoEvents { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Odyssey.Engine
{
    public class WinRTApplicationContext : ApplicationContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameContext"/> class.
        /// </summary>
        /// <param name="control">The control, platform dependent. See remarks for supported controls.</param>
        /// <param name="requestedWidth">Width of the requested.</param>
        /// <param name="requestedHeight">Height of the requested.</param>
        /// <exception cref="System.ArgumentException">Control is not supported. Must inherit from System.Windows.Forms.Control (WinForm) or System.Windows.Controls.Border (WPF hosting WinForm) or SharpDX.Toolkit.SharpDXElement (WPF)</exception>
        /// <remarks>
        /// On Windows Phone, the Toolkit supports the following controls:
        /// <ul>
        /// <li>XAML control inheriting <see cref="System.Windows.Controls.DrawingSurfaceBackgroundGrid"/></li>
        /// <li>XAML control inheriting <see cref="System.Windows.Controls.DrawingSurface"/></li>
        /// </ul>
        /// </remarks>
        public WinRTApplicationContext(object control = null, int requestedWidth = 0, int requestedHeight = 0) : base(ApplicationContextType.WinRT)
        {
            Contract.Requires<ArgumentNullException>(control != null, "Does no support null control on WP8 platform");

            Control = control;
            RequestedWidth = requestedWidth;
            RequestedHeight = requestedHeight;
            var controlType = Control.GetType();
            if (Utilities.IsTypeInheritFrom(controlType, "System.Windows.Controls.DrawingSurfaceBackgroundGrid"))
            {
                ContextType = GameContextType.WindowsPhoneBackgroundXaml;
            }
            else if (Utilities.IsTypeInheritFrom(controlType, "System.Windows.Controls.DrawingSurface"))
            {
                ContextType = GameContextType.WindowsPhoneXaml;
            }
            else
            {
                throw new ArgumentException("Control is not supported. Must inherit from System.Windows.Forms.Control (WinForm) or System.Windows.Controls.Border (WPF hosting WinForm) or SharpDX.Toolkit.SharpDXElement (WPF)");
            }
        }
    }
}

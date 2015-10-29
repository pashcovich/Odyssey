#region Using Directives

using System;
using System.Collections.Generic;
using System.IO;

#endregion Using Directives

namespace Odyssey.Engine
{
    internal class DesktopApplicationPlatform : ApplicationPlatform
    {
        public DesktopApplicationPlatform(Application application)
            : base(application)
        {
        }

        /// <summary>
        /// Returns the default directory name where the current game runs.
        /// </summary>
        public override string DefaultAppDirectory
        {
            get
            {
                var assemblyUri = new Uri(Application.GetType().Assembly.CodeBase);
                return Path.GetDirectoryName(assemblyUri.LocalPath);
            }
        }

        /// <summary>
        /// Gets the list of supported application window instances.
        /// </summary>
        /// <returns>The list of supported <see cref="ApplicationWindow"/> instances.</returns>
        /// <remarks>Supports WinForms on any platform, and WPF only on DX11 (DX11.1 and up is not supported for WPF).</remarks>
        protected override IEnumerable<ApplicationWindow> GetSupportedApplicationWindows()
        {
            return new ApplicationWindow[] {new DesktopApplicationWindow()};
        }

        protected override void CreatePresenter(DirectXDevice device, PresentationParameters parameters, object newControl = null)
        {
            base.CreatePresenter(device, parameters, newControl);
            // Give a chance to applicationWindow to create desired graphics presenter, otherwise - create our own.
            var swapchainPresenter = new StereoDesktopPresenter(device, parameters);
            device.Presenter = swapchainPresenter;
            Application.Services.AddService(typeof (ISwapChainPresenterService), swapchainPresenter);
        }
    }
}
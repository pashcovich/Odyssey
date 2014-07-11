using Odyssey.UserInterface;
using Odyssey.UserInterface.Style;
using System.Collections.Generic;

namespace Odyssey.Engine
{
    [RequiredService(typeof(IUserInterfaceState), typeof(DesktopUserInterfaceManager))]
    [RequiredService(typeof(IStyleService), typeof(StyleManager))]
    [PlatformType(typeof(DesktopApplicationPlatform))]
    internal class DesktopWpfApplicationPlatform : DesktopApplicationPlatform
    {
        public DesktopWpfApplicationPlatform(Application application)
            : base(application)
        {
        }

        protected override IEnumerable<ApplicationWindow> GetSupportedApplicationWindows()
        {
            return new ApplicationWindow[] { new DesktopWpfApplicationWindow(), };
        }
    }
}
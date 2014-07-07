using Odyssey.Engine;
using System;
using System.Collections.Generic;
using Odyssey.UserInterface;
using Odyssey.UserInterface.Style;

namespace Odyssey
{
    [RequiredService(typeof(IUserInterfaceState), typeof(DesktopUserInterfaceManager))]
    [RequiredService(typeof(IStyleService), typeof(StyleManager))]
    [PlatformType(typeof(DesktopWpfApplicationPlatform))]
    public class DesktopWpfApplication : Application
    {
        protected DesktopWpfApplication()
            : base()
        {
        }
    }
}
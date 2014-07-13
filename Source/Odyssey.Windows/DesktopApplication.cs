#region Using Directives

using Odyssey.Engine;
using Odyssey.UserInterface;
using Odyssey.UserInterface.Style;

#endregion Using Directives

namespace Odyssey
{
    [RequiredService(typeof(IDirect2DService), typeof(Direct2DDeviceManager))]
    [RequiredService(typeof(IUserInterfaceState), typeof(DesktopUserInterfaceManager))]
    [RequiredService(typeof(IStyleService), typeof(StyleManager))]
    [PlatformType(typeof(DesktopApplicationPlatform))]
    public class DesktopApplication : Application
    {
        protected DesktopApplication()
            : base()
        {
        }
    }
}
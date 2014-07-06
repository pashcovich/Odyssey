#region Using Directives

using System.Collections.Generic;

#endregion Using Directives

namespace Odyssey.Engine
{
    public interface IDirectXDeviceFactory
    {
        List<DeviceInformation> FindBestDevices(ApplicationGraphicsParameters graphicsParameters);

        DirectXDevice CreateDevice(DeviceInformation deviceInformation);
    }
}
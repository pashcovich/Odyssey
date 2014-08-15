using System;

namespace Odyssey.Engine
{
    public interface IDirect2DService
    {
        event EventHandler<EventArgs> DeviceCreated;
        event EventHandler<EventArgs> DeviceDisposing;
        Direct2DDevice Direct2DDevice { get; }
    }
}
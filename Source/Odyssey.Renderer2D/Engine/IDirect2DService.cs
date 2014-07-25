using System;

namespace Odyssey.Engine
{
    public interface IDirect2DService
    {
        event EventHandler<EventArgs> DeviceCreated;
        Direct2DDevice Direct2DDevice { get; }
    }
}
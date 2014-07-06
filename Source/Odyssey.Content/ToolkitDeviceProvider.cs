using System;
using SharpDX;

namespace Odyssey.Content
{
    public class ToolkitDeviceProvider : Component, SharpDX.Toolkit.Graphics.IGraphicsDeviceService
    {
        public GraphicsDevice GraphicsDevice { get; private set; }

        public event EventHandler<EventArgs> DeviceChangeBegin;

        public event EventHandler<EventArgs> DeviceChangeEnd;

        public event EventHandler<EventArgs> DeviceCreated;

        public event EventHandler<EventArgs> DeviceDisposing;

        public event EventHandler<EventArgs> DeviceLost;


    }
}

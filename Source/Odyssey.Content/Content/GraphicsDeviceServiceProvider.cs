using SharpDX.Toolkit.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Graphics.Resources
{
    public class GraphicsDeviceServiceProvider : IGraphicsDeviceService
    {

        public event EventHandler<EventArgs> DeviceCreated;

        public event EventHandler<EventArgs> DeviceDisposing;

        public event EventHandler<EventArgs> DeviceLost;

        public GraphicsDeviceServiceProvider()
        {
            GraphicsDevice = GraphicsDevice.New(Global.DeviceManager.DeviceDirect3D);

            DeviceCreated += GraphicsDeviceServiceProvider_DeviceCreated;
        }

        void GraphicsDeviceServiceProvider_DeviceCreated(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("oh no!");
        }

        public GraphicsDevice GraphicsDevice
        {
            get;
            private set;
        }
    }
}

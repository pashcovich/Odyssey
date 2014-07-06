#region Using Directives

using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using Device = SharpDX.Direct3D11.Device;
using Device1 = SharpDX.Direct3D11.Device1;
using Resource = SharpDX.Direct3D11.Resource;

#endregion Using Directives

namespace Odyssey.Engine
{
    public class SimpleDeviceManager : Component, IDirectXDeviceService, IDirect3DProvider, IDirectXDeviceSettings,
        ISwapChainPresenterService
    {
        private Texture2D backBuffer;
        private DeviceContext1 context;
        private Device1 device;
        private RenderTargetView renderTargetView;
        private SwapChain swapChain;

        public SimpleDeviceManager(IServiceRegistry services)
        {
            services.AddService(typeof(IDirectXDeviceService), this);
            services.AddService(typeof(IDirectXDeviceSettings), this);
            services.AddService(typeof(ISwapChainPresenterService), this);
        }

        public event EventHandler<EventArgs> DeviceChangeBegin;

        public event EventHandler<EventArgs> DeviceChangeEnd;

        public event EventHandler<EventArgs> DeviceCreated;

        public event EventHandler<EventArgs> DeviceDisposing;

        public event EventHandler<EventArgs> DeviceLost;

        public RenderTargetView BackBuffer
        {
            get { return renderTargetView; }
        }

        public DeviceContext1 Context
        {
            get { return context; }
        }

        public Device1 Device
        {
            get { return device; }
        }

        public IDirect3DProvider DirectXDevice
        {
            get { return this; }
        }

        public float HorizontalDpi { get; set; }

        public bool IsDebugMode { get; private set; }

        public bool IsFullScreen { get; set; }

        public bool IsStereo { get; set; }

        public Format PreferredBackBufferFormat { get; set; }

        public int PreferredBackBufferHeight { get; set; }

        public int PreferredBackBufferWidth { get; set; }

        public FeatureLevel[] PreferredGraphicsProfile { get; set; }

        public int PreferredMultiSampleCount { get; set; }

        public SwapChain SwapChain
        {
            get { return swapChain; }
        }

        public float VerticalDpi { get; set; }

        public void CreateDevice(object hostControl, DeviceCreationFlags flags)
        {
            OnDeviceChangeBegin(this, EventArgs.Empty);
            // SwapChain description
            var desc = new SwapChainDescription()
            {
                BufferCount = 1,
                ModeDescription = new ModeDescription(PreferredBackBufferWidth, PreferredBackBufferHeight, new Rational(60, 1),
                    PreferredBackBufferFormat),
                IsWindowed = true,
                OutputHandle = (IntPtr)hostControl,
                SampleDescription = new SampleDescription(1, 0),
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput
            };
            IsDebugMode = flags.HasFlag(DeviceCreationFlags.Debug);
            var device = ToDispose(new Device(DriverType.Hardware, flags, PreferredGraphicsProfile[0]));
            this.device = ToDispose(device.QueryInterface<Device1>());

            var factory = ToDispose(new Factory1());
            swapChain = ToDispose(new SwapChain(factory, device, desc));
            factory.MakeWindowAssociation(swapChain.Description.OutputHandle, WindowAssociationFlags.IgnoreAll);

            // New RenderTargetView from the backbuffer
            backBuffer = ToDispose(Resource.FromSwapChain<Texture2D>(swapChain, 0));
            renderTargetView = ToDispose(new RenderTargetView(device, backBuffer));
            context = ToDispose(this.device.ImmediateContext.QueryInterface<DeviceContext1>());
            OnDeviceCreated(this, EventArgs.Empty);
            OnDeviceChangeEnd(this, EventArgs.Empty);
        }

        protected override void Dispose(bool disposeManagedResources)
        {
            OnDeviceDisposing(this, EventArgs.Empty);
            base.Dispose(disposeManagedResources);
        }

        protected virtual void OnDeviceChangeBegin(object sender, EventArgs args)
        {
            RaiseEvent(DeviceChangeBegin, sender, args);
        }

        protected virtual void OnDeviceChangeEnd(object sender, EventArgs args)
        {
            RaiseEvent(DeviceChangeEnd, sender, args);
        }

        protected virtual void OnDeviceCreated(object sender, EventArgs args)
        {
            RaiseEvent(DeviceCreated, sender, args);
        }

        protected virtual void OnDeviceDisposing(object sender, EventArgs args)
        {
            RaiseEvent(DeviceDisposing, sender, args);
        }

        protected virtual void OnDeviceLost(object sender, EventArgs args)
        {
            RaiseEvent(DeviceLost, sender, args);
        }

        private void RaiseEvent<T>(EventHandler<T> handler, object sender, T args)
            where T : EventArgs
        {
            if (handler != null)
                handler(sender, args);
        }
    }
}
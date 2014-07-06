using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct3D;
using System;
using SharpDX.Direct3D11;
using SharpDX.WIC;
using Device = SharpDX.Direct2D1.Device;
using Device1 = SharpDX.Direct3D11.Device1;
using DeviceContext = SharpDX.Direct2D1.DeviceContext;
using DeviceContext1 = SharpDX.Direct3D11.DeviceContext1;
using Factory = SharpDX.DirectWrite.Factory;
using FeatureLevel = SharpDX.Direct3D.FeatureLevel;

namespace Odyssey.Engine
{
    public class DeviceManager : Component,  IDirectXProvider, IDirect3DProvider 
#if !WP8
, IDirect2DProvider, IDirectWriteProvider
#endif
    {
        protected float dpi;
        protected FeatureLevel featureLevel;

#if (DIRECTX11_2 || DIRECTX11_1)
        // Direct3D Objects
        protected Device1 d3dDevice;
        protected DeviceContext1 d3dContext;
        /// <summary>
        /// Gets the Direct3D11 device.
        /// </summary>
        public Device1 DeviceDirect3D
        {
            get { return d3dDevice; }
        }
        /// <summary>
        /// Gets the Direct3D11 context.
        /// </summary>
        public DeviceContext1 ContextDirect3D { get { return d3dContext; } }
#else
        // Direct3D Objects
        protected SharpDX.Direct3D11.Device d3dDevice;
        protected SharpDX.Direct3D11.DeviceContext d3dContext;
        /// <summary>
        /// Gets the Direct3D11 device.
        /// </summary>
        public SharpDX.Direct3D11.Device DeviceDirect3D
        {
            get { return d3dDevice; }
        }
        /// <summary>
        /// Gets the Direct3D11 context.
        /// </summary>
        public SharpDX.Direct3D11.DeviceContext ContextDirect3D { get { return d3dContext; } }
#endif
#if !WP8
        // Declare Direct2D Objects
        protected DeviceContext d2dContext;
        protected Device d2dDevice;

        protected Factory1 d2dFactory;

        // Declare DirectWrite & Windows Imaging Component Objects
        protected Factory dwriteFactory;
        protected ImagingFactory2 wicFactory;
#endif


#if !WP8

        /// <summary>
        /// Gets the Direct2D context.
        /// </summary>
        public DeviceContext ContextDirect2D { get { return d2dContext; } }

        /// <summary>
        /// Gets the Direct2D device.
        /// </summary>
        public Device DeviceDirect2D { get { return d2dDevice; } }

        /// <summary>
        /// Gets the Direct2D factory.
        /// </summary>
        public Factory1 FactoryDirect2D { get { return d2dFactory; } }
        /// <summary>
        /// Gets the DirectWrite factory.
        /// </summary>
        public Factory FactoryDirectWrite { get { return dwriteFactory; } }

        /// <summary>
        /// Gets the WIC factory.
        /// </summary>
        public ImagingFactory2 WICFactory { get { return wicFactory; } }
#endif

#if !WP8
        /// <summary>
        /// Gets or sets the DPI.
        /// </summary>
        /// <remarks>
        /// This method will fire the event <see cref="DpiChanged"/> if the dpi is modified.
        /// </remarks>
        public float Dpi
        {
            get
            {
                return dpi;
            }
            set
            {
                if (dpi == value) return;
                
                dpi = value;

                d2dContext.DotsPerInch = new Size2F(dpi, dpi);

                Settings.Dpi = dpi;

                if (DpiChanged != null)
                    DpiChanged(this, EventArgs.Empty);
            }
        }
#endif

        public DeviceSettings Settings { get; private set; }

        /// <summary>
        /// This event is fired when the DeviceMamanger is initialized by the
        /// <see cref="Initialized"/> method.
        /// </summary>
        public event EventHandler<InitializeDirectXEventArgs> Initialized;

        /// <summary>
        /// This event is fired when the <see cref="Dpi"/> is called,
        /// </summary>
        public event EventHandler<EventArgs> DpiChanged;

        public void Initialize(DeviceSettings settings)
        {
            Settings = settings;
#if !WP8
            CreateDeviceIndependentResources();
#endif
            CreateDeviceResources();
        }

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public void InitializeDeviceDependentResources()
        {
            if (Initialized != null)
                Initialized(this, new InitializeDirectXEventArgs(this, Settings));
        }

#if !WP8

        /// <summary>
        /// Creates device independent resources.
        /// </summary>
        /// <remarks>
        /// This method is called at the initialization of this instance.
        /// </remarks>
        protected void CreateDeviceIndependentResources()
        {
#if DEBUG
            const DebugLevel debugLevel = DebugLevel.Information;
#else
            var debugLevel = SharpDX.Direct2D1.DebugLevel.None;
#endif

            // Dispose previous references and set to null
            RemoveAndDispose(ref d2dFactory);
            RemoveAndDispose(ref dwriteFactory);
            RemoveAndDispose(ref wicFactory);

            // Allocate new references
            d2dFactory = ToDispose(new Factory1(FactoryType.SingleThreaded, debugLevel));
            dwriteFactory = ToDispose(new Factory(SharpDX.DirectWrite.FactoryType.Shared));
            wicFactory = ToDispose(new ImagingFactory2());
        }

#endif

#if (DIRECTX11_2 || DIRECTX11_1)
        void CreateDirect3D11_2Device()
        {
            // Allocate new references Enable compatibility with Direct2D Retrieve the Direct3D 11.2
            // device amd device context
            var creationFlags = DeviceCreationFlags.VideoSupport | DeviceCreationFlags.BgraSupport
                | DeviceCreationFlags.Debug;

            // Decomment this line to have Debug. Unfortunately, debug is sometimes crashing
            // applications, so it is disable by default
            try
            {
                // Try to create it with Video Support If it is not working, we just use BGRA Force
                // to FeatureLevel.Level_9_1
                using (var defaultDevice = new SharpDX.Direct3D11.Device(DriverType.Hardware, creationFlags))
                    d3dDevice = ToDispose(defaultDevice.QueryInterface<Device1>());
            }
            catch (Exception)
            {
                creationFlags = DeviceCreationFlags.BgraSupport;
                using (var defaultDevice = new SharpDX.Direct3D11.Device(DriverType.Hardware, creationFlags))
                    d3dDevice = ToDispose(defaultDevice.QueryInterface<Device1>());
            }
            featureLevel = d3dDevice.FeatureLevel;

            // Get Direct3D 11.2 context
            d3dContext = ToDispose(d3dDevice.ImmediateContext.QueryInterface<DeviceContext1>());
        }
#else
        void CreateDirect3D11Device()
        {
            // Allocate new references Enable compatibility with Direct2D Retrieve the Direct3D 11.2
            // device amd device context
            var creationFlags = SharpDX.Direct3D11.DeviceCreationFlags.BgraSupport | SharpDX.Direct3D11.DeviceCreationFlags.Debug;

            try
            {
                using (var defaultDevice = new SharpDX.Direct3D11.Device(DriverType.Hardware, creationFlags))
                    d3dDevice = ToDispose(defaultDevice.QueryInterface<SharpDX.Direct3D11.Device>());
            }
            catch (Exception)
            {
                creationFlags = SharpDX.Direct3D11.DeviceCreationFlags.BgraSupport;
                using (var defaultDevice = new SharpDX.Direct3D11.Device(DriverType.Hardware, creationFlags))
                    d3dDevice = ToDispose(defaultDevice.QueryInterface<SharpDX.Direct3D11.Device>());
            }
            featureLevel = d3dDevice.FeatureLevel;

            // Get Direct3D 11 context
            d3dContext = ToDispose(d3dDevice.ImmediateContext.QueryInterface<SharpDX.Direct3D11.DeviceContext>());
        }
#endif

        /// <summary>
        /// Creates device resources.
        /// </summary>
        /// <remarks>
        /// This method is called at the initialization of this instance.
        /// </remarks>
        void CreateDeviceResources()
        {
            // Dispose previous references and set to null
            RemoveAndDispose(ref d3dDevice);
            RemoveAndDispose(ref d3dContext);

#if (DIRECTX11_1 || DIRECTX11_2)
            CreateDirect3D11_2Device();
#else

            CreateDirect3D11Device();
#endif

#if !WP8
            RemoveAndDispose(ref d2dDevice);
            RemoveAndDispose(ref d2dContext);

            // Create Direct2D device
            using (var dxgiDevice = d3dDevice.QueryInterface<SharpDX.DXGI.Device>())
                d2dDevice = ToDispose(new Device(d2dFactory, dxgiDevice));

            // Create Direct2D context
            d2dContext = ToDispose(new DeviceContext(d2dDevice, DeviceContextOptions.None));
#endif
        }

        #region DeviceProviders

#if DIRECTX11_1

        DeviceContext1 IDirect3DProvider.Context
        {
            get { return d3dContext; }
        }
        Device1 IDirect3DProvider.Device
        {
            get { return d3dDevice; }
        }
#else
        SharpDX.Direct3D11.DeviceContext IDirect3DProvider.Context
        {
            get { return d3dContext; }
        }
        SharpDX.Direct3D11.Device IDirect3DProvider.Device
        {
            get { return d3dDevice; }
        }
#endif

#if !WP8
        DeviceContext IDirect2DProvider.Context
        {
            get { return d2dContext; }
        }

        Device IDirect2DProvider.Device
        {
            get { return d2dDevice; }
        }
        Factory1 IDirect2DProvider.Factory
        {
            get { return d2dFactory; }
        }

        Factory IDirectWriteProvider.Factory
        {
            get { return dwriteFactory; }
        }
        IDirect2DProvider IDirectXProvider.Direct2D
        { get { return this; } }

        IDirect3DProvider IDirectXProvider.Direct3D
        { get { return this; } }

        IDirectWriteProvider IDirectXProvider.DirectWrite
        { get { return this; } }
#endif



        #endregion DeviceProviders
    }
}
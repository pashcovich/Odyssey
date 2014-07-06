#region Using Directives

using Odyssey.Graphics;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using System;
using System.Diagnostics.Contracts;
using Brush = Odyssey.Graphics.Shapes.Brush;
using Factory = SharpDX.Direct2D1.Factory;
using Factory1 = SharpDX.DirectWrite.Factory1;
using FactoryType = SharpDX.Direct2D1.FactoryType;

#endregion Using Directives

namespace Odyssey.Engine
{
    public class Direct2DDevice : Component, IDirect2DDevice
    {
        private readonly DebugLevel debugLevel;
        private readonly IDirectXDeviceService dx11Service;
        private readonly IServiceRegistry services;
        private BackBufferSurface backBuffer;
        private Device device;
        private DeviceContext deviceContext;
        private Factory1 directWriteFactory;
        private IDirect3DProvider dxDeviceCache;
        private Factory factory;
        private Direct2DSurface target;

        public Direct2DDevice(IServiceRegistry services)
            : this(services, DebugLevel.None)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Direct2DDevice" />, subscribes to <see cref="IDirectXDeviceService" /> changes
        /// events via
        /// <see cref="IDirectXDeviceService" />.
        /// </summary>
        /// <exception cref="ArgumentNullException">If <paramref name="services" /> is null.</exception>
        public Direct2DDevice(IServiceRegistry services, DebugLevel debugLevel)
        {
            Contract.Requires<ArgumentNullException>(services != null, "services");
            dx11Service = services.GetService<IDirectXDeviceService>();
            this.services = services;
            this.debugLevel = debugLevel;

            dx11Service.DeviceCreated += DirectXDx11ServiceOnDx11Created;
            dx11Service.DeviceDisposing += DirectXDx11ServiceOnDx11Disposing;
            dx11Service.DeviceChangeBegin += DirectXDx11ServiceOnDx11ChangeBegin;
            dx11Service.DeviceChangeEnd += DirectXDx11ServiceOnDx11ChangeEnd;
            dx11Service.DeviceLost += DirectXDx11ServiceOnDx11Lost;
        }

        public Direct2DSurface BackBuffer
        {
            get { return backBuffer; }
        }

        /// <summary>
        /// Gets a reference to the Direct2D device.
        /// </summary>
        public Device Device
        {
            get { return device; }
        }

        /// <summary>
        /// Gets a reference to the default <see cref="Direct2D1.DeviceContext" />.
        /// </summary>
        public DeviceContext DeviceContext
        {
            get { return deviceContext; }
        }

        /// <summary>
        /// Gets a reference to the default <see cref="SharpDX.DirectWrite.Factory1" />.
        /// </summary>
        public Factory1 DirectWriteFactory
        {
            get { return directWriteFactory; }
        }

        public bool IsDebugMode
        {
            get { return debugLevel != DebugLevel.None; }
        }

        public Direct2DSurface Target
        {
            get { return target; }
            set
            {
                if (target != value)
                {
                    target = value;
                    DeviceContext.Target = target;
                }
            }
        }

        internal IServiceRegistry Services
        {
            get { return services; }
        }

        public void DrawGeometry(Graphics.Shapes.Geometry geometry, Brush brush)
        {
            deviceContext.DrawGeometry(geometry, brush);
        }

        public void DrawRectangle(ShapeBase shape, Brush brush, float strokeWidth = 1.0f)
        {
            deviceContext.DrawRectangle(shape.BoundingRectangle, brush, strokeWidth);
        }

        public void DrawTest(string text, TextFormat textFormat, RectangleF layoutRect, Brush foregroundBrush,
            DrawTextOptions textOptions = DrawTextOptions.None)
        {
            deviceContext.DrawText(text, textFormat, layoutRect, foregroundBrush, textOptions);
        }

        public void FillGeometry(Graphics.Shapes.Geometry geometry, Brush brush)
        {
            deviceContext.FillGeometry(geometry, brush);
        }

        public void FillRectangle(ShapeBase shape, Brush brush)
        {
            deviceContext.FillRectangle(shape.BoundingRectangle, brush);
        }

        public void SetTextAntialias(AntialiasMode antialiasMode)
        {
            deviceContext.AntialiasMode = antialiasMode;
        }

        public void SetTransform(Matrix3x2 transform)
        {
            deviceContext.Transform = transform;
        }

        /// <summary>
        /// Diposes all resources associated with the current <see cref="Direct2DDevice" /> instance.
        /// </summary>
        /// <param name="disposeManagedResources">Indicates whether to dispose management resources.</param>
        protected override void Dispose(bool disposeManagedResources)
        {
            base.Dispose(disposeManagedResources);

            DisposeAll();
        }

        private void CreateOrUpdateDirect2D()
        {
            // Dispose and recreate all devices only if the DirectXDevice changed
            if (dxDeviceCache != dx11Service.DirectXDevice)
            {
                dxDeviceCache = dx11Service.DirectXDevice;

                DisposeAll();

                var d3dDevice = dxDeviceCache.Device;

                using (var dxgiDevice = d3dDevice.QueryInterface<SharpDX.DXGI.Device>())
                {
                    device = ToDispose(new Device(dxgiDevice, new CreationProperties { DebugLevel = debugLevel }));
                    deviceContext = ToDispose(new DeviceContext(device, DeviceContextOptions.None));
                    factory = ToDispose(new SharpDX.Direct2D1.Factory1(FactoryType.SingleThreaded, debugLevel));
                }

                backBuffer = ToDispose(BackBufferSurface.New(this));
                backBuffer.Initialize();

                directWriteFactory = ToDispose(new Factory1());
            }
        }

        private void DirectXDx11ServiceOnDx11ChangeBegin(object sender, EventArgs e)
        {
        }

        private void DirectXDx11ServiceOnDx11ChangeEnd(object sender, EventArgs e)
        {
            CreateOrUpdateDirect2D();
        }

        /// <summary>
        /// Handles the <see cref="IDirectXDeviceService.DeviceCreated" /> event.
        /// Initializes the <see cref="Direct2DDevice.Device" /> and <see cref="DeviceContext" />.
        /// </summary>
        /// <param name="sender">Ignored.</param>
        /// <param name="e">Ignored.</param>
        private void DirectXDx11ServiceOnDx11Created(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the <see cref="IDirectXDeviceService.DeviceDisposing" /> event.
        /// Disposes the <see cref="Direct2DDevice.Device" />, <see cref="DeviceContext" /> and its render target
        /// associated with the current <see cref="Direct2DDevice" /> instance.
        /// </summary>
        /// <param name="sender">Ignored.</param>
        /// <param name="e">Ignored.</param>
        private void DirectXDx11ServiceOnDx11Disposing(object sender, EventArgs e)
        {
            DisposeAll();
        }

        private void DirectXDx11ServiceOnDx11Lost(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Disposes the <see cref="Direct2DDevice.Device" />, <see cref="DeviceContext" /> and its render target
        /// associated with the current <see cref="Direct2DDevice" /> instance.
        /// </summary>
        private void DisposeAll()
        {
            if (target != backBuffer)
                RemoveAndDispose(ref target);
            RemoveAndDispose(ref backBuffer);
            RemoveAndDispose(ref directWriteFactory);
            RemoveAndDispose(ref deviceContext);
            RemoveAndDispose(ref device);
        }

        #region Operators

        public static implicit operator DeviceContext(Direct2DDevice from)
        {
            return from == null ? null : from.deviceContext;
        }

        public static implicit operator Factory(Direct2DDevice from)
        {
            return from == null ? null : from.factory;
        }

        public static implicit operator SharpDX.Direct3D11.Device(Direct2DDevice from)
        {
            return from == null ? null : from.dx11Service.DirectXDevice.Device;
        }

        public static implicit operator SharpDX.DirectWrite.Factory(Direct2DDevice from)
        {
            return from == null ? null : from.directWriteFactory;
        }

        #endregion Operators
    }
}
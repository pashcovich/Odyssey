#region Using Directives

using System;
using Odyssey.Core;
using Odyssey.Geometry;
using Odyssey.Graphics.Drawing;
using Odyssey.Reflection;
using Odyssey.UserInterface.Style;
using SharpDX.Direct2D1;
using SharpDX.Mathematics;
using SharpDX.Mathematics.Interop;
using Brush = Odyssey.Graphics.Brush;
using D2DFactory = SharpDX.Direct2D1.Factory1;
using DWFactory = SharpDX.DirectWrite.Factory1;
using Ellipse = Odyssey.Graphics.Drawing.Ellipse;
using Factory = SharpDX.DirectWrite.Factory;

#endregion

namespace Odyssey.Engine
{
    public class Direct2DDevice : Component, IDirect2DDevice
    {
        private readonly DebugLevel debugLevel;
        private readonly IServiceRegistry services;
        private BackBufferSurface backBuffer;
        private Device device;
        private DeviceContext deviceContext;
        private DWFactory directWriteFactory;
        private D2DFactory factory;
        private Direct2DSurface target;

        /// <summary>
        ///     Initializes a new instance of <see cref="Direct2DDevice" />.
        ///     events via
        ///     <see cref="IDirectXDeviceService" />.
        /// </summary>
        /// <exception cref="ArgumentNullException">If <paramref name="services" /> is null.</exception>
        public Direct2DDevice(IServiceRegistry services, SharpDX.Direct3D11.Device d3dDevice, DebugLevel debugLevel)
        {
            this.services = services;
            this.debugLevel = debugLevel;

            using (var dxgiDevice = d3dDevice.QueryInterface<SharpDX.DXGI.Device>())
            {
                factory = ToDispose(new Factory1(FactoryType.SingleThreaded, debugLevel));
                device = ToDispose(new Device(factory, dxgiDevice));
                deviceContext = ToDispose(new DeviceContext(device, DeviceContextOptions.None));
            }

            backBuffer = ToDispose(BackBufferSurface.New(this));
            backBuffer.Initialize();
            directWriteFactory = ToDispose(new DWFactory());
        }

        public Direct2DSurface BackBuffer
        {
            get { return backBuffer; }
        }

        public float HorizontalDpi
        {
            get { return deviceContext.DotsPerInch.Width; }
        }

        public float VerticalDpi
        {
            get { return deviceContext.DotsPerInch.Height; }
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

        public Matrix3x2 Transform
        {
            get { return deviceContext.Transform; }
            set { deviceContext.Transform = value; }
        }

        /// <summary>
        ///     Gets a reference to the Direct2D device.
        /// </summary>
        public Device Device
        {
            get { return device; }
        }

        /// <summary>
        ///     Gets a reference to the default <see cref="DeviceContext" />.
        /// </summary>
        public DeviceContext DeviceContext
        {
            get { return deviceContext; }
        }

        /// <summary>
        ///     Gets a reference to the default <see cref="SharpDX.DirectWrite.Factory1" />.
        /// </summary>
        public DWFactory DirectWriteFactory
        {
            get { return directWriteFactory; }
        }

        public void DrawGeometry(Graphics.Drawing.Geometry geometry, Brush brush, float strokeThickness = 1.0f)
        {
            deviceContext.DrawGeometry(geometry, brush, strokeThickness);
        }

        public void DrawRectangle(Shape shape, Brush brush, float strokeThickness = 1.0f)
        {
            deviceContext.DrawRectangle(Convert(shape.BoundingRectangle), brush, strokeThickness);
        }

        public void DrawEllipse(Ellipse ellipse, Brush brush, float strokeThickness = 1.0f)
        {
            deviceContext.DrawEllipse(ellipse, brush, strokeThickness);
        }

        public void FillEllipse(Ellipse ellipse, Brush brush)
        {
            deviceContext.FillEllipse(ellipse, brush);
        }

        public void DrawLine(Line line, Brush brush, float strokeThickness = 1.0f)
        {
            deviceContext.DrawLine(line.P0 + line.Position.XY(), line.P1 + line.Position.XY(), brush, strokeThickness);
        }

        public void DrawText(string text, TextFormat textFormat, RectangleF layoutRect, Brush foregroundBrush,
            DrawTextOptions textOptions = DrawTextOptions.None)
        {
            deviceContext.DrawText(text, textFormat, Convert(layoutRect), foregroundBrush, textOptions);
        }

        public void FillGeometry(Graphics.Drawing.Geometry geometry, Brush brush)
        {
            deviceContext.FillGeometry(geometry, brush);
        }

        public void FillRectangle(Shape shape, Brush brush)
        {
            FillRectangle(shape.BoundingRectangle, brush);
        }

        public void FillRectangle(RectangleF rectangle, Brush brush)
        {
            deviceContext.FillRectangle(Convert(rectangle), brush);
        }

        public void SetTextAntialias(AntialiasMode antialiasMode)
        {
            deviceContext.AntialiasMode = antialiasMode;
        }

        private static RawRectangleF Convert(RectangleF rectangle)
        {
            object s = rectangle;
            return ReflectionHelper.CopyStruct<RawRectangleF>(ref s);
        }

        public void SetPrimitiveBlend(PrimitiveBlend blendMode)
        {
            deviceContext.PrimitiveBlend = blendMode;
        }

        /// <summary>
        ///     Diposes all resources associated with the current <see cref="Direct2DDevice" /> instance.
        /// </summary>
        /// <param name="disposeManagedResources">Indicates whether to dispose management resources.</param>
        protected override void Dispose(bool disposeManagedResources)
        {
            base.Dispose(disposeManagedResources);

            DisposeAll();
        }

        /// <summary>
        ///     Disposes the <see cref="Direct2DDevice.Device" />, <see cref="DeviceContext" /> and its render target
        ///     associated with the current <see cref="Direct2DDevice" /> instance.
        /// </summary>
        public void DisposeAll()
        {
            device.ClearResources(0);

            if (target != backBuffer)
                RemoveAndDispose(ref target);
            RemoveAndDispose(ref backBuffer);

            RemoveAndDispose(ref directWriteFactory);
            RemoveAndDispose(ref factory);
            RemoveAndDispose(ref deviceContext);
            RemoveAndDispose(ref device);
        }

        #region Operators

        public static implicit operator D2DFactory(Direct2DDevice from)
        {
            return from == null ? null : from.factory;
        }

        public static implicit operator DeviceContext(Direct2DDevice from)
        {
            return from == null ? null : from.deviceContext;
        }

        public static implicit operator SharpDX.Direct3D11.Device(Direct2DDevice from)
        {
            return from == null ? null : from.dx11Service.DirectXDevice.Device;
        }

        public static implicit operator Factory(Direct2DDevice from)
        {
            return from == null ? null : from.directWriteFactory;
        }

        #endregion Operators
    }
}
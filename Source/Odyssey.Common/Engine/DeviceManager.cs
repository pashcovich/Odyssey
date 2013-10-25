﻿using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute,
// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
// NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Engine
{
    /// <summary>
    /// This class handles device creation for Direct2D, Direct3D, DirectWrite and WIC.
    /// </summary>
    /// <remarks>
    /// SharpDX CommonDX is inspired from the DirectXBase C++ class from Win8 Metro samples, but the
    /// design is slightly improved in order to reuse components more easily.
    /// <see cref="DeviceManager"/> is responsible for device creation. <see cref="TargetBase"/> is
    /// responsible for rendering, render target creation. Initialization and Rendering is event
    /// driven based, allowing a better reuse of different components.
    /// </remarks>
    public class DeviceManager : Component, IDirectXProvider, IDirect3DProvider
#if !WP8
, IDirect2DProvider, IDirectWriteProvider
#endif
    {
        protected float dpi;
        protected FeatureLevel featureLevel;
        
        // Direct3D Objects
        protected SharpDX.Direct3D11.Device1 d3dDevice;
        protected SharpDX.Direct3D11.DeviceContext1 d3dContext;

#if !WP8
        protected SharpDX.Direct2D1.DeviceContext d2dContext;
        protected SharpDX.Direct2D1.Device d2dDevice;

        // Declare Direct2D Objects
        protected SharpDX.Direct2D1.Factory1 d2dFactory;

        // Declare DirectWrite & Windows Imaging Component Objects
        protected SharpDX.DirectWrite.Factory dwriteFactory;


        protected SharpDX.WIC.ImagingFactory2 wicFactory;
#endif
        /// <summary>
        /// Gets the Direct3D11 device.
        /// </summary>
        public SharpDX.Direct3D11.Device1 DeviceDirect3D { 
            get { return d3dDevice; } 
        }
        /// <summary>
        /// Gets the Direct3D11 context.
        /// </summary>
        public SharpDX.Direct3D11.DeviceContext1 ContextDirect3D { get { return d3dContext; } }

#if !WP8

        /// <summary>
        /// Gets the Direct2D context.
        /// </summary>
        public SharpDX.Direct2D1.DeviceContext ContextDirect2D { get { return d2dContext; } }

        /// <summary>
        /// Gets the Direct2D device.
        /// </summary>
        public SharpDX.Direct2D1.Device DeviceDirect2D { get { return d2dDevice; } } 

                /// <summary>
        /// Gets the Direct2D factory.
        /// </summary>
        public SharpDX.Direct2D1.Factory1 FactoryDirect2D { get { return d2dFactory; } }
        /// <summary>
        /// Gets the DirectWrite factory.
        /// </summary>
        public SharpDX.DirectWrite.Factory FactoryDirectWrite { get { return dwriteFactory; } }

                /// <summary>
        /// Gets the WIC factory.
        /// </summary>
        public SharpDX.WIC.ImagingFactory2 WICFactory { get { return wicFactory; } }
#endif

#if !WP8
        /// <summary>
        /// Gets or sets the DPI.
        /// </summary>
        /// <remarks>
        /// This method will fire the event <see cref="OnDpiChanged"/> if the dpi is modified.
        /// </remarks>
        public virtual float Dpi
        {
            get
            {
                return dpi;
            }
            set
            {
                if (dpi != value)
                {
                    dpi = value;

                    d2dContext.DotsPerInch = new Size2F(dpi, dpi);


                    Settings.Dpi = dpi;

                    if (OnDpiChanged != null)
                        OnDpiChanged(this);
                }
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
        public event Action<DeviceManager> OnDpiChanged;

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
        public virtual void InitializeDeviceDependentResources()
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
        protected virtual void CreateDeviceIndependentResources()
        {
#if DEBUG
            var debugLevel = SharpDX.Direct2D1.DebugLevel.Information;
#else
            var debugLevel = SharpDX.Direct2D1.DebugLevel.None;
#endif

            // Dispose previous references and set to null
            RemoveAndDispose(ref d2dFactory);
            RemoveAndDispose(ref dwriteFactory);
            RemoveAndDispose(ref wicFactory);

            // Allocate new references
            d2dFactory = ToDispose(new SharpDX.Direct2D1.Factory1(SharpDX.Direct2D1.FactoryType.SingleThreaded, debugLevel));
            dwriteFactory = ToDispose(new SharpDX.DirectWrite.Factory(SharpDX.DirectWrite.FactoryType.Shared));
            wicFactory = ToDispose(new SharpDX.WIC.ImagingFactory2());
        }

#endif

        /// <summary>
        /// Creates device resources.
        /// </summary>
        /// <remarks>
        /// This method is called at the initialization of this instance.
        /// </remarks>
        protected virtual void CreateDeviceResources()
        {
            // Dispose previous references and set to null
            RemoveAndDispose(ref d3dDevice);
            RemoveAndDispose(ref d3dContext);

#if !WP8
            RemoveAndDispose(ref d2dDevice);
            RemoveAndDispose(ref d2dContext);
#endif
            // Allocate new references Enable compatibility with Direct2D Retrieve the Direct3D 11.1
            // device amd device context
            var creationFlags = SharpDX.Direct3D11.DeviceCreationFlags.VideoSupport | SharpDX.Direct3D11.DeviceCreationFlags.BgraSupport
                | SharpDX.Direct3D11.DeviceCreationFlags.Debug;

            // Decomment this line to have Debug. Unfortunately, debug is sometimes crashing
            // applications, so it is disable by default
            try
            {
                // Try to create it with Video Support If it is not working, we just use BGRA Force
                // to FeatureLevel.Level_9_1
                using (var defaultDevice = new SharpDX.Direct3D11.Device(DriverType.Hardware, creationFlags))
                    d3dDevice = ToDispose(defaultDevice.QueryInterface<SharpDX.Direct3D11.Device1>());
            }
            catch (Exception)
            {
                creationFlags = SharpDX.Direct3D11.DeviceCreationFlags.BgraSupport;
                using (var defaultDevice = new SharpDX.Direct3D11.Device(DriverType.Hardware, creationFlags))
                    d3dDevice = ToDispose(defaultDevice.QueryInterface<SharpDX.Direct3D11.Device1>());
            }
            featureLevel = d3dDevice.FeatureLevel;

            // Get Direct3D 11.1 context
            d3dContext = ToDispose(d3dDevice.ImmediateContext.QueryInterface<SharpDX.Direct3D11.DeviceContext1>());
#if !WP8
            // Create Direct2D device
            using (var dxgiDevice = d3dDevice.QueryInterface<SharpDX.DXGI.Device>())
                d2dDevice = ToDispose(new SharpDX.Direct2D1.Device(d2dFactory, dxgiDevice));

            // Create Direct2D context
            d2dContext = ToDispose(new SharpDX.Direct2D1.DeviceContext(d2dDevice, SharpDX.Direct2D1.DeviceContextOptions.None));
#endif
        }

        #region DeviceProviders

        
        SharpDX.Direct3D11.DeviceContext1 IDirect3DProvider.Context
        {
            get { return d3dContext; }
        }
        SharpDX.Direct3D11.Device1 IDirect3DProvider.Device
        {
            get { return d3dDevice; }
        }

        IDirect3DProvider IDirectXProvider.Direct3D
        { get { return this; } }
#if !WP8
        SharpDX.Direct2D1.DeviceContext IDirect2DProvider.Context
        {
            get { return d2dContext; }
        }
        SharpDX.Direct2D1.Device IDirect2DProvider.Device
        {
            get { return d2dDevice; }
        }
        SharpDX.Direct2D1.Factory1 IDirect2DProvider.Factory
        {
            get { return d2dFactory; }
        }

        SharpDX.DirectWrite.Factory IDirectWriteProvider.Factory
        {
            get { return dwriteFactory; }
        }
        IDirect2DProvider IDirectXProvider.Direct2D
        { get { return this; } }

        IDirectWriteProvider IDirectXProvider.DirectWrite
        { get { return this; } }
#endif



        #endregion DeviceProviders
    }
}
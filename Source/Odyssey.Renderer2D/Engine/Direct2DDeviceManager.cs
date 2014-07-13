#region License

// Copyright © 2013-2014 Avengers UTD - Adalberto L. Simeone
// 
// The Odyssey Engine is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License Version 3 as published by
// the Free Software Foundation.
// 
// The Odyssey Engine is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details at http://gplv3.fsf.org/

#endregion

#region Using Directives

using System;
using Odyssey.Geometry;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct3D11;

#endregion

namespace Odyssey.Engine
{
    public class Direct2DDeviceManager : Component, IDirect2DService
    {
        private readonly IServiceRegistry services;
        private Direct2DDevice d2dDevice;
        private IDirectXDeviceService dx11Service;
        private IDirect3DProvider dxDeviceCache;

        public Direct2DDeviceManager(IServiceRegistry services)
        {
            this.services = services;
            services.ServiceAdded += AttachToD3DDevice;
        }

        public Direct2DDevice Direct2DDevice
        {
            get { return d2dDevice; }
        }

        private void AttachToD3DDevice(object sender, ServiceEventArgs e)
        {
            if (e.ServiceType == typeof (IDirectXDeviceService))
            {
                dx11Service = (IDirectXDeviceService) e.Instance;
                dx11Service.DeviceCreated += DirectXDx11ServiceOnDx11Created;
                dx11Service.DeviceDisposing += DirectXDx11ServiceOnDx11Disposing;
                dx11Service.DeviceChangeBegin += DirectXDx11ServiceOnDx11ChangeBegin;
                dx11Service.DeviceChangeEnd += DirectXDx11ServiceOnDx11ChangeEnd;
                dx11Service.DeviceLost += DirectXDx11ServiceOnDx11Lost;
                services.ServiceAdded -= AttachToD3DDevice;
            }
        }

        private void CreateOrUpdateDirect2D()
        {
            // Dispose and recreate all devices only if the DirectXDevice changed
            if (dxDeviceCache != dx11Service.DirectXDevice)
            {
                dxDeviceCache = dx11Service.DirectXDevice;

                if (d2dDevice != null)
                    d2dDevice.DisposeAll();

                var d3dDevice = dxDeviceCache.Device;
                d2dDevice =
                    ToDispose(new Direct2DDevice(services, d3dDevice,
                        d3dDevice.CreationFlags.HasFlag(DeviceCreationFlags.Debug) ? DebugLevel.Warning : DebugLevel.None));
                var deviceSettings = services.GetService<IDirectXDeviceSettings>();
                if (!MathHelper.ScalarNearEqual(deviceSettings.HorizontalDpi, d2dDevice.HorizontalDpi) ||
                    !MathHelper.ScalarNearEqual(deviceSettings.VerticalDpi, d2dDevice.VerticalDpi))
                    throw new InvalidOperationException("Direct2D device DPI values do not match Direct3D device DPI values");
            }
        }

        private void DirectXDx11ServiceOnDx11Lost(object sender, EventArgs e)
        {
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
        /// Initializes the <see cref="Engine.Direct2DDevice.Device" /> and <see cref="SharpDX.Direct2D1.DeviceContext" />.
        /// </summary>
        /// <param name="sender">Ignored.</param>
        /// <param name="e">Ignored.</param>
        private void DirectXDx11ServiceOnDx11Created(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the <see cref="IDirectXDeviceService.DeviceDisposing" /> event.
        /// Disposes the <see cref="Engine.Direct2DDevice.Device" />, <see cref="SharpDX.Direct2D1.DeviceContext" /> and its render target
        /// associated with the current <see cref="Engine.Direct2DDevice" /> instance.
        /// </summary>
        /// <param name="sender">Ignored.</param>
        /// <param name="e">Ignored.</param>
        private void DirectXDx11ServiceOnDx11Disposing(object sender, EventArgs e)
        {
            d2dDevice.DisposeAll();
        }
    }
}
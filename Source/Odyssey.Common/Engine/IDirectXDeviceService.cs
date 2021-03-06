﻿using SharpDX.Direct3D11;
using System;

namespace Odyssey.Engine
{
    public interface IDirectXDeviceService
    {
        /// <summary>
        /// Occurs when a device is created.
        /// </summary>
        event EventHandler<EventArgs> DeviceCreated;

        /// <summary>
        /// Occurs when a device is disposing.
        /// </summary>
        event EventHandler<EventArgs> DeviceDisposing;

        /// <summary>
        /// Occurs when a device is lost.
        /// </summary>
        event EventHandler<EventArgs> DeviceLost;

        /// <summary>
        /// Occurs right before device is about to change (recreate or resize)
        /// </summary>
        event EventHandler<EventArgs> DeviceChangeBegin;

        /// <summary>
        /// Occurs when device is changed (recreated or resized)
        /// </summary>
        event EventHandler<EventArgs> DeviceChangeEnd;

        IDirect3DProvider DirectXDevice { get; }
    }
}
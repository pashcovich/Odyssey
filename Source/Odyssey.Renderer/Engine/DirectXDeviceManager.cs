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

#endregion License

#region Using Directives

using Odyssey.Graphics;
using Odyssey.Utilities.Logging;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using ResultCode = SharpDX.DXGI.ResultCode;

#endregion Using Directives

namespace Odyssey.Engine
{
    /// <summary>
    /// Manages the <see cref="DirectXDevice"/> lifecycle.
    /// </summary>
    public class DirectXDeviceManager : Component, IDirectXDeviceManager, IOdysseyDeviceService, IDirectXDeviceSettings
    {
        #region Fields

        private readonly Application application;
        private readonly IDirectXDeviceFactory deviceFactory;
        private bool beginDrawOk;
        private DisplayOrientation currentWindowOrientation;
        private bool depthBufferShaderResource;

        private bool deviceSettingsChanged;
        private DirectXDevice directXDevice;
        private float horizontalDpi;
        private bool isBackBufferToResize;
        private bool isChangingDevice;

        private bool isFullScreen;
        private bool isReallyFullScreen;

        private bool isStereo;

        private bool preferMultiSampling;

        private Format preferredBackBufferFormat;

        private int preferredBackBufferHeight;
        private int preferredBackBufferWidth;

        private DepthFormat preferredDepthStencilFormat;

        private int preferredFullScreenOutputIndex;
        private FeatureLevel preferredGraphicsProfile;
        private int preferredMultiSampleCount;
        private int resizedBackBufferHeight;
        private int resizedBackBufferWidth;

        private DisplayOrientation supportedOrientations;

        private bool synchronizeWithVerticalRetrace;
        private float verticalDpi;

        #endregion Fields

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectXDeviceManager" /> class.
        /// </summary>
        /// <param name="application">The Application.</param>
        /// <exception cref="System.ArgumentNullException">The Application instance cannot be null.</exception>
        public DirectXDeviceManager(Application application)
        {
            this.application = application;
            if (this.application == null)
            {
                throw new ArgumentNullException("application");
            }

            // Defines all default values
            SynchronizeWithVerticalRetrace = true;
            PreferredBackBufferFormat = Format.R8G8B8A8_UNorm;
            PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8;
            preferredBackBufferWidth = Global.DefaultBackBufferWidth;
            preferredBackBufferHeight = Global.DefaultBackBufferHeight;
            PreferMultiSampling = false;
            preferredMultiSampleCount = 1;
            PreferredGraphicsProfile = new[]
            {
#if WP8
    // By default on WP8, only run in 9.3 to make sure
    // that we are not going to use 11.1 features when
    // running from the debugger.
                    FeatureLevel.Level_9_3,
#elif DIRECTX11_1
                FeatureLevel.Level_11_1,
#endif
                FeatureLevel.Level_11_0,
                FeatureLevel.Level_10_1,
                FeatureLevel.Level_10_0,
                FeatureLevel.Level_9_3,
                FeatureLevel.Level_9_2,
                FeatureLevel.Level_9_1
            };
            // Register the services to the registry
            application.Services.AddService(typeof (IDirectXDeviceManager), this);
            application.Services.AddService(typeof (IDirectXDeviceService), this);
            application.Services.AddService(typeof (IOdysseyDeviceService), this);
            application.Services.AddService(typeof (IDirectXDeviceSettings), this);

            deviceFactory = (IDirectXDeviceFactory) application.Services.GetService(typeof (IDirectXDeviceFactory));
            if (deviceFactory == null)
            {
                throw new InvalidOperationException("IDirectXDeviceFactory is not registered as a service");
            }

            application.WindowCreated += ApplicationOnWindowCreated;
        }

        private void ApplicationOnWindowCreated(object sender, EventArgs eventArgs)
        {
            application.Window.ClientSizeChanged += Window_ClientSizeChanged;
            application.Window.OrientationChanged += Window_OrientationChanged;
        }

        #endregion Constructors and Destructors

        #region Public Events

        public event EventHandler<EventArgs> DeviceChangeBegin;

        public event EventHandler<EventArgs> DeviceChangeEnd;

        public event EventHandler<EventArgs> DeviceCreated;

        public event EventHandler<EventArgs> DeviceDisposing;

        public event EventHandler<EventArgs> DeviceLost;

        IDirect3DProvider IDirectXDeviceService.DirectXDevice
        {
            get { return directXDevice; }
        }

        public event EventHandler<InitializeDeviceSettingsEventArgs> PreparingDeviceSettings;

        #endregion Public Events

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether the DepthBuffer should be created with the ShaderResource flag. Default is false.
        /// </summary>
        public bool DepthBufferShaderResource
        {
            get { return depthBufferShaderResource; }
            set
            {
                if (depthBufferShaderResource != value)
                {
                    depthBufferShaderResource = value;
                    deviceSettingsChanged = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets the device creation flags that will be used to create the <see cref="DirectXDevice"/>
        /// </summary>
        /// <value>The device creation flags.</value>
        public DeviceCreationFlags DeviceCreationFlags { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [prefer multi sampling].
        /// </summary>
        /// <value><c>true</c> if [prefer multi sampling]; otherwise, <c>false</c>.</value>
        public bool PreferMultiSampling
        {
            get { return preferMultiSampling; }

            set
            {
                if (preferMultiSampling != value)
                {
                    preferMultiSampling = value;
                    deviceSettingsChanged = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets the preferred depth stencil format.
        /// </summary>
        /// <value>The preferred depth stencil format.</value>
        public DepthFormat PreferredDepthStencilFormat
        {
            get { return preferredDepthStencilFormat; }

            set
            {
                if (preferredDepthStencilFormat != value)
                {
                    preferredDepthStencilFormat = value;
                    deviceSettingsChanged = true;
                }
            }
        }

        /// <summary>
        /// The output (monitor) index to use when switching to fullscreen mode. Doesn't have any effect when windowed mode is used.
        /// </summary>
        public int PreferredFullScreenOutputIndex
        {
            get { return preferredFullScreenOutputIndex; }

            set
            {
                if (preferredFullScreenOutputIndex != value)
                {
                    preferredFullScreenOutputIndex = value;
                    deviceSettingsChanged = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets the supported orientations.
        /// </summary>
        /// <value>The supported orientations.</value>
        public DisplayOrientation SupportedOrientations
        {
            get { return supportedOrientations; }

            set
            {
                if (supportedOrientations != value)
                {
                    supportedOrientations = value;
                    deviceSettingsChanged = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [synchronize with vertical retrace].
        /// </summary>
        /// <value><c>true</c> if [synchronize with vertical retrace]; otherwise, <c>false</c>.</value>
        public bool SynchronizeWithVerticalRetrace
        {
            get { return synchronizeWithVerticalRetrace; }
            set
            {
                if (synchronizeWithVerticalRetrace != value)
                {
                    synchronizeWithVerticalRetrace = value;
                    deviceSettingsChanged = true;
                }
            }
        }

        public float HorizontalDpi
        {
            get { return horizontalDpi; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is full screen.
        /// </summary>
        /// <value><c>true</c> if this instance is full screen; otherwise, <c>false</c>.</value>
        public bool IsFullScreen
        {
            get { return isFullScreen; }

            set
            {
                if (isFullScreen != value)
                {
                    isFullScreen = value;
                    deviceSettingsChanged = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is using stereoscopy.
        /// </summary>
        /// <value><c>true</c> if this instance is using stereoscopy; otherwise, <c>false</c>.</value>
        public bool IsStereo
        {
            get { return isStereo; }

            set
            {
                if (isStereo != value)
                {
                    isStereo = value;
                    deviceSettingsChanged = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets the preferred back buffer format.
        /// </summary>
        /// <value>The preferred back buffer format.</value>
        public Format PreferredBackBufferFormat
        {
            get { return preferredBackBufferFormat; }

            set
            {
                if (preferredBackBufferFormat != value)
                {
                    preferredBackBufferFormat = value;
                    deviceSettingsChanged = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets the height of the preferred back buffer.
        /// </summary>
        /// <value>The height of the preferred back buffer.</value>
        public int PreferredBackBufferHeight
        {
            get { return preferredBackBufferHeight; }

            set
            {
                if (preferredBackBufferHeight != value)
                {
                    preferredBackBufferHeight = value;
                    isBackBufferToResize = false;
                    deviceSettingsChanged = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets the width of the preferred back buffer.
        /// </summary>
        /// <value>The width of the preferred back buffer.</value>
        public int PreferredBackBufferWidth
        {
            get { return preferredBackBufferWidth; }

            set
            {
                if (preferredBackBufferWidth != value)
                {
                    preferredBackBufferWidth = value;
                    isBackBufferToResize = false;
                    deviceSettingsChanged = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets the list of graphics profile to select from the best feature to the lower feature. See remarks.
        /// </summary>
        /// <value>The graphics profile.</value>
        /// <remarks>
        /// By default, the PreferredGraphicsProfile is set to { <see cref="FeatureLevel.Level_11_1"/>,
        /// <see cref="FeatureLevel.Level_11_0"/>,
        /// <see cref="FeatureLevel.Level_10_1"/>,
        /// <see cref="FeatureLevel.Level_10_0"/>,
        /// <see cref="FeatureLevel.Level_9_3"/>,
        /// <see cref="FeatureLevel.Level_9_2"/>,
        /// <see cref="FeatureLevel.Level_9_1"/>}
        /// </remarks>
        public FeatureLevel[] PreferredGraphicsProfile { get; set; }

        public int PreferredMultiSampleCount
        {
            get { return preferredMultiSampleCount; }
            set
            {
                if (preferredMultiSampleCount != value)
                {
                    preferredMultiSampleCount = value;
                    deviceSettingsChanged = true;
                }
            }
        }

        public float VerticalDpi
        {
            get { return verticalDpi; }
        }

        public DirectXDevice DirectXDevice
        {
            get { return directXDevice; }
        }

        /// <summary>
        /// Sets the preferred graphics profile.
        /// </summary>
        /// <param name="levels">The levels.</param>
        /// <seealso cref="PreferredGraphicsProfile"/>
        public void SetPreferredGraphicsProfile(params FeatureLevel[] levels)
        {
            PreferredGraphicsProfile = levels;
        }

        #endregion Public Properties

        #region Public Methods and Operators

        bool IDirectXDeviceManager.BeginDraw()
        {
            beginDrawOk = false;
            if (DirectXDevice == null)
            {
                return false;
            }

            switch (DirectXDevice.DeviceStatus)
            {
                case DeviceStatus.Normal:
                    // Before drawing, we should clear the state to make sure that there is no unstable graphics device states (On some WP8 devices for example)
                    // An application should not rely on previous state (last frame...etc.) after BeginDraw.
                    DirectXDevice.ClearState();

                    // By default, we setup the render target to the back buffer, and the viewport as well.
                    if (DirectXDevice.RenderTarget != null)
                    {
                        DirectXDevice.SetRenderTargets(DirectXDevice.DepthStencilBuffer, DirectXDevice.RenderTarget);
                        DirectXDevice.SetViewport(0, 0, DirectXDevice.Presenter.BackBuffer.Description.Width,
                            DirectXDevice.Presenter.BackBuffer.Description.Height);
                    }

                    break;

                default:
                    SharpDX.Utilities.Sleep(TimeSpan.FromMilliseconds(20));
                    try
                    {
                        OnDeviceLost(this, EventArgs.Empty);
                        ChangeOrCreateDevice(true);
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                    break;
            }

            beginDrawOk = true;
            return true;
        }

        void IDirectXDeviceManager.CreateDevice()
        {
            // Force the creation of the device
            ChangeOrCreateDevice(true);
        }

        void IDirectXDeviceManager.EndDraw()
        {
            if (beginDrawOk && DirectXDevice != null)
            {
                try
                {
                    DirectXDevice.Present();
                }
                catch (SharpDXException ex)
                {
                    // If this is not a DeviceRemoved or DeviceReset, than throw an exception
                    if (ex.ResultCode != ResultCode.DeviceRemoved && ex.ResultCode != ResultCode.DeviceReset)
                    {
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Applies the changes from this instance and change or create the <see cref="DirectXDevice"/> according to the new values.
        /// </summary>
        public void ApplyChanges()
        {
            if (DirectXDevice == null || deviceSettingsChanged)
            {
                ChangeOrCreateDevice(false);
            }
        }

        #endregion Public Methods and Operators

        protected static DisplayOrientation SelectOrientation(DisplayOrientation orientation, int width, int height,
            bool allowLandscapeLeftAndRight)
        {
            if (orientation != DisplayOrientation.Default)
            {
                return orientation;
            }

            if (width <= height)
            {
                return DisplayOrientation.Portrait;
            }

            if (allowLandscapeLeftAndRight)
            {
                return DisplayOrientation.LandscapeRight | DisplayOrientation.LandscapeLeft;
            }

            return DisplayOrientation.LandscapeLeft;
        }

        /// <summary>
        /// Determines whether this instance is compatible with the the specified new <see cref="DeviceInformation"/>.
        /// </summary>
        /// <param name="newDeviceInfo">The new device info.</param>
        /// <returns><c>true</c> if this instance this instance is compatible with the the specified new <see cref="DeviceInformation"/>; otherwise, <c>false</c>.</returns>
        protected virtual bool CanResetDevice(DeviceInformation newDeviceInfo)
        {
            // By default, a reset is compatible when we stay under the same graphics profile.
            return DirectXDevice.Features.Level == newDeviceInfo.GraphicsProfile;
        }

        protected override void Dispose(bool disposeManagedResources)
        {
            if (disposeManagedResources)
            {
                if (application != null)
                {
                    if (application.Services.GetService(typeof (IOdysseyDeviceService)) == this)
                    {
                        application.Services.RemoveService(typeof (IOdysseyDeviceService));
                    }

                    application.Window.ClientSizeChanged -= Window_ClientSizeChanged;
                    application.Window.OrientationChanged -= Window_OrientationChanged;
                }

                SharpDX.Utilities.Dispose(ref directXDevice);
            }

            base.Dispose(disposeManagedResources);
        }

        /// <summary>
        /// Finds the best device that is compatible with the preferences defined in this instance.
        /// </summary>
        /// <param name="anySuitableDevice">if set to <c>true</c> a device can be selected from any existing adapters, otherwise, it will select only from default adapter.</param>
        /// <returns>The graphics device information.</returns>
        protected virtual DeviceInformation FindBestDevice(bool anySuitableDevice)
        {
            // Setup preferred parameters before passing them to the factory
            var preferredParameters = new ApplicationGraphicsParameters
            {
                PreferredBackBufferWidth = PreferredBackBufferWidth,
                PreferredBackBufferHeight = PreferredBackBufferHeight,
                PreferredBackBufferFormat = PreferredBackBufferFormat,
                PreferredDepthStencilFormat = PreferredDepthStencilFormat,
                IsFullScreen = IsFullScreen,
                IsStereo = IsStereo,
                PreferredFullScreenOutputIndex = PreferredFullScreenOutputIndex,
                DepthBufferShaderResource = DepthBufferShaderResource,
                PreferMultiSampling = PreferMultiSampling,
                SynchronizeWithVerticalRetrace = SynchronizeWithVerticalRetrace,
                PreferredGraphicsProfile = (FeatureLevel[]) PreferredGraphicsProfile.Clone(),
                PreferredMultiSampleCount = PreferMultiSampling ? PreferredMultiSampleCount : 1,
            };

            // Setup resized value if there is a resize pending
            if (!IsFullScreen && isBackBufferToResize)
            {
                preferredParameters.PreferredBackBufferWidth = resizedBackBufferWidth;
                preferredParameters.PreferredBackBufferHeight = resizedBackBufferHeight;
            }

            var devices = deviceFactory.FindBestDevices(preferredParameters);
            if (devices.Count == 0)
            {
                throw new InvalidOperationException("No screen modes found");
            }

            RankDevices(devices);

            if (devices.Count == 0)
            {
                throw new InvalidOperationException("No screen modes found after ranking");
            }

            return devices[0];
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

        protected virtual void OnInitializeDeviceSettings(object sender, InitializeDeviceSettingsEventArgs args)
        {
            RaiseEvent(PreparingDeviceSettings, sender, args);
        }

        /// <summary>
        /// Ranks a list of <see cref="DeviceInformation"/> before creating a new device.
        /// </summary>
        /// <param name="foundDevices">The list of devices that can be reorder.</param>
        protected virtual void RankDevices(List<DeviceInformation> foundDevices)
        {
            // Don't sort if there is a single device (mostly for XAML/WP8)
            if (foundDevices.Count == 1)
            {
                return;
            }

            foundDevices.Sort((left, right) =>
            {
                var leftParams = left.PresentationParameters;
                var rightParams = right.PresentationParameters;

                var leftAdapter = left.Adapter;
                var rightAdapter = right.Adapter;

                // Sort by GraphicsProfile
                if (left.GraphicsProfile != right.GraphicsProfile)
                {
                    return left.GraphicsProfile <= right.GraphicsProfile ? 1 : -1;
                }

                // Sort by FullScreen mode
                if (leftParams.IsFullScreen != rightParams.IsFullScreen)
                {
                    return IsFullScreen != leftParams.IsFullScreen ? 1 : -1;
                }

                // Sort by BackBufferFormat
                int leftFormat = CalculateRankForFormat(leftParams.BackBufferFormat);
                int rightFormat = CalculateRankForFormat(rightParams.BackBufferFormat);
                if (leftFormat != rightFormat)
                {
                    return leftFormat >= rightFormat ? 1 : -1;
                }

                // Sort by MultiSampleCount
                if (leftParams.MultiSampleCount != rightParams.MultiSampleCount)
                {
                    return leftParams.MultiSampleCount <= rightParams.MultiSampleCount ? 1 : -1;
                }

                // Sort by AspectRatio
                var targetAspectRatio = (PreferredBackBufferWidth == 0) || (PreferredBackBufferHeight == 0)
                    ? (float) Global.DefaultBackBufferWidth/Global.DefaultBackBufferHeight
                    : (float) PreferredBackBufferWidth/PreferredBackBufferHeight;
                var leftDiffRatio =
                    Math.Abs(((float) leftParams.BackBufferWidth/leftParams.BackBufferHeight) - targetAspectRatio);
                var rightDiffRatio =
                    Math.Abs(((float) rightParams.BackBufferWidth/rightParams.BackBufferHeight) - targetAspectRatio);
                if (Math.Abs(leftDiffRatio - rightDiffRatio) > 0.2f)
                {
                    return leftDiffRatio >= rightDiffRatio ? 1 : -1;
                }

                // Sort by PixelCount
                int leftPixelCount;
                int rightPixelCount;
                if (IsFullScreen)
                {
                    if ((PreferredBackBufferWidth == 0) || (PreferredBackBufferHeight == 0))
                    {
                        // assume we got here only adapters that have the needed number of outputs:
                        var leftOutput = leftAdapter.GetOutputAt(PreferredFullScreenOutputIndex);
                        var rightOutput = rightAdapter.GetOutputAt(PreferredFullScreenOutputIndex);

                        leftPixelCount = leftOutput.CurrentDisplayMode.Width*leftOutput.CurrentDisplayMode.Height;
                        rightPixelCount = rightOutput.CurrentDisplayMode.Width*rightOutput.CurrentDisplayMode.Height;
                    }
                    else
                    {
                        leftPixelCount = rightPixelCount = PreferredBackBufferWidth*PreferredBackBufferHeight;
                    }
                }
                else if ((PreferredBackBufferWidth == 0) || (PreferredBackBufferHeight == 0))
                {
                    leftPixelCount = rightPixelCount = Global.DefaultBackBufferWidth*Global.DefaultBackBufferHeight;
                }
                else
                {
                    leftPixelCount = rightPixelCount = PreferredBackBufferWidth*PreferredBackBufferHeight;
                }

                int leftDeltaPixelCount = Math.Abs((leftParams.BackBufferWidth*leftParams.BackBufferHeight) - leftPixelCount);
                int rightDeltaPixelCount =
                    Math.Abs((rightParams.BackBufferWidth*rightParams.BackBufferHeight) - rightPixelCount);
                if (leftDeltaPixelCount != rightDeltaPixelCount)
                {
                    return leftDeltaPixelCount >= rightDeltaPixelCount ? 1 : -1;
                }

                // Sort by default Adapter, default adapter first
                if (left.Adapter != right.Adapter)
                {
                    if (left.Adapter.IsDefaultAdapter)
                    {
                        return -1;
                    }

                    if (right.Adapter.IsDefaultAdapter)
                    {
                        return 1;
                    }
                }

                return 0;
            });
        }

        private int CalculateFormatSize(Format format)
        {
            switch (format)
            {
                case Format.R8G8B8A8_UNorm:
                case Format.R8G8B8A8_UNorm_SRgb:
                case Format.B8G8R8A8_UNorm:
                case Format.B8G8R8A8_UNorm_SRgb:
                case Format.R10G10B10A2_UNorm:
                    return 32;

                case Format.B5G6R5_UNorm:
                case Format.B5G5R5A1_UNorm:
                    return 16;
            }

            return 0;
        }

        private int CalculateRankForFormat(Format format)
        {
            if (format == PreferredBackBufferFormat)
            {
                return 0;
            }

            if (CalculateFormatSize(format) == CalculateFormatSize(PreferredBackBufferFormat))
            {
                return 1;
            }

            return int.MaxValue;
        }

        private void ChangeOrCreateDevice(bool forceCreate)
        {
            if (forceCreate)
            {
                // Make sure that all GraphicsAdapter are cleared and removed when device is disposed.
                GraphicsAdapter.Dispose();

                // Make sure that GraphicsAdapter are initialized.
                GraphicsAdapter.Initialize();
            }

            isChangingDevice = true;
            int width = application.Window.ClientBounds.Width;
            int height = application.Window.ClientBounds.Height;

            OnDeviceChangeBegin(this, EventArgs.Empty);

            bool isBeginScreenDeviceChange = false;
            try
            {
                // Notifies the application window for the new orientation
                application.Window.SetSupportedOrientations(SelectOrientation(supportedOrientations, PreferredBackBufferWidth,
                    PreferredBackBufferHeight, true));

                var deviceInformation = FindBestDevice(forceCreate);
                application.Window.BeginScreenDeviceChange(deviceInformation.PresentationParameters.IsFullScreen);
                isBeginScreenDeviceChange = true;
                bool needToCreateNewDevice = true;

                // If we are not forced to create a new device and this is already an existing Device
                // try to reset and resize it.
                if (!forceCreate && DirectXDevice != null)
                {
                    OnInitializeDeviceSettings(this, new InitializeDeviceSettingsEventArgs(deviceInformation));
                    if (CanResetDevice(deviceInformation))
                    {
                        try
                        {
                            var newWidth = deviceInformation.PresentationParameters.BackBufferWidth;
                            var newHeight = deviceInformation.PresentationParameters.BackBufferHeight;
                            var newFormat = deviceInformation.PresentationParameters.BackBufferFormat;
                            var newOutputIndex = deviceInformation.PresentationParameters.PreferredFullScreenOutputIndex;

                            DirectXDevice.Presenter.PreferredFullScreenOutputIndex = newOutputIndex;
                            DirectXDevice.Presenter.Resize(newWidth, newHeight, newFormat);

                            // Change full screen if needed
                            DirectXDevice.Presenter.IsFullScreen = deviceInformation.PresentationParameters.IsFullScreen;

                            needToCreateNewDevice = false;
                        }
                        catch (Exception ex)
                        {
                            LogEvent.Engine.Error(ex);
                        }
                    }
                }

                // If we still need to create a device, then we need to create it
                if (needToCreateNewDevice)
                {
                    CreateDevice(deviceInformation);
                }

                var presentationParameters = DirectXDevice.Presenter.Description;
                isReallyFullScreen = presentationParameters.IsFullScreen;
                if (presentationParameters.BackBufferWidth != 0)
                {
                    width = presentationParameters.BackBufferWidth;
                }

                if (presentationParameters.BackBufferHeight != 0)
                {
                    height = presentationParameters.BackBufferHeight;
                }

                horizontalDpi = application.Context.DpiX;
                verticalDpi = application.Context.DpiY;

                deviceSettingsChanged = false;

                OnDeviceChangeEnd(this, EventArgs.Empty);
            }
            finally
            {
                if (isBeginScreenDeviceChange)
                {
                    application.Window.EndScreenDeviceChange(width, height);
                }

                currentWindowOrientation = application.Window.CurrentOrientation;
                isChangingDevice = false;
            }
        }

        private void CreateDevice(DeviceInformation newInfo)
        {
            SharpDX.Utilities.Dispose(ref directXDevice);

            newInfo.PresentationParameters.IsFullScreen = isFullScreen;
            newInfo.PresentationParameters.PresentationInterval = SynchronizeWithVerticalRetrace
                ? PresentInterval.One
                : PresentInterval.Immediate;
            newInfo.DeviceCreationFlags = DeviceCreationFlags;

            OnInitializeDeviceSettings(this, new InitializeDeviceSettingsEventArgs(newInfo));

            // this.ValidateGraphicsDeviceInformation(newInfo);
            directXDevice = deviceFactory.CreateDevice(newInfo);
            application.Services.AddService(typeof (DirectXDevice), DirectXDevice);

            DirectXDevice.Disposing += GraphicsDevice_Disposing;

            OnDeviceCreated(this, EventArgs.Empty);
        }

        private void GraphicsDevice_Disposing(object sender, EventArgs e)
        {
            // Clears the Device
            directXDevice = null;

            OnDeviceDisposing(sender, e);
        }

        private void RaiseEvent<T>(EventHandler<T> handler, object sender, T args)
            where T : EventArgs
        {
            if (handler != null)
                handler(sender, args);
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            if (!isChangingDevice &&
                ((application.Window.ClientBounds.Height != 0) || (application.Window.ClientBounds.Width != 0)))
            {
                resizedBackBufferWidth = application.Window.ClientBounds.Width;
                resizedBackBufferHeight = application.Window.ClientBounds.Height;
                isBackBufferToResize = true;
                ChangeOrCreateDevice(false);
            }
        }

        private void Window_OrientationChanged(object sender, EventArgs e)
        {
            if ((!isChangingDevice &&
                 ((application.Window.ClientBounds.Height != 0) || (application.Window.ClientBounds.Width != 0))) &&
                (application.Window.CurrentOrientation != currentWindowOrientation))
            {
                ChangeOrCreateDevice(false);
            }
        }
    }
}
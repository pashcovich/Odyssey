#region Using Directives

using Odyssey.Graphics;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;

#endregion Using Directives

namespace Odyssey.Engine
{
    internal abstract class ApplicationPlatform : DisposeBase, IDirectXDeviceFactory
    {
        private readonly Application application;
        private readonly IServiceRegistry services;

        protected ApplicationPlatform(Application application)
        {
            this.application = application;
            services = application.Services;
            Services.AddService(typeof(IDirectXDeviceFactory), this);
        }

        #region Events

        public event EventHandler<EventArgs> Activated;

        public event EventHandler<EventArgs> Deactivated;

        public event EventHandler<EventArgs> Exiting;

        public event EventHandler<EventArgs> Idle;

        public event EventHandler<EventArgs> Resume;

        public event EventHandler<EventArgs> Suspend;

        public event EventHandler<EventArgs> WindowCreated;

        protected void OnActivated(object source, EventArgs e)
        {
            EventHandler<EventArgs> handler = Activated;
            if (handler != null) handler(this, e);
        }

        protected void OnDeactivated(object source, EventArgs e)
        {
            EventHandler<EventArgs> handler = Deactivated;
            if (handler != null) handler(this, e);
        }

        protected void OnExiting(object source, EventArgs e)
        {
            EventHandler<EventArgs> handler = Exiting;
            if (handler != null) handler(this, e);
        }

        protected void OnIdle(object source, EventArgs e)
        {
            EventHandler<EventArgs> handler = Idle;
            if (handler != null) handler(this, e);
        }

        protected void OnInitialize(object sender, EventArgs e)
        {
            application.InitializeBeforeRun();
        }

        protected void OnResume(object source, EventArgs e)
        {
            EventHandler<EventArgs> handler = Resume;
            if (handler != null) handler(this, e);
        }

        protected void OnSuspend(object source, EventArgs e)
        {
            EventHandler<EventArgs> handler = Suspend;
            if (handler != null) handler(this, e);
        }

        #endregion Events

        protected delegate void AddDeviceToListDelegate(ApplicationGraphicsParameters prefferedParameters,
            GraphicsAdapter graphicsAdapter,
            DeviceInformation deviceInfo,
            List<DeviceInformation> graphicsDeviceInfos);

        public abstract string DefaultAppDirectory { get; }

        public bool IsBlockingRun
        {
            get { return ApplicationWindow != null && ApplicationWindow.IsBlockingRun; }
        }

        public ApplicationWindow MainWindow
        {
            get { return ApplicationWindow; }
        }

        public object WindowContext { get; set; }

        protected Application Application
        {
            get { return application; }
        }

        protected ApplicationWindow ApplicationWindow { get; private set; }

        protected IServiceRegistry Services
        {
            get { return services; }
        }

        public virtual DirectXDevice CreateDevice(DeviceInformation deviceInformation)
        {
            var device = DirectXDevice.New(deviceInformation.Adapter, deviceInformation.DeviceCreationFlags,
                deviceInformation.GraphicsProfile);

            var parameters = deviceInformation.PresentationParameters;

            CreatePresenter(device, parameters);

            // Force to resize the applicationWindow
            ApplicationWindow.Resize(parameters.BackBufferWidth, parameters.BackBufferHeight);

            return device;
        }

        public virtual ApplicationWindow CreateWindow(ApplicationContext applicationContext)
        {
            var windows = GetSupportedApplicationWindows();

            foreach (var applicationWindowToTest in windows)
            {
                if (applicationWindowToTest.CanHandle(applicationContext))
                {
                    applicationWindowToTest.Services = Services;
                    applicationWindowToTest.Initialize(applicationContext);
                    return applicationWindowToTest;
                }
            }

            throw new ArgumentException("Application Window context not supported on this platform");
        }

        public virtual void Exit()
        {
            ApplicationWindow.Exiting = true;
            Activated = null;
            Deactivated = null;
            Exiting = null;
            Idle = null;
            Resume = null;
            Suspend = null;
        }

        public virtual List<DeviceInformation> FindBestDevices(ApplicationGraphicsParameters preferredParameters)
        {
            var graphicsDeviceInfos = new List<DeviceInformation>();

            // Iterate on each adapter
            foreach (var graphicsAdapter in GraphicsAdapter.Adapters)
            {
                TryFindSupportedFeatureLevel(preferredParameters, graphicsAdapter, graphicsDeviceInfos, TryAddDeviceWithDisplayMode);
            }

            return graphicsDeviceInfos;
        }

        public void Run(ApplicationContext applicationContext)
        {
            ApplicationWindow = CreateWindow(applicationContext);

            // Register on Activated
            ApplicationWindow.Activated += OnActivated;
            ApplicationWindow.Deactivated += OnDeactivated;
            ApplicationWindow.Initialization += OnInitialize;
            ApplicationWindow.Tick += (s, e) => application.Tick();
            ApplicationWindow.Shutdown += OnExiting;

            var windowCreated = WindowCreated;
            if (windowCreated != null)
            {
                windowCreated(this, EventArgs.Empty);
            }

            ApplicationWindow.Run();
        }

        protected void AddDevice(DisplayMode mode, DeviceInformation deviceBaseInfo,
            ApplicationGraphicsParameters preferredParameters, List<DeviceInformation> graphicsDeviceInfos)
        {
            var deviceInfo = deviceBaseInfo.Clone();

            deviceInfo.PresentationParameters.RefreshRate = mode.RefreshRate;
            deviceInfo.PresentationParameters.PreferredFullScreenOutputIndex = preferredParameters.PreferredFullScreenOutputIndex;
            deviceBaseInfo.PresentationParameters.DepthBufferShaderResource = preferredParameters.DepthBufferShaderResource;

            if (preferredParameters.IsFullScreen)
            {
                deviceInfo.PresentationParameters.BackBufferFormat = mode.Format;
                deviceInfo.PresentationParameters.BackBufferWidth = mode.Width;
                deviceInfo.PresentationParameters.BackBufferHeight = mode.Height;
            }
            else
            {
                deviceInfo.PresentationParameters.BackBufferFormat = preferredParameters.PreferredBackBufferFormat;
                deviceInfo.PresentationParameters.BackBufferWidth = preferredParameters.PreferredBackBufferWidth;
                deviceInfo.PresentationParameters.BackBufferHeight = preferredParameters.PreferredBackBufferHeight;
            }

            // TODO: Handle multisampling / depthstencil format
            deviceInfo.PresentationParameters.DepthStencilFormat = preferredParameters.PreferredDepthStencilFormat;
            deviceInfo.PresentationParameters.MultiSampleCount = MSAALevelFromCount(preferredParameters.PreferredMultiSampleCount);

            if (!graphicsDeviceInfos.Contains(deviceInfo))
            {
                graphicsDeviceInfos.Add(deviceInfo);
            }
        }

        protected virtual void CreatePresenter(DirectXDevice device, PresentationParameters parameters, object newControl = null)
        {
            if (device == null) throw new ArgumentNullException("device");

            parameters = TryGetParameters(device, parameters);

            DisposeGraphicsPresenter(device);

            // override the device window, if available
            if (newControl != null)
                parameters.DeviceWindowHandle = newControl;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (ApplicationWindow != null)
                {
                    ApplicationWindow.Dispose();
                    ApplicationWindow = null;
                }
            }
        }

        protected void DisposeGraphicsPresenter(DirectXDevice device)
        {
            var oldPresenter = device.Presenter;
            if (oldPresenter != null)
            {
                device.Presenter = null;
                oldPresenter.Dispose();
            }
        }

        protected abstract IEnumerable<ApplicationWindow> GetSupportedApplicationWindows();

        protected void TryFindSupportedFeatureLevel(ApplicationGraphicsParameters preferredParameters,
            GraphicsAdapter graphicsAdapter,
            List<DeviceInformation> graphicsDeviceInfos,
            AddDeviceToListDelegate addDelegate)
        {
            // Check if the adapter has an output with the preffered index
            if (preferredParameters.IsFullScreen &&
                graphicsAdapter.OutputsCount <= preferredParameters.PreferredFullScreenOutputIndex)
                return;

            // Iterate on each preferred graphics profile
            foreach (var featureLevel in preferredParameters.PreferredGraphicsProfile)
            {
                // Check if this profile is supported.
                if (graphicsAdapter.IsProfileSupported(featureLevel))
                {
                    var deviceInfo = CreateDeviceInformation(preferredParameters, graphicsAdapter, featureLevel);

                    addDelegate(preferredParameters, graphicsAdapter, deviceInfo, graphicsDeviceInfos);

                    // If the profile is supported, we are just using the first best one
                    break;
                }
            }
        }

        protected PresentationParameters TryGetParameters(DirectXDevice device, PresentationParameters parameters)
        {
            var oldPresenter = device.Presenter;

            if (parameters == null)
            {
                if (oldPresenter == null)
                    throw new InvalidOperationException("Cannot retrieve PresentationParameters.");

                // preserve the parameters from old GraphicsPresenter
                parameters = oldPresenter.Description.Clone();
            }

            return parameters;
        }

        private static MSAALevel MSAALevelFromCount(int multiSampleCount)
        {
            switch (multiSampleCount)
            {
                case 1:
                    return MSAALevel.None;

                case 2:
                    return MSAALevel.X2;

                case 4:
                    return MSAALevel.X4;

                case 8:
                    return MSAALevel.X8;

                default:
                    throw new ArgumentOutOfRangeException(string.Format("MultiSampleCount of {0} is not supported", multiSampleCount));
            }
        }

        private DeviceInformation CreateDeviceInformation(ApplicationGraphicsParameters preferredParameters,
            GraphicsAdapter graphicsAdapter,
            FeatureLevel featureLevel)
        {
            return new DeviceInformation
            {
                Adapter = graphicsAdapter,
                GraphicsProfile = featureLevel,
                PresentationParameters =
                {
                    MultiSampleCount = (MSAALevel)preferredParameters.PreferredMultiSampleCount,
                    IsFullScreen = preferredParameters.IsFullScreen,
                    PreferredFullScreenOutputIndex = preferredParameters.PreferredFullScreenOutputIndex,
                    DepthBufferShaderResource = preferredParameters.DepthBufferShaderResource,
                    PresentationInterval =
                        preferredParameters.SynchronizeWithVerticalRetrace ? PresentInterval.One : PresentInterval.Immediate,
                    DeviceWindowHandle = MainWindow.NativeWindow,
                    RenderTargetUsage = Usage.BackBuffer | Usage.RenderTargetOutput
                }
            };
        }

        private void TryAddDeviceFromOutput(ApplicationGraphicsParameters prefferedParameters,
            GraphicsOutput output,
            DeviceInformation deviceInfo,
            List<DeviceInformation> graphicsDeviceInfos)
        {
            if (output.CurrentDisplayMode != null)
                AddDevice(output.CurrentDisplayMode, deviceInfo, prefferedParameters, graphicsDeviceInfos);

            if (prefferedParameters.IsFullScreen)
            {
                // Get display mode for the particular width, height, pixelformat
                foreach (var displayMode in output.SupportedDisplayModes)
                    AddDevice(displayMode, deviceInfo, prefferedParameters, graphicsDeviceInfos);
            }
        }

        private void TryAddDeviceWithDisplayMode(ApplicationGraphicsParameters prefferedParameters,
            GraphicsAdapter graphicsAdapter,
            DeviceInformation deviceInfo,
            List<DeviceInformation> graphicsDeviceInfos)
        {
            // if we want to switch to fullscreen, try to find only needed output, otherwise check them all
            if (prefferedParameters.IsFullScreen)
            {
                if (prefferedParameters.PreferredFullScreenOutputIndex < graphicsAdapter.OutputsCount)
                {
                    var output = graphicsAdapter.GetOutputAt(prefferedParameters.PreferredFullScreenOutputIndex);
                    TryAddDeviceFromOutput(prefferedParameters, output, deviceInfo, graphicsDeviceInfos);
                }
            }
            else
            {
                for (var i = 0; i < graphicsAdapter.OutputsCount; i++)
                {
                    var output = graphicsAdapter.GetOutputAt(i);
                    TryAddDeviceFromOutput(prefferedParameters, output, deviceInfo, graphicsDeviceInfos);
                }
            }
        }
    }
}
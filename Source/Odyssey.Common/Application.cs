using Odyssey.Content;
using Odyssey.Engine;
using SharpDX;
using System;

namespace Odyssey
{
    public abstract class Application : Component
    {
        private readonly ApplicationPlatform applicationPlatform;
        private readonly IServiceRegistry services;
        private readonly IAssetProvider contentManager;
        IDirectXDeviceService deviceService;
        IDirectXDeviceManager deviceManager;
        ApplicationContext applicationContext;
        private readonly TimerTick timer;
        private bool isEndRunRequired;
        private bool isExiting;

        internal IApplicationPlatform ApplicationPlatform { get { return applicationPlatform; } }

        public bool IsActive { get; private set; }

        /// <summary>
        /// Gets a value indicating whether is running.
        /// </summary>
        public bool IsRunning { get; private set; }
        
        public IServiceRegistry Services { get {return services; } }
        public static IDirectXProvider DirectX { get; private set; }
        public static IDirectXTarget Target { get; private set; }
        public IAssetProvider Content { get { return contentManager; } }
        public static FpsTimer Timer { get; private set; }

        public DirectXDevice Device
        {
            get
            {
                if (deviceService == null)
                {
                    throw new InvalidOperationException("GraphicsDeviceService is not yet initialized");
                }

                return deviceService.DirectXDevice;
            }
        }

        /// <summary>
        /// Gets the abstract window.
        /// </summary>
        /// <value>The window.</value>
        internal ApplicationWindow Window
        {
            get
            {
                if (applicationPlatform != null)
                {
                    return applicationPlatform.MainWindow;
                }
                return null;
            }
        }

        internal Application(ApplicationPlatform platform, IAssetProvider contentManager)
        {
            services = new ServiceRegistry();
            this.contentManager = contentManager;
            timer = new TimerTick();
            Timer = new FpsTimer();


            // Create Platform
            applicationPlatform = platform;
            applicationPlatform.Activated += ApplicationPlatformActivated;
            applicationPlatform.Deactivated += ApplicationPlatformDeactivated;
            applicationPlatform.Exiting += ApplicationPlatform_Exiting;
            applicationPlatform.WindowCreated += ApplicationPlatformWindowCreated;

            contentManager.InitializeApplication(this);
            applicationPlatform.InitializeApplication(this);
        }


        internal void InitializeBeforeRun()
        {
            // Make sure that the device is already created
            deviceManager.CreateDevice();
            

            // Gets the graphics device service
            deviceService = Services.GetService(typeof(IDirectXDeviceService)) as IDirectXDeviceService;
            if (deviceService == null)
            {
                throw new InvalidOperationException("No deviceService found");
            }

            // Checks the graphics device
            if (deviceService.DirectXDevice == null)
            {
                throw new InvalidOperationException("No GraphicsDevice found");
            }

            // Initialize this instance and all game systems
            Initialize();

            IsRunning = true;

            //BeginRun();

            timer.Reset();
            //gameTime.Update(totalGameTime, TimeSpan.Zero, false);
            //gameTime.FrameCount = 0;

            // Run the first time an update
            //Update(gameTime);

            //isFirstUpdateDone = true;
        }

        protected virtual void Initialize()
        {
            SetupGraphicsDeviceEvents();
        }

        /// <summary>
        /// Call this method to initialize the game, begin running the game loop, and start processing events for the game.
        /// </summary>
        /// <param name="gameContext">The window Context for this game.</param>
        /// <exception cref="System.InvalidOperationException">Cannot run this instance while it is already running</exception>
        public void Run(ApplicationContext applicationContext = null)
        {
            if (IsRunning)
            {
                throw new InvalidOperationException("Cannot run this instance while it is already running");
            }

            // Gets the graphics device manager
            deviceManager = Services.GetService(typeof(IDirectXDeviceManager)) as IDirectXDeviceManager;
            if (deviceManager == null)
            {
                throw new InvalidOperationException("No DeviceManager found");
            }

            if (deviceManager == null)
            {
                throw new InvalidOperationException("No DeviceManager found");
            }

            // Gets the GameWindow Context
            this.applicationContext = applicationContext;

            try
            {
                applicationPlatform.Run(applicationContext);

                if (applicationPlatform.IsBlockingRun)
                {
                    // If the previous call was blocking, then we can call Endrun
                    EndRun();
                }
                else
                {
                    // EndRun will be executed on Game.Exit
                    isEndRunRequired = true;
                }
            }
            finally
            {
                if (!isEndRunRequired)
                {
                    IsRunning = false;
                }
            }
        }

        private void SetupGraphicsDeviceEvents()
        {
            // Find the IGraphicsDeviceSerive.
            deviceService = Services.GetService(typeof(IDirectXDeviceService)) as IDirectXDeviceService;

            // If there is no graphics device service, don't go further as the whole Game would not work
            if (deviceService == null)
            {
                throw new InvalidOperationException("Unable to create find a IGraphicsDeviceService");
            }

            if (deviceService.DirectXDevice == null)
            {
                throw new InvalidOperationException("Unable to find a GraphicsDevice instance");
            }

            deviceService.DeviceCreated += deviceService_DeviceCreated;
            deviceService.DeviceDisposing += deviceService_DeviceDisposing;
        }

        protected override void Dispose(bool disposeManagedResources)
        {
            if (disposeManagedResources)
            {
                lock (this)
                {
                    //var array = new IGameSystem[GameSystems.Count];
                    //this.GameSystems.CopyTo(array, 0);
                    //for (int i = 0; i < array.Length; i++)
                    //{
                    //    var disposable = array[i] as IDisposable;
                    //    if (disposable != null)
                    //    {
                    //        disposable.Dispose();
                    //    }
                    //}

                    var disposableGraphicsManager = deviceManager as IDisposable;
                    if (disposableGraphicsManager != null)
                    {
                        disposableGraphicsManager.Dispose();
                    }

                    DisposeGraphicsDeviceEvents();

                    if (applicationPlatform != null)
                    {
                        applicationPlatform.Dispose();
                    }
                }
            }

            base.Dispose(disposeManagedResources);
        }

        private void DisposeGraphicsDeviceEvents()
        {
            if (deviceService != null)
            {
                deviceService.DeviceCreated -= deviceService_DeviceCreated;
                deviceService.DeviceDisposing -= deviceService_DeviceDisposing;
            }
        }

        private void deviceService_DeviceCreated(object sender, EventArgs e)
        {
            //LoadContent();
        }

        private void deviceService_DeviceDisposing(object sender, EventArgs e)
        {
            Content.Unload();
            ApplicationPlatform.MainWindow.Dispose();

            //if (contentLoaded)
            //{
            //    UnloadContent();
            //    contentLoaded = false;
            //}
        }

        /// <summary>
        /// Exits the game.
        /// </summary>
        public void Exit()
        {
            isExiting = true;
            applicationPlatform.Exit();
            if (IsRunning && isEndRunRequired)
            {
                EndRun();
                IsRunning = false;
            }
        }

        public virtual void Tick()
        { }

        #region Events

        /// <summary>
        /// Occurs when [activated].
        /// </summary>
        public event EventHandler<EventArgs> Activated;

        /// <summary>
        /// Occurs when [deactivated].
        /// </summary>
        public event EventHandler<EventArgs> Deactivated;

        /// <summary>
        /// Occurs when [exiting].
        /// </summary>
        public event EventHandler<EventArgs> Exiting;

        /// <summary>
        /// Occurs when [window created].
        /// </summary>
        public event EventHandler<EventArgs> WindowCreated;

        /// <summary>
        /// Raises the Activated event. Override this method to add code to handle when the game gains focus.
        /// </summary>
        /// <param name="sender">The Game.</param>
        /// <param name="args">Arguments for the Activated event.</param>
        protected virtual void OnActivated(object sender, EventArgs args)
        {
            var handler = Activated;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        /// <summary>
        /// Raises the Deactivated event. Override this method to add code to handle when the game loses focus.
        /// </summary>
        /// <param name="sender">The Game.</param>
        /// <param name="args">Arguments for the Deactivated event.</param>
        protected virtual void OnDeactivated(object sender, EventArgs args)
        {
            var handler = Deactivated;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        /// <summary>
        /// Raises an Exiting event. Override this method to add code to handle when the game is exiting.
        /// </summary>
        /// <param name="sender">The Game.</param>
        /// <param name="args">Arguments for the Exiting event.</param>
        protected virtual void OnExiting(object sender, EventArgs args)
        {
            var handler = Exiting;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        protected virtual void OnWindowCreated()
        {
            EventHandler<EventArgs> handler = WindowCreated;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
        #endregion

        #region ApplicationPlatform events
        private void ApplicationPlatformActivated(object sender, EventArgs e)
        {
            if (!IsActive)
            {
                IsActive = true;
                OnActivated(this, EventArgs.Empty);
            }
        }

        private void ApplicationPlatformDeactivated(object sender, EventArgs e)
        {
            if (IsActive)
            {
                IsActive = false;
                OnDeactivated(this, EventArgs.Empty);
            }
        }

        private void ApplicationPlatform_Exiting(object sender, EventArgs e)
        {
            Content.Unload();
            OnExiting(this, EventArgs.Empty);
        }

        private void ApplicationPlatformWindowCreated(object sender, EventArgs eventArgs)
        {
            OnWindowCreated();
        }
        #endregion


        /// <summary>
        /// Starts the drawing of a frame. This method is followed by calls to Draw and EndDraw.
        /// </summary>
        /// <returns><c>true</c> to continue drawing, false to not call <see cref="Draw"/> and <see cref="EndDraw"/></returns>
        protected virtual bool BeginDraw()
        {
            if ((deviceManager != null) && !deviceManager.BeginDraw())
            {
                return false;
            }

            return true;
        }

        /// <summary>Called after the game loop has stopped running before exiting.</summary>
        protected virtual void EndRun()
        { }

        protected virtual void EndDraw()
        {
            if (deviceManager != null)
            {
                deviceManager.EndDraw();
            }
        }
    }
}

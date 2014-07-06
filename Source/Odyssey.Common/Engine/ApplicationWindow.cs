#region Using Directives

using Odyssey.Graphics;
using SharpDX;
using System;
using System.Diagnostics.Contracts;

#endregion Using Directives

namespace Odyssey.Engine
{
    public abstract class ApplicationWindow : Component
    {
        #region Fields

        private string title;

        internal ApplicationContext ApplicationContext { get; set; }

        #endregion Fields

        #region Public Events

        /// <summary>
        /// Occurs when this window is activated.
        /// </summary>
        public event EventHandler<EventArgs> Activated;

        /// <summary>
        /// Occurs, when device client size is changed.
        /// </summary>
        public event EventHandler<EventArgs> ClientSizeChanged;

        /// <summary>
        /// Occurs when this window is deactivated.
        /// </summary>
        public event EventHandler<EventArgs> Deactivated;

        public event EventHandler<EventArgs> Initialization;

        /// <summary>
        /// Occurs, when device orientation is changed.
        /// </summary>
        public event EventHandler<EventArgs> OrientationChanged;

        public event EventHandler<EventArgs> Shutdown;

        public event EventHandler<EventArgs> Tick;

        #endregion Public Events

        #region Public Properties

        /// <summary>
        /// Gets or sets, user possibility to resize this window.
        /// </summary>
        public abstract bool AllowUserResizing { get; set; }

        /// <summary>
        /// Gets the client bounds.
        /// </summary>
        /// <value>The client bounds.</value>
        public abstract Rectangle ClientBounds { get; }

        /// <summary>
        /// Gets the current orientation.
        /// </summary>
        /// <value>The current orientation.</value>
        public abstract DisplayOrientation CurrentOrientation { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is minimized.
        /// </summary>
        /// <value><c>true</c> if this instance is minimized; otherwise, <c>false</c>.</value>
        public abstract bool IsMinimized { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the mouse pointer is visible over this window.
        /// </summary>
        /// <value><c>true</c> if this instance is mouse visible; otherwise, <c>false</c>.</value>
        public abstract bool IsMouseVisible { get; set; }

        /// <summary>
        /// Gets the native window.
        /// </summary>
        /// <value>The native window.</value>
        public abstract object NativeWindow { get; }

        /// <summary>
        /// Gets or sets the title of the window.
        /// </summary>
        public string Title
        {
            get { return title; }

            set
            {
                Contract.Requires<InvalidOperationException>(value != null);

                if (title != value)
                {
                    title = value;
                    SetTitle(title);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ApplicationWindow" /> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public abstract bool Visible { get; set; }

        internal abstract bool IsBlockingRun { get; }

        #endregion Public Properties

        #region Public Methods and Operators

        public abstract void BeginScreenDeviceChange(bool willBeFullScreen);

        public void EndScreenDeviceChange()
        {
            EndScreenDeviceChange(ClientBounds.Width, ClientBounds.Height);
        }

        public abstract void EndScreenDeviceChange(int clientWidth, int clientHeight);

        #endregion Public Methods and Operators

        #region Methods

        internal bool Exiting;

        internal IServiceRegistry Services { get; set; }

        /// <summary>
        /// Initializes the ApplicationWindow with the specified window context.
        /// </summary>
        /// <param name="applicationContext">The window context.</param>
        internal abstract bool CanHandle(ApplicationContext applicationContext);

        internal abstract void Initialize(ApplicationContext applicationContext);

        internal abstract void Resize(int width, int height);

        internal abstract void Run();

        /// <summary>
        /// Switches the rendering onto another game context.
        /// </summary>
        /// <param name="context">The new context to switch to.</param>
        internal abstract void Switch(ApplicationContext context);

        protected internal abstract void SetSupportedOrientations(DisplayOrientation orientations);

        protected void OnActivated(object source, EventArgs e)
        {
            EventHandler<EventArgs> handler = Activated;
            if (handler != null)
            {
                handler(source, e);
            }
        }

        protected void OnClientSizeChanged(object source, EventArgs e)
        {
            EventHandler<EventArgs> handler = ClientSizeChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected void OnDeactivated(object source, EventArgs e)
        {
            EventHandler<EventArgs> handler = Deactivated;
            if (handler != null)
            {
                handler(source, e);
            }
        }

        protected void OnInitialization(object source, EventArgs e)
        {
            EventHandler<EventArgs> handler = Initialization;
            if (handler != null)
            {
                handler(source, e);
            }
        }

        protected void OnOrientationChanged(object source, EventArgs e)
        {
            EventHandler<EventArgs> handler = OrientationChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected void OnShutdown(object source, EventArgs e)
        {
            EventHandler<EventArgs> handler = Shutdown;
            if (handler != null)
            {
                handler(source, e);
            }
        }

        protected void OnTick(object source, EventArgs e)
        {
            EventHandler<EventArgs> handler = Tick;
            if (handler != null) handler(this, e);
        }

        protected abstract void SetTitle(string title);

        #endregion Methods
    }
}
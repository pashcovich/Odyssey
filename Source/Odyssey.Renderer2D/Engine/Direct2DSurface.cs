#region Using Directives

using SharpDX;
using SharpDX.Direct2D1;
using System;
using System.Diagnostics.Contracts;

#endregion Using Directives

namespace Odyssey.Engine
{
    public class Direct2DSurface : Direct2DResource
    {
        private readonly Direct2DDevice d2DDevice;
        private readonly IServiceRegistry services;

        private bool isVisible;

        /// <summary>
        /// Initializes a new instance of the <see cref="Direct2DSurface"/> class.
        /// </summary>
        /// <param name="services">The service provider from where to get <see cref="IDirectXDeviceService"/> and <see cref="IDirect2DDevice"/>.</param>
        /// <exception cref="ArgumentNullException">Is thrown when <paramref name="services"/> is null.</exception>
        protected Direct2DSurface(Direct2DDevice device, string name)
            : base(device, name)
        {
            Contract.Requires<ArgumentNullException>(device != null, "device");
            d2DDevice = device;
            services = device.Services;

            IsDirty = true;
            IsVisible = true;
        }

        protected BitmapTarget BitmapTarget { get; set; }

        /// <summary>
        /// The service provider to retrieve additional services in derived classes.
        /// </summary>
        protected IServiceRegistry Services
        {
            get { return services; }
        }

        /// <summary>
        /// Indicates whether this surface needs to be redrawn.
        /// </summary>
        public bool IsDirty { get; private set; }

        /// <summary>
        /// The underlying Direct2D service to create additional resources.
        /// </summary>
        protected Direct2DDevice Direct2DDevice
        {
            get { return d2DDevice; }
        }

        /// <summary>
        /// Gets of sets the value indicating whether this surface is visible.
        /// </summary>
        /// <remarks>When this flag is changed from <c>false</c> to <c>true</c> - the surface is redrawn (<see cref="IsDirty"/> is set to true).</remarks>
        internal bool IsVisible
        {
            get { return isVisible; }
            set
            {
                if (value && (!isVisible)) IsDirty = true;
                isVisible = value;
            }
        }

        public bool IsInited { get; private set; }

        public override void Initialize()
        {
            var settings = services.GetService<IDirectXDeviceSettings>();
            BitmapTarget = ToDispose(BitmapTarget.New(d2DDevice, settings.PreferredBackBufferWidth, settings.PreferredBackBufferHeight, settings.PreferredBackBufferFormat));

            Initialize(BitmapTarget);
        }

        public void Unload()
        {
            if (BitmapTarget != null)
                BitmapTarget.Dispose();
        }

        /// <summary>
        /// RenderTargetView casting operator.
        /// </summary>
        /// <param name="from">Source for the.</param>
        public static implicit operator Bitmap1(Direct2DSurface from)
        {
            return from == null ? null : from.Resource != null ? (Bitmap1)from.Resource : null;
        }
    }
}
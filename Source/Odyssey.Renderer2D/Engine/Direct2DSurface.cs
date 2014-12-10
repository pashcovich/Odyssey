#region Using Directives

using System;
using Odyssey.Core;
using SharpDX.Direct2D1;

#endregion

namespace Odyssey.Engine
{
    public class Direct2DSurface : Direct2DResource
    {
        private readonly IServiceRegistry services;

        private bool isVisible;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Direct2DSurface" /> class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="device"></param>
        /// <exception cref="ArgumentNullException">Is thrown when <paramref name="device" /> is null.</exception>
        protected Direct2DSurface(string name, Direct2DDevice device)
            : base(name, device)
        {
            services = device.Services;

            IsDirty = true;
            IsVisible = true;
        }

        protected BitmapTarget BitmapTarget { get; set; }

        /// <summary>
        ///     The service provider to retrieve additional services in derived classes.
        /// </summary>
        protected IServiceRegistry Services
        {
            get { return services; }
        }

        /// <summary>
        ///     Indicates whether this surface needs to be redrawn.
        /// </summary>
        public bool IsDirty { get; private set; }


        /// <summary>
        ///     Gets of sets the value indicating whether this surface is visible.
        /// </summary>
        /// <remarks>
        ///     When this flag is changed from <c>false</c> to <c>true</c> - the surface is redrawn (<see cref="IsDirty" /> is
        ///     set to true).
        /// </remarks>
        internal bool IsVisible
        {
            get { return isVisible; }
            set
            {
                if (value && (!isVisible)) IsDirty = true;
                isVisible = value;
            }
        }

        public override void Initialize()
        {
            var settings = services.GetService<IDirectXDeviceSettings>();
            BitmapTarget =
                ToDispose(BitmapTarget.New(Name, Device, settings.PreferredBackBufferWidth,
                    settings.PreferredBackBufferHeight, settings.PreferredBackBufferFormat));

            Initialize(BitmapTarget);
        }

        public void Unload()
        {
            if (BitmapTarget != null)
                BitmapTarget.Dispose();
        }

        /// <summary>
        ///     RenderTargetView casting operator.
        /// </summary>
        /// <param name="from">Source for the.</param>
        public static implicit operator Bitmap1(Direct2DSurface from)
        {
            return from == null ? null : from.Resource != null ? (Bitmap1) from.Resource : null;
        }
    }
}
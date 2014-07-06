using SharpDX;
using System;
using SharpDX.Direct2D1;
using SharpDX.Direct3D11;

namespace Odyssey.Engine
{
    public abstract class TargetBase : Component, IDirectXTarget, IDeviceDependentComponent
    {
        protected DeviceManager deviceManager;
        protected RenderTargetView renderTargetView;
        protected RenderTargetView renderTargetViewRight;
        protected DepthStencilView depthStencilView;
        protected Texture2D backBuffer;

#if !WP8 && (DIRECTX11_2 || DIRECTX11_1)
        protected Bitmap1 bitmapTarget;
        protected Bitmap1 bitmapTargetRight;

        /// <summary>
        /// Gets the Direct2D RenderTarget used by this target.
        /// </summary>
        public Bitmap1 BitmapTarget { get { return bitmapTarget; } }
        public Bitmap1 BitmapTargetRight { get { return bitmapTargetRight; } }
#else
        protected SharpDX.Direct2D1.Bitmap bitmapTarget;
        protected SharpDX.Direct2D1.Bitmap bitmapTargetRight;

        /// <summary>
        /// Gets the Direct2D RenderTarget used by this target.
        /// </summary>
        public SharpDX.Direct2D1.Bitmap BitmapTarget { get { return bitmapTarget; } }
        public SharpDX.Direct2D1.Bitmap BitmapTargetRight { get { return bitmapTargetRight; } }
#endif

        /// <summary>
        /// Gets the Direct3D RenderTargetView used by this target.
        /// </summary>
        public RenderTargetView RenderTargetView { get { return renderTargetView; } }
        public RenderTargetView RenderTargetViewRight { get { return renderTargetViewRight; } }
        public Texture2D BackBuffer { get { return backBuffer; } }

        /// <summary>
        /// Gets the Direct3D DepthStencilView used by this target.
        /// </summary>
        public DepthStencilView DepthStencilView { get { return depthStencilView; } }


        /// <summary>
        /// Event fired when size of the underlying control is changed
        /// </summary>
        public event EventHandler<RenderEventArgs> SizeChanged;

        protected virtual void OnSizeChanged(RenderEventArgs e)
        {
            if (SizeChanged != null)
                SizeChanged(this, e);
        }

        /// <summary>
        /// Event fired when rendering is performed by this target
        /// </summary>
        public event EventHandler<RenderEventArgs> Render;

        /// <summary>
        /// Render all events registered on event <see cref="Render"/>
        /// </summary>
        public virtual void RenderAll()
        {
            if (Render != null)
                Render(this, new RenderEventArgs(this));
        }


        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <param name="deviceManager">The device manager</param>
        public virtual void Initialize(InitializeDirectXEventArgs args)
        {
            deviceManager = (DeviceManager)args.DirectX;

            // If the DPI is changed, we need to perform a OnSizeChanged event
            deviceManager.DpiChanged -= devices_OnDpiChanged;
            deviceManager.DpiChanged += devices_OnDpiChanged;
#if !WP8
            deviceManager.Dpi = args.Settings.Dpi;
#endif
        }

        public abstract void Close();

        protected virtual void Cleanup()
        {
            deviceManager.ContextDirect2D.Target = null;
            RemoveAndDispose(ref renderTargetView);
            RemoveAndDispose(ref depthStencilView);
            RemoveAndDispose(ref bitmapTarget);
            RemoveAndDispose(ref backBuffer);
        }

        private void devices_OnDpiChanged(object sender, EventArgs args)
        {
            if (SizeChanged != null)
                SizeChanged(this, new RenderEventArgs(this));
        }


        public IDirectXProvider DirectX
        {
            get { return deviceManager; }
        }

    }
}

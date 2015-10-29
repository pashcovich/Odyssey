#region Using Directives

using Odyssey.Core;
using Odyssey.Graphics;
using SharpDX.Mathematics;
using SharpDX.DXGI;

#endregion Using Directives

namespace Odyssey.Engine
{
    /// <summary>
    /// This class is a front end to <see cref="SwapChain" /> and <see cref="SwapChain1" />.
    /// </summary>
    /// <remarks>
    /// In order to create a new <see cref="GraphicsPresenter"/>, a <see cref="DirectXDevice"/> should have been initialized first.
    /// </remarks>
    /// <msdn-id>bb174569</msdn-id>
    /// <unmanaged>IDXGISwapChain</unmanaged>
    /// <unmanaged-short>IDXGISwapChain</unmanaged-short>
    public abstract class GraphicsPresenter : Component
    {
        private DepthStencilBuffer depthStencilBuffer;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsPresenter" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="presentationParameters"> </param>
        protected GraphicsPresenter(DirectXDevice device, PresentationParameters presentationParameters)
        {
            DirectXDevice = device;
            Description = presentationParameters.Clone();

            DefaultViewport = new ViewportF(0, 0, Description.BackBufferWidth, Description.BackBufferHeight);

            // Creates a default DepthStencilBuffer.
            CreateDepthStencilBuffer();
        }

        /// <summary>
        /// Gets the default back buffer for this presenter.
        /// </summary>
        public abstract RenderTarget2D BackBuffer { get; protected set; }

        /// <summary>
        /// Default viewport that covers the whole presenter surface.
        /// </summary>
        public ViewportF DefaultViewport { get; protected set; }

        /// <summary>
        /// Gets the default depth stencil buffer for this presenter.
        /// </summary>
        public DepthStencilBuffer DepthStencilBuffer
        {
            get { return depthStencilBuffer; }

            protected set { depthStencilBuffer = value; }
        }

        /// <summary>
        /// Gets the description of this presenter.
        /// </summary>
        public PresentationParameters Description { get; private set; }

        /// <summary>
        /// Gets the DirectX device.
        /// </summary>
        /// <value>The graphics device.</value>
        public DirectXDevice DirectXDevice { get; private set; }

        /// <summary>
        /// Gets or sets fullscreen mode for this presenter.
        /// </summary>
        /// <value><c>true</c> if this instance is full screen; otherwise, <c>false</c>.</value>
        /// <msdn-id>bb174579</msdn-id>
        ///   <unmanaged>HRESULT IDXGISwapChain::SetFullscreenState([In] BOOL Fullscreen,[In, Optional] IDXGIOutput* pTarget)</unmanaged>
        ///   <unmanaged-short>IDXGISwapChain::SetFullscreenState</unmanaged-short>
        /// <remarks>This method is only valid on Windows Desktop and has no effect on Windows Metro.</remarks>
        public abstract bool IsFullScreen { get; set; }

        /// <summary>
        /// Gets the underlying native presenter (can be a <see cref="SharpDX.DXGI.SwapChain"/> or <see cref="SharpDX.DXGI.SwapChain1"/> or null, depending on the platform).
        /// </summary>
        /// <value>The native presenter.</value>
        public abstract object NativePresenter { get; }

        /// <summary>
        /// Gets or sets the output index to use when switching to fullscreen mode.
        /// </summary>
        public int PreferredFullScreenOutputIndex { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="PresentInterval"/>. Default is to wait for one vertical blanking.
        /// </summary>
        /// <value>The present interval.</value>
        public PresentInterval PresentInterval
        {
            get { return Description.PresentationInterval; }
            set { Description.PresentationInterval = value; }
        }

        /// <summary>
        /// Presents the Backbuffer to the screen.
        /// </summary>
        /// <msdn-id>bb174576</msdn-id>
        /// <unmanaged>HRESULT IDXGISwapChain::Present([In] unsigned int SyncInterval,[In] DXGI_PRESENT_FLAGS Flags)</unmanaged>
        /// <unmanaged-short>IDXGISwapChain::Present</unmanaged-short>
        public abstract void Present();

        /// <summary>
        /// Resizes the current presenter, by resizing the back buffer and the depth stencil buffer.
        /// </summary>
        /// <param name="width">New backbuffer width</param>
        /// <param name="height">New backbuffer height</param>
        /// <param name="format">Backbuffer display format.</param>
        /// <param name="refreshRate"></param>
        /// <returns><c>true</c> if the presenter was resized, <c>false</c> otherwise</returns>
        public virtual bool Resize(int width, int height, Format format, Rational? refreshRate = null)
        {
            if (Description.BackBufferWidth == width && Description.BackBufferHeight == height &&
                Description.BackBufferFormat == format)
            {
                return false;
            }

            if (DepthStencilBuffer != null)
            {
                RemoveAndDispose(ref depthStencilBuffer);
            }

            Description.BackBufferWidth = width;
            Description.BackBufferHeight = height;
            Description.BackBufferFormat = format;
            if (refreshRate.HasValue)
            {
                Description.RefreshRate = refreshRate.Value;
            }

            DefaultViewport = new ViewportF(0, 0, Description.BackBufferWidth, Description.BackBufferHeight);

            CreateDepthStencilBuffer();
            return true;
        }

        /// <summary>
        /// Creates the depth stencil buffer.
        /// </summary>
        protected virtual void CreateDepthStencilBuffer()
        {
            // If no depth stencil buffer, just return
            if (Description.DepthStencilFormat == DepthFormat.None)
            {
                return;
            }

            // Creates the depth stencil buffer.
            DepthStencilBuffer =
                ToDispose(DepthStencilBuffer.New(DirectXDevice,
                    Description.BackBufferWidth,
                    Description.BackBufferHeight,
                    Description.MultiSampleCount,
                    Description.DepthStencilFormat,
                    Description.DepthBufferShaderResource, 1));
        }

        //protected virtual void CreateDepthStencilBuffer()
        //{
        //    // If no depth stencil buffer, just return
        //    if (Description.DepthStencilFormat == DepthFormat.None)
        //    {
        //        return;
        //    }

        //    var depthBufferDescription = new Texture2DDescription
        //    {
        //        Format = (Format)Description.DepthStencilFormat,
        //        ArraySize = 1,
        //        MipLevels = 1,
        //        Width = Description.BackBufferWidth,
        //        Height = Description.BackBufferHeight,
        //        SampleDescription = new SampleDescription(1, 0),
        //        BindFlags = BindFlags.DepthStencil,
        //    };

        //    // Sets the MSAALevel
        //    int maximumMSAA = (int)DirectXDevice.Features[(Format)Description.DepthStencilFormat].MSAALevelMax;
        //    depthBufferDescription.SampleDescription.Count = Math.Max(1, Math.Min((int)Description.MultiSampleCount, maximumMSAA));

        //    var depthStencilDescription = new DepthStencilViewDescription
        //    {
        //                   Format = (Format)Description.DepthStencilFormat,
        //                   Flags = DepthStencilViewFlags.None,
        //                   Dimension = DepthStencilViewDimension.Texture2D,
        //                   Texture2D = new DepthStencilViewDescription.Texture2DResource { MipSlice = 0 }
        //               };

        //    if ((int)Description.MultiSampleCount > 1)
        //        depthStencilDescription.Dimension = DepthStencilViewDimension.Texture2DMultisampled;

        //    // Create a descriptor for the depth/stencil buffer. Allocate a 2-D surface as the
        //    // depth/stencil buffer. Create a DepthStencil view on this surface to use on bind.
        //    using (var depthBuffer = new Texture2D(DirectXDevice.NativeDevice,
        //        depthBufferDescription))
        //        DepthStencilBuffer = ToDispose(new DepthStencilBuffer(DirectXDevice.NativeDevice, depthBuffer, depthStencilDescription));
        //}
    }
}
using Odyssey.Graphics;
using SharpDX.DXGI;

namespace Odyssey.Engine
{
    /// <summary>
    /// Describes how data will be displayed to the screen.
    /// </summary>
    /// <msdn-id>bb173075</msdn-id>
    /// <unmanaged>DXGI_SWAP_CHAIN_DESC</unmanaged>
    /// <unmanaged-short>DXGI_SWAP_CHAIN_DESC</unmanaged-short>
    public class PresentationParameters
    {
        #region Fields

        /// <summary>
        ///   A <strong><see cref="SharpDX.DXGI.Format" /></strong> structure describing the display format.
        /// </summary>
        /// <msdn-id>bb173075</msdn-id>
        /// <unmanaged>DXGI_MODE_DESC BufferDesc</unmanaged>
        /// <unmanaged-short>DXGI_MODE_DESC BufferDesc</unmanaged-short>
        public Format BackBufferFormat;

        /// <summary>
        ///   A value that describes the resolution height.
        /// </summary>
        /// <msdn-id>bb173075</msdn-id>
        /// <unmanaged>DXGI_MODE_DESC BufferDesc</unmanaged>
        /// <unmanaged-short>DXGI_MODE_DESC BufferDesc</unmanaged-short>
        public int BackBufferHeight;

        /// <summary>
        ///   A value that describes the resolution width.
        /// </summary>
        /// <msdn-id>bb173075</msdn-id>
        /// <unmanaged>DXGI_MODE_DESC BufferDesc</unmanaged>
        /// <unmanaged-short>DXGI_MODE_DESC BufferDesc</unmanaged-short>
        public int BackBufferWidth;

        /// <summary>
        /// Gets or sets the depth stencil format
        /// </summary>
        public DepthFormat DepthStencilFormat;

        /// <summary>
        ///   A Window object. See remarks.
        /// </summary>
        /// <remarks>
        ///   A window object is platform dependent:
        ///   <ul>
        ///     <li>On Windows Desktop: This could a low level window/control handle (IntPtr), or directly a Winform Control object.</li>
        ///     <li>On Windows Metro: This could be SwapChainBackgroundPanel object.</li>
        ///   </ul>
        /// </remarks>
        public object DeviceWindowHandle;

        /// <summary>
        ///   A member of the <see cref="SharpDX.DXGI.SwapChainFlags" />
        ///   enumerated type that describes options for swap-chain behavior.
        /// </summary>
        /// <msdn-id>bb173075</msdn-id>
        /// <unmanaged>DXGI_SWAP_CHAIN_FLAG Flags</unmanaged>
        /// <unmanaged-short>DXGI_SWAP_CHAIN_FLAG Flags</unmanaged-short>
        public SwapChainFlags Flags;

        /// <summary>
        ///   Gets or sets a value indicating whether the application is in full screen mode.
        /// </summary>
        public bool IsFullScreen;

        /// <summary>
        ///   Gets or sets a value indicating whether the application is using stereoscopy.
        /// </summary>
        public bool IsStereo;

        /// <summary>
        ///   Gets or sets a value indicating the number of sample locations during multisampling.
        /// </summary>
        public MSAALevel MultiSampleCount;

        /// <summary>
        ///   Gets or sets the maximum rate at which the swap chain's back buffers can be presented to the front buffer.
        /// </summary>
        public PresentInterval PresentationInterval;

        /// <summary>
        ///   A <see cref="SharpDX.DXGI.Rational" />   structure describing the refresh rate in hertz
        /// </summary>
        /// <msdn-id>bb173064</msdn-id>
        /// <unmanaged>DXGI_RATIONAL RefreshRate</unmanaged>
        /// <unmanaged-short>DXGI_RATIONAL RefreshRate</unmanaged-short>
        public Rational RefreshRate;

        /// <summary>
        ///   <p>A member of the DXGI_USAGE enumerated type that describes the surface usage and CPU access options for the back buffer. The back buffer can  be used for shader input or render-target output.</p>
        /// </summary>
        /// <msdn-id>bb173075</msdn-id>
        /// <unmanaged>DXGI_USAGE_ENUM BufferUsage</unmanaged>
        /// <unmanaged-short>DXGI_USAGE_ENUM BufferUsage</unmanaged-short>
        public Usage RenderTargetUsage;

        /// <summary>
        /// The output (monitor) index to use when switching to fullscreen mode. Doesn't have any effect when windowed mode is used.
        /// </summary>
        public int PreferredFullScreenOutputIndex;

        /// <summary>
        /// Indicates whether the DepthBuffer should be created with the ShaderResource flag. Default is false.
        /// </summary>
        public bool DepthBufferShaderResource;

        #endregion Fields

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PresentationParameters" /> class with default values.
        /// </summary>
        public PresentationParameters()
        {
            BackBufferWidth = 1280;
            BackBufferHeight = 720;
            BackBufferFormat = Format.R8G8B8A8_UNorm;
            PresentationInterval = PresentInterval.Immediate;
            DepthStencilFormat = DepthFormat.Depth24Stencil8;
            MultiSampleCount = MSAALevel.None;
            IsFullScreen = false;
            IsStereo = false;
            RefreshRate = new Rational(60, 1); // by default
            RenderTargetUsage = Usage.BackBuffer | Usage.RenderTargetOutput;
            Flags = SwapChainFlags.AllowModeSwitch;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PresentationParameters" /> class with <see cref="PixelFormat.R8G8B8A8.UNorm"/>.
        /// </summary>
        /// <param name="backBufferWidth">Width of the back buffer.</param>
        /// <param name="backBufferHeight">Height of the back buffer.</param>
        /// <param name="deviceWindowHandle">The device window handle.</param>
        public PresentationParameters(int backBufferWidth, int backBufferHeight, object deviceWindowHandle)
            : this(backBufferWidth, backBufferHeight, deviceWindowHandle, Format.R8G8B8A8_UNorm)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PresentationParameters" /> class.
        /// </summary>
        /// <param name="backBufferWidth">Width of the back buffer.</param>
        /// <param name="backBufferHeight">Height of the back buffer.</param>
        /// <param name="deviceWindowHandle">The device window handle.</param>
        /// <param name="backBufferFormat">The back buffer format.</param>
        public PresentationParameters(int backBufferWidth, int backBufferHeight, object deviceWindowHandle, Format backBufferFormat)
            : this()
        {
            BackBufferWidth = backBufferWidth;
            BackBufferHeight = backBufferHeight;
            DeviceWindowHandle = deviceWindowHandle;
            BackBufferFormat = backBufferFormat;
        }

        #endregion Constructors and Destructors

        #region Methods

        public PresentationParameters Clone()
        {
            return (PresentationParameters)MemberwiseClone();
        }

        #endregion Methods
    }
}
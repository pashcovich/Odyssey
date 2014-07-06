#region Using Directives

using Odyssey.Graphics;
using SharpDX.Direct3D;
using SharpDX.DXGI;

#endregion Using Directives

namespace Odyssey.Engine
{
    /// <summary>
    ///   Describes how data will be displayed to the screen.
    /// </summary>
    /// <msdn-id>bb173075</msdn-id>
    /// <unmanaged>DXGI_SWAP_CHAIN_DESC</unmanaged>
    /// <unmanaged-short>DXGI_SWAP_CHAIN_DESC</unmanaged-short>
    public class ApplicationGraphicsParameters
    {
        /// <summary>
        /// Indicates whether the DepthBuffer should be created with the ShaderResource flag. Default is false.
        /// </summary>
        public bool DepthBufferShaderResource;

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
        public bool PreferMultiSampling;

        /// <summary>
        ///   A <strong><see cref="SharpDX.DXGI.Format" /></strong> structure describing the display format.
        /// </summary>
        /// <msdn-id>bb173075</msdn-id>
        /// <unmanaged>DXGI_MODE_DESC BufferDesc</unmanaged>
        /// <unmanaged-short>DXGI_MODE_DESC BufferDesc</unmanaged-short>
        public Format PreferredBackBufferFormat;

        /// <summary>
        ///   A value that describes the resolution height.
        /// </summary>
        /// <msdn-id>bb173075</msdn-id>
        /// <unmanaged>DXGI_MODE_DESC BufferDesc</unmanaged>
        /// <unmanaged-short>DXGI_MODE_DESC BufferDesc</unmanaged-short>
        public int PreferredBackBufferHeight;

        /// <summary>
        ///   A value that describes the resolution width.
        /// </summary>
        /// <msdn-id>bb173075</msdn-id>
        /// <unmanaged>DXGI_MODE_DESC BufferDesc</unmanaged>
        /// <unmanaged-short>DXGI_MODE_DESC BufferDesc</unmanaged-short>
        public int PreferredBackBufferWidth;

        /// <summary>
        /// Gets or sets the depth stencil format
        /// </summary>
        public DepthFormat PreferredDepthStencilFormat;

        /// <summary>
        /// The output (monitor) index to use when switching to fullscreen mode. Doesn't have any effect when windowed mode is used.
        /// </summary>
        public int PreferredFullScreenOutputIndex;

        /// <summary>
        /// Gets or sets the minimum graphics profile.
        /// </summary>
        public FeatureLevel[] PreferredGraphicsProfile;

        public int PreferredMultiSampleCount;

        /// <summary>
        /// Gets or sets a value indicating whether to synchronize present with vertical blanking.
        /// </summary>
        public bool SynchronizeWithVerticalRetrace;
    }
}
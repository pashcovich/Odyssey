#region Using Directives

using Odyssey.Graphics;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

#endregion Using Directives

namespace Odyssey.Engine
{
    /// <summary>
    /// Graphics presenter for SwapChain.
    /// </summary>
    public class RenderTargetGraphicsPresenter : GraphicsPresenter
    {
        private readonly bool allowFormatChange;
        private readonly bool allowRecreateBackBuffer;
        private RenderTarget2D backBuffer;
        private Texture2DDescription renderTargetDescription;

        public RenderTargetGraphicsPresenter(DirectXDevice device, Texture2DDescription renderTargetDescription,
            DepthFormat depthFormat = DepthFormat.None, bool allowFormatChange = true, bool disposeRenderTarget = false,
            bool depthAsShaderResource = false)
            : base(device, CreatePresentationParameters(renderTargetDescription, depthFormat, depthAsShaderResource))
        {
            PresentInterval = Description.PresentationInterval;

            this.renderTargetDescription = renderTargetDescription;
            this.allowFormatChange = allowFormatChange;

            allowRecreateBackBuffer = true;

            backBuffer = RenderTarget2D.New(device, renderTargetDescription);

            if (disposeRenderTarget)
                ToDispose(backBuffer);
        }

        public RenderTargetGraphicsPresenter(DirectXDevice device, RenderTarget2D backBuffer,
            DepthFormat depthFormat = DepthFormat.None, bool disposeRenderTarget = false, bool depthAsShaderResource = false)
            : base(device, CreatePresentationParameters(backBuffer, depthFormat, depthAsShaderResource))
        {
            PresentInterval = Description.PresentationInterval;

            this.backBuffer = backBuffer;

            if (disposeRenderTarget)
                ToDispose(this.backBuffer);
        }

        public override RenderTarget2D BackBuffer
        {
            get { return backBuffer; }
            protected set { backBuffer = value; }
        }

        public override object NativePresenter => backBuffer;

        public override bool IsFullScreen
        {
            get { return true; }

            set { }
        }

        private static PresentationParameters CreatePresentationParameters(Texture2DDescription renderTargetDescription,
            DepthFormat depthFormat, bool depthAsShaderResource)
        {
            return new PresentationParameters()
            {
                BackBufferWidth = renderTargetDescription.Width,
                BackBufferHeight = renderTargetDescription.Height,
                BackBufferFormat = renderTargetDescription.Format,
                DepthStencilFormat = depthFormat,
                DepthBufferShaderResource = depthAsShaderResource,
                DeviceWindowHandle = renderTargetDescription,
                Flags = SwapChainFlags.None,
                IsFullScreen = true,
                MultiSampleCount = MSAALevel.None,
                PresentationInterval = PresentInterval.One,
                RefreshRate = new Rational(60, 1),
                RenderTargetUsage = Usage.RenderTargetOutput
            };
        }

        private static PresentationParameters CreatePresentationParameters(RenderTarget2D renderTarget2D, DepthFormat depthFormat,
            bool depthAsShaderResource)
        {
            return new PresentationParameters()
            {
                BackBufferWidth = renderTarget2D.Width,
                BackBufferHeight = renderTarget2D.Height,
                BackBufferFormat = renderTarget2D.Description.Format,
                DepthStencilFormat = depthFormat,
                DepthBufferShaderResource = depthAsShaderResource,
                DeviceWindowHandle = renderTarget2D,
                Flags = SwapChainFlags.None,
                IsFullScreen = true,
                MultiSampleCount = MSAALevel.None,
                PresentationInterval = PresentInterval.One,
                RefreshRate = new Rational(60, 1),
                RenderTargetUsage = Usage.RenderTargetOutput
            };
        }

        public void SetBackBuffer(RenderTarget2D backBuffer)
        {
            this.backBuffer = backBuffer;
        }

        public override void Present()
        {
#if !WP8
            DirectXDevice.Flush();
#endif
        }

        public override bool Resize(int width, int height, Format format, Rational? refreshRate)
        {
            if (!base.Resize(width, height, format, refreshRate)) return false;

            // backbuffer was set externally, do not touch it
            if (!allowRecreateBackBuffer) return false;

            renderTargetDescription.Width = width;
            renderTargetDescription.Height = height;

            if (allowFormatChange)
                renderTargetDescription.Format = format;

            if (backBuffer != null)
            {
                RemoveAndDispose(ref backBuffer);
                backBuffer = RenderTarget2D.New(DirectXDevice, renderTargetDescription);
            }
            return true;
        }
    }
}
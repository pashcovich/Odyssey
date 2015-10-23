#region Using Directives

using Odyssey.Graphics;
using SharpDX.Mathematics;
using SharpDX.DXGI;
using System;
using Texture2D = SharpDX.Direct3D11.Texture2D;

#endregion Using Directives

namespace Odyssey.Engine
{
    /// <summary>
    /// Graphics presenter for SwapChain.
    /// </summary>
    public abstract class SwapChainGraphicsPresenter : GraphicsPresenter, ISwapChainPresenterService
    {
        private RenderTarget2D backBuffer;
        private readonly SwapChain swapChain;

        /// <summary>
        /// Gets the default back buffer for this presenter.
        /// </summary>
        public override RenderTarget2D BackBuffer => backBuffer;

        protected SwapChain SwapChain { get; set; }

        protected void SetBackbuffer(RenderTarget2D backBuffer)
        {
            this.backBuffer = backBuffer;
        }

        protected SwapChainGraphicsPresenter(DirectXDevice device, PresentationParameters presentationParameters)
            : base(device, presentationParameters)
        {
            PresentInterval = presentationParameters.PresentationInterval;

            // Initialize the swap chain
            swapChain = ToDispose(CreateSwapChain());
            var rawBackbuffer = swapChain.GetBackBuffer<Texture2D>(0);
            backBuffer = ToDispose(RenderTarget2D.New(device, rawBackbuffer));
        }

        protected int BufferCount { get; set; }

        public override object NativePresenter => swapChain;

        public override void Present()
        {
            swapChain.Present((int)PresentInterval, PresentFlags.None);
        }

        /// <summary>
        /// Called when name changed for this component.
        /// </summary>
        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == "Name")
            {
                if (DirectXDevice.IsDebugMode && swapChain != null)
                {
                    swapChain.DebugName = Name;
                }
            }
        }

        public override bool Resize(int width, int height, Format format, Rational? refreshRate = null)
        {
            if (!base.Resize(width, height, format, refreshRate)) return false;

            string backBufferName = backBuffer.DebugName;
            RemoveAndDispose(ref backBuffer);

            swapChain.ResizeBuffers(BufferCount, width, height, format, Description.Flags);

            // Recreate the back buffer
            backBuffer = ToDispose(RenderTarget2D.New(DirectXDevice, swapChain.GetBackBuffer<Texture2D>(0)));
            backBuffer.DebugName = backBufferName;
            // Reinit the Viewport
            DefaultViewport = new ViewportF(0, 0, backBuffer.Width, backBuffer.Height);

            return true;
        }

        protected SwapChain CreateSwapChain()
        {
            // Check for Window Handle parameter
            if (Description.DeviceWindowHandle == null)
            {
                throw new ArgumentException("DeviceWindowHandle cannot be null");
            }

            return CreateSwapChainResources();
        }

        protected abstract SwapChain CreateSwapChainResources();

        #region ISwapChainPresenterService

        SharpDX.Direct3D11.RenderTargetView ISwapChainPresenterService.BackBuffer => backBuffer;

        SwapChain ISwapChainPresenterService.SwapChain => swapChain;

        #endregion ISwapChainPresenterService
    }
}
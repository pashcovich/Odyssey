using Odyssey.Graphics;
using SharpDX.DXGI;
using SharpDX.Mathematics;
using Texture2D = SharpDX.Direct3D11.Texture2D;

namespace Odyssey.Engine
{
    public abstract class MonoSwapChainPresenter : SwapChainGraphicsPresenter
    {
        public MonoSwapChainPresenter(DirectXDevice device, PresentationParameters presentationParameters) : base(device, presentationParameters)
        {
        }

        public override bool Resize(int width, int height, Format format, Rational? refreshRate = null)
        {
            if (!base.Resize(width, height, format, refreshRate)) return false;

            string backBufferName = BackBuffer.DebugName;
            var backBuffer = BackBuffer;
            RemoveAndDispose(ref backBuffer);

            SwapChain.ResizeBuffers(BufferCount, width, height, format, Description.Flags);

            // Recreate the back buffer
            backBuffer = ToDispose(RenderTarget2D.New(DirectXDevice, SwapChain.GetBackBuffer<Texture2D>(0)));
            backBuffer.DebugName = backBufferName;
            // Reinit the Viewport
            DefaultViewport = new ViewportF(0, 0, backBuffer.Width, backBuffer.Height);

            return true;
        }
    }
}

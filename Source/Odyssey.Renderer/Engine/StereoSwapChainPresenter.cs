using Odyssey.Graphics;
using SharpDX.Direct3D11;
using Texture2D = SharpDX.Direct3D11.Texture2D;

namespace Odyssey.Engine
{
    public abstract class StereoSwapChainPresenter : SwapChainGraphicsPresenter
    {
        private StereoChannel currentChannel;
        private readonly RenderTargetView targetRight;
        private readonly RenderTargetView targetLeft;

        public RenderTargetView TargetRight => targetRight;
        public RenderTargetView TargetLeft => targetLeft;

        public StereoSwapChainPresenter(DirectXDevice device, PresentationParameters presentationParameters)
            : base(device, presentationParameters)
        {
            PresentInterval = presentationParameters.PresentationInterval;

            var rawBackbuffer = SwapChain.GetBackBuffer<Texture2D>(0);

            RenderTargetViewDescription rtvDescription = new RenderTargetViewDescription()
            {
                Dimension = RenderTargetViewDimension.Texture2DArray,
                Format = presentationParameters.BackBufferFormat,
                Texture2DArray = new RenderTargetViewDescription.Texture2DArrayResource()
                { 
                    MipSlice = 0,
                    FirstArraySlice = 0,
                    ArraySize = 1
                }
            };
            // Create a view interface on the rendertarget to use on bind.
            targetLeft = ToDispose(new RenderTargetView(device, rawBackbuffer, rtvDescription));
            var backBuffer = BackBuffer;
            RemoveAndDispose(ref backBuffer);
            BackBuffer = RenderTarget2D.New(device, targetLeft);
            // Create a view interface on the rendertarget to use on bind.
            rtvDescription.Texture2DArray.FirstArraySlice = 1;
            targetRight = ToDispose(new RenderTargetView(device, rawBackbuffer, rtvDescription));
        }

        public void AlternateTargets()
        {
            if (currentChannel == StereoChannel.Left)
            {
                DirectXDevice.SetRenderTargets(DepthStencilBuffer, targetRight);
                currentChannel = StereoChannel.Right;
            }
            else
            {
                DirectXDevice.SetRenderTargets(DepthStencilBuffer, targetLeft);
                currentChannel = StereoChannel.Left;
            }
        }
    }
}

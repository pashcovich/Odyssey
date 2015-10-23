using Odyssey.Graphics;
using SharpDX.Direct3D11;
using Texture2D = SharpDX.Direct3D11.Texture2D;

namespace Odyssey.Engine
{
    public abstract class StereoSwapChainGraphicsPresenter : SwapChainGraphicsPresenter
    {
        private RenderTargetView targetRight;
        private RenderTargetView targetLeft;

        public RenderTargetView TargetRight => targetRight;
        public RenderTargetView TargetLeft => targetLeft;

        public StereoSwapChainGraphicsPresenter(DirectXDevice device, PresentationParameters presentationParameters)
            : base(device, presentationParameters)
        {
            PresentInterval = presentationParameters.PresentationInterval;

            // Initialize the swap chain
            SwapChain = ToDispose(CreateSwapChain());
            var rawBackbuffer = SwapChain.GetBackBuffer<Texture2D>(0);

            RenderTargetViewDescription rtvDescription = new RenderTargetViewDescription()
            {
                Dimension = RenderTargetViewDimension.Texture2DMultisampledArray,
                Format = presentationParameters.BackBufferFormat,
                Texture2DMSArray = new RenderTargetViewDescription.Texture2DMultisampledArrayResource()
                { 
                    FirstArraySlice = 0, ArraySize = 1
                }
            };
            var backbuffer =ToDispose(RenderTarget2D.New(device, rawBackbuffer));
            // Create a view interface on the rendertarget to use on bind.
            targetLeft = ToDispose(new RenderTargetView(device, backbuffer, rtvDescription));
            // Create a view interface on the rendertarget to use on bind.
            rtvDescription.Texture2DArray.FirstArraySlice = 1;
            targetRight = ToDispose(new RenderTargetView(device, backbuffer, rtvDescription));
        }
    }
}

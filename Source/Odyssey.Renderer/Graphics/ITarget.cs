using SharpDX.Direct3D11;

namespace Odyssey.Graphics
{
    public interface ITarget
    {
        DepthStencilView DepthStencilBuffer { get; }
        RenderTargetView RenderTarget{ get; }
    }
}

#region Using Directives

using SharpDX.Direct3D11;
using SharpDX.DXGI;

#endregion Using Directives

namespace Odyssey.Engine
{
    public interface ISwapChainPresenterService
    {
        RenderTargetView BackBuffer { get; }

        SwapChain SwapChain { get; }
    }
}
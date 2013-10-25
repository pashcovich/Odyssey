#if !ODYSSEY_ENGINE && DIRECTX11_1
using SharpDX.DXGI;
using System;

namespace Odyssey.Engine
{
    public interface IDirectXTarget : IDisposable
    {
        SharpDX.Direct3D11.Texture2D BackBuffer { get; }
        SharpDX.Direct3D11.DepthStencilView DepthStencilView { get; }
        SharpDX.Direct3D11.RenderTargetView RenderTargetView { get; }
        
        IDirect3DProvider Direct3D { get; }
#if !WP8
        SharpDX.Direct2D1.Bitmap1 BitmapTarget { get; }
        IDirect2DProvider Direct2D { get; }
        IDirectWriteProvider DirectWrite { get; }
#endif

    }

    public interface ISwapChainTarget : IDirectXTarget
    {
        void Present();
        SwapChain1 SwapChain { get; }
    }

    public interface IStereoSwapChainTarget : ISwapChainTarget
    {
        bool IsStereoEnabled { get; }
        SharpDX.Direct3D11.RenderTargetView RenderTargetViewRight { get; }
#if !WP8
        SharpDX.Direct2D1.Bitmap1 BitmapTargetRight { get; }
#endif

    }

    public interface IOdysseyTarget : IStereoSwapChainTarget, IDeviceDependentComponent
    {
        event EventHandler<RenderEventArgs> Render;

        void RenderAll();
        void Close();
    }
}
#endif
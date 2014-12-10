#region Using Directives

using System;
using Odyssey.Text.Logging;
using SharpDX.DXGI;

#endregion

namespace Odyssey.Engine
{
    public class BackBufferSurface : Direct2DSurface
    {
        protected BackBufferSurface(Direct2DDevice device)
            : base("D2D_Backbuffer", device)
        {
        }

        public static BackBufferSurface New(Direct2DDevice device)
        {
            return new BackBufferSurface(device);
        }

        public override void Initialize()
        {
            try
            {
                var swapChainService = Services.GetService<ISwapChainPresenterService>();
                BitmapTarget =
                    ToDispose(BitmapTarget.New("BT_Backbuffer", Device,
                        swapChainService.SwapChain.GetBackBuffer<Surface2>(0)));
                Initialize(BitmapTarget);
            }
            catch (ArgumentException e)
            {
                LogEvent.Engine.Warning("A SwapChain is required");
                LogEvent.Engine.Error(e);
            }
        }
    }
}
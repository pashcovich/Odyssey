#region Using Directives

using Odyssey.Utilities.Logging;
using SharpDX.DXGI;
using System;

#endregion Using Directives

namespace Odyssey.Engine
{
    public class BackBufferSurface : Direct2DSurface
    {
        protected BackBufferSurface(Direct2DDevice device)
            : base(device, "D2D_Backbuffer")
        {
        }

        public static BackBufferSurface New(Direct2DDevice device)
        {
            return new BackBufferSurface(device);
        }

        public override void Initialize()
        {
            var settings = Services.GetService<IDirectXDeviceSettings>();
            try
            {
                var swapChainService = Services.GetService<ISwapChainPresenterService>();
                BitmapTarget = ToDispose(BitmapTarget.New(Direct2DDevice, settings.HorizontalDpi, settings.VerticalDpi,
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
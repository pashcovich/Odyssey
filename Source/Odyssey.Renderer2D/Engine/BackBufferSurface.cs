#region Using Directives

using System;
using Odyssey.Text.Logging;
using SharpDX.DXGI;

#endregion

namespace Odyssey.Engine
{
    public class BackBufferSurface : Direct2DSurface
    {
        protected BitmapTarget BitmapTargetRight { get; set; }

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
                var swapChain = Services.GetService<ISwapChainPresenterService>().SwapChain;
                if (Services.GetService<IDirectXDeviceSettings>().IsStereo)
                {
                    using (var dxgiBackBuffer = swapChain.GetBackBuffer<SharpDX.DXGI.Resource1>(0))
                    {
                        using (var dxgiSurface = new Surface2(dxgiBackBuffer, 0))
                            BitmapTarget = ToDispose(BitmapTarget.New("BT_BackbufferLeft", Device, dxgiSurface));
                        using (var dxgiSurface = new Surface2(dxgiBackBuffer, 1))
                            BitmapTarget = ToDispose(BitmapTarget.New("BT_BackbufferRight", Device, dxgiSurface));
                        Initialize(BitmapTargetRight);
                    }
                }
                else
                {
                    BitmapTarget = ToDispose(BitmapTarget.New("BT_Backbuffer", Device, swapChain.GetBackBuffer<Surface2>(0)));
                }
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
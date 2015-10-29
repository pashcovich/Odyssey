using System;
using Odyssey.Graphics;
using SharpDX.DXGI;
using Texture2D = SharpDX.Direct3D11.Texture2D;

namespace Odyssey.Engine
{
    class StereoDesktopPresenter : StereoSwapChainPresenter
    {
        public StereoDesktopPresenter(DirectXDevice device, PresentationParameters presentationParameters) : base(device, presentationParameters)
        {
        }

        public override bool IsFullScreen { get; set; }

        protected override SwapChain CreateSwapChainResources()
        {
            bool isStereo = Description.IsStereo;
            bool windows8 = Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor == 2;
            var handle = DesktopPresenter.FindHandle(Description.DeviceWindowHandle);

            var description = new SwapChainDescription1()
            {
                // Automatic sizing
                Width = Description.BackBufferWidth,
                Height = Description.BackBufferHeight,
                Format = Description.BackBufferFormat,
                Stereo = true,
                SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                Usage = Description.RenderTargetUsage,

                // Use two buffers to enable flip effect.
                BufferCount = 2,
                Scaling = windows8 ? SharpDX.DXGI.Scaling.None : Scaling.Stretch,
                SwapEffect = SharpDX.DXGI.SwapEffect.FlipSequential,
            };

            var scFullScreenDesc = new SwapChainFullScreenDescription()
            {
                Windowed = !Description.IsFullScreen,
                RefreshRate = new Rational(120, 1)
            };
            using (var dxgiDevice2 = DirectXDevice.NativeDevice.QueryInterface<SharpDX.DXGI.Device2>())
            using (var dxgiAdapter = dxgiDevice2.Adapter)
            using (var dxgiFactory2 = dxgiAdapter.GetParent<Factory2>())
            {
                var swapChain = new SwapChain1(dxgiFactory2, DirectXDevice.NativeDevice, handle.Value, ref description, scFullScreenDesc);
                swapChain.ResizeBuffers(2, description.Width, description.Height, Description.BackBufferFormat, description.Flags);

                // Ensure that DXGI does not queue more than one frame at a time. This both
                // reduces latency and ensures that the application will only render after each
                // VSync, minimizing power consumption.
                dxgiDevice2.MaximumFrameLatency = 1;
                return swapChain;
            }

        }
    }
}

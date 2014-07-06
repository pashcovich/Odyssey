using SharpDX.Direct3D11;
using SharpDX.DXGI;

// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

namespace Odyssey.Engine
{
    /// <summary>
    /// Base class to render to a SwapChain.
    /// </summary>
    /// <remarks>
    /// This class is the base class for <see cref="CoreWindowTarget"/>
    /// and <see cref="SwapChainBackgroundPanelTarget"/>.
    /// </remarks>
    public abstract class SwapChainTargetBase : TargetBaseWindows8, ISwapChainTarget
    {
        private SharpDX.DXGI.SwapChain1 swapChain;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        protected SwapChainTargetBase()
        {
            SizeChanged += CreateSizeDependentResources;
        }

        /// <summary>
        /// Width of the swap chain to create or resize.
        /// </summary>
        protected virtual int Width
        {
            get
            {
                return (int)(ControlBounds.Width * deviceManager.Dpi / 96.0);
            }
        }

        /// <summary>
        /// Height of the swap chain to create or resize.
        /// </summary>
        protected virtual int Height
        {
            get
            {
                return (int)(ControlBounds.Height * deviceManager.Dpi / 96.0);
            }
        }

        public SwapChain1 SwapChain
        {
            get { return swapChain; }
        }

        public bool IsStereoEnabled
        {
            get { return deviceManager.Settings.IsStereo; }
        }

        /// <summary>
        /// Present the results to the swap chain.
        /// </summary>
        public virtual void Present()
        {
            // The application may optionally specify "dirty" or "scroll" rects to improve efficiency
            // in certain scenarios.  In this sample, however, we do not utilize those features.
            var parameters = new SharpDX.DXGI.PresentParameters();

            try
            {
                // The first argument instructs DXGI to block until VSync, putting the application
                // to sleep until the next VSync. This ensures we don't waste any cycles rendering
                // frames that will never be displayed to the screen.
                swapChain.Present(1, SharpDX.DXGI.PresentFlags.None, parameters);
            }
            catch (SharpDX.SharpDXException ex)
            {
                // TODO PLUG CODE HERE TO REINITIALIZE

                // If the device was removed either by a disconnect or a driver upgrade, we
                // must completely reinitialize the renderer.
                if (ex.ResultCode == SharpDX.DXGI.ResultCode.DeviceRemoved
                    || ex.ResultCode == SharpDX.DXGI.ResultCode.DeviceReset)
                    deviceManager.InitializeDeviceDependentResources();
                else
                    throw;
            }
        }

        protected virtual void CreateSizeDependentResources(object sender, RenderEventArgs e)
        {
            var d3dDevice = DirectX.Direct3D.Device;
            var d3dContext = DirectX.Direct3D.Context;
            var d2dContext = DirectX.Direct2D.Context;
            bool isStereoEnabled = deviceManager.Settings.IsStereo;

            Cleanup();

            // If the swap chain already exists, resize it.
            if (swapChain != null)
            {
                swapChain.ResizeBuffers(2, Width, Height, SharpDX.DXGI.Format.B8G8R8A8_UNorm, SharpDX.DXGI.SwapChainFlags.None);
            }

            // Otherwise, create a new one.
            else
            {
                // SwapChain description
                var desc = CreateSwapChainDescription();

                // Once the desired swap chain description is configured, it must be created on the same adapter as our D3D Device

                // First, retrieve the underlying DXGI Device from the D3D Device.
                // Creates the swap chain
                using (var dxgiDevice2 = d3dDevice.QueryInterface<SharpDX.DXGI.Device2>())
                using (var dxgiAdapter = dxgiDevice2.Adapter)
                using (var dxgiFactory2 = dxgiAdapter.GetParent<SharpDX.DXGI.Factory2>())
                {
                    swapChain = ToDispose(CreateSwapChain(dxgiFactory2, d3dDevice, desc));

                    // Ensure that DXGI does not queue more than one frame at a time. This both reduces
                    // latency and ensures that the application will only render after each VSync, minimizing
                    // power consumption.
                    dxgiDevice2.MaximumFrameLatency = 1;
                }
            }

            // Obtain the backbuffer for this window which will be the final 3D rendertarget.
            backBuffer = ToDispose(SharpDX.Direct3D11.Texture2D.FromSwapChain<SharpDX.Direct3D11.Texture2D>(swapChain, 0));
            {
                RenderTargetViewDescription rtvDescription = new RenderTargetViewDescription()
                {
                    Dimension = RenderTargetViewDimension.Texture2DArray,
                    Format = SharpDX.DXGI.Format.B8G8R8A8_UNorm,
                    Texture2DArray = new RenderTargetViewDescription.Texture2DArrayResource() { MipSlice = 0, FirstArraySlice = 0, ArraySize = 1 }
                };

                // Create a view interface on the rendertarget to use on bind.
                renderTargetView = ToDispose(new SharpDX.Direct3D11.RenderTargetView(d3dDevice, BackBuffer, rtvDescription));

                if (IsStereoEnabled)
                {
                    RenderTargetViewDescription rtvDescriptionRight = new RenderTargetViewDescription()
                    {
                        Dimension = RenderTargetViewDimension.Texture2DArray,
                        Format = SharpDX.DXGI.Format.B8G8R8A8_UNorm,
                        Texture2DArray = new RenderTargetViewDescription.Texture2DArrayResource() { MipSlice = 0, FirstArraySlice = 1, ArraySize = 1 }
                    };

                    renderTargetViewRight = ToDispose(new SharpDX.Direct3D11.RenderTargetView(d3dDevice, BackBuffer, rtvDescriptionRight));
                }

                // Cache the rendertarget dimensions in our helper class for convenient use.
                var backBufferDesc = BackBuffer.Description;
                RenderTargetBounds = new Windows.Foundation.Rect(0, 0, backBufferDesc.Width, backBufferDesc.Height);
            }

            // Create a descriptor for the depth/stencil buffer.
            // Allocate a 2-D surface as the depth/stencil buffer.
            // Create a DepthStencil view on this surface to use on bind.
            using (var depthBuffer = new SharpDX.Direct3D11.Texture2D(d3dDevice, new SharpDX.Direct3D11.Texture2DDescription()
            {
                Format = SharpDX.DXGI.Format.D24_UNorm_S8_UInt,
                ArraySize = 1,
                MipLevels = 1,
                Width = (int)RenderTargetSize.Width,
                Height = (int)RenderTargetSize.Height,
                SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                BindFlags = SharpDX.Direct3D11.BindFlags.DepthStencil,
            }))
                depthStencilView = ToDispose(new SharpDX.Direct3D11.DepthStencilView(d3dDevice, depthBuffer,
                    new SharpDX.Direct3D11.DepthStencilViewDescription() { Dimension = SharpDX.Direct3D11.DepthStencilViewDimension.Texture2D }));

            // Create a viewport descriptor of the full window size.
            var viewport = new SharpDX.ViewportF((float)RenderTargetBounds.X, (float)RenderTargetBounds.Y, (float)RenderTargetBounds.Width, (float)RenderTargetBounds.Height, 0.0f, 1.0f);

            // Set the current viewport using the descriptor.
            d3dContext.Rasterizer.SetViewport(viewport);

            // Now we set up the Direct2D render target bitmap linked to the swapchain.
            // Whenever we render to this bitmap, it will be directly rendered to the
            // swapchain associated with the window.
            var bitmapProperties = new SharpDX.Direct2D1.BitmapProperties1(
                new SharpDX.Direct2D1.PixelFormat(SharpDX.DXGI.Format.B8G8R8A8_UNorm, SharpDX.Direct2D1.AlphaMode.Premultiplied),
                deviceManager.Dpi,
                deviceManager.Dpi,
                SharpDX.Direct2D1.BitmapOptions.Target | SharpDX.Direct2D1.BitmapOptions.CannotDraw);

            // Direct2D needs the dxgi version of the backbuffer surface pointer.
            // Get a D2D surface from the DXGI back buffer to use as the D2D render target.
            if (IsStereoEnabled)
                using (var dxgiBackBuffer = swapChain.GetBackBuffer<SharpDX.DXGI.Resource1>(0))
                {
                    using (var dxgiSurface = new Surface2(dxgiBackBuffer, 0))
                        bitmapTarget = ToDispose(new SharpDX.Direct2D1.Bitmap1(d2dContext, dxgiSurface, bitmapProperties));
                    using (var dxgiSurface = new Surface2(dxgiBackBuffer, 1))
                        bitmapTargetRight = ToDispose(new SharpDX.Direct2D1.Bitmap1(d2dContext, dxgiSurface, bitmapProperties));
                }
            else
                using (var dxgiBackBuffer = swapChain.GetBackBuffer<SharpDX.DXGI.Surface2>(0))
                    bitmapTarget = ToDispose(new SharpDX.Direct2D1.Bitmap1(d2dContext, dxgiBackBuffer, bitmapProperties));
            // So now we can set the Direct2D render target.
            d2dContext.Target = BitmapTarget;

            // Set D2D text anti-alias mode to Grayscale to ensure proper rendering of text on intermediate surfaces.
            d2dContext.TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode.Grayscale;
        }

        public override void Close()
        {
            if (!deviceManager.Settings.IsWindowed)
            {
                SwapChain.SetFullscreenState(false, null);
            }
            deviceManager.Dispose();
            Dispose();
        }

        /// <summary>
        /// Creates the swap chain description.
        /// </summary>
        /// <returns>A swap chain description</returns>
        /// <remarks>
        /// This method can be overloaded in order to modify default parameters.
        /// </remarks>
        protected virtual SharpDX.DXGI.SwapChainDescription1 CreateSwapChainDescription()
        {
            // SwapChain description
            var desc = new SharpDX.DXGI.SwapChainDescription1()
            {
                // Automatic sizing
                Width = Width,
                Height = Height,
                Format = SharpDX.DXGI.Format.B8G8R8A8_UNorm,
                Stereo = deviceManager.Settings.IsStereo,
                SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                Usage = SharpDX.DXGI.Usage.BackBuffer | SharpDX.DXGI.Usage.RenderTargetOutput,

                // Use two buffers to enable flip effect.
                BufferCount = 2 ,
                Scaling = SharpDX.DXGI.Scaling.None,
                SwapEffect = SharpDX.DXGI.SwapEffect.FlipSequential,
            };
            return desc;
        }

        /// <summary>
        /// Creates the swap chain.
        /// </summary>
        /// <param name="factory">The DXGI factory</param>
        /// <param name="device">The D3D11 device</param>
        /// <param name="desc">The swap chain description</param>
        /// <returns>An instance of swap chain</returns>
        protected abstract SharpDX.DXGI.SwapChain1 CreateSwapChain(SharpDX.DXGI.Factory2 factory, SharpDX.Direct3D11.Device1 device, SharpDX.DXGI.SwapChainDescription1 desc);



    }
}
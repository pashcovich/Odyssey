#region Using Directives

using Odyssey.Graphics;
using Odyssey.Text.Logging;
using SharpDX;
using SharpDX.DXGI;
using System;
using System.Windows.Forms;
using SharpDX.Mathematics.Interop;
using Device = SharpDX.Direct3D11.Device;
using Texture2D = SharpDX.Direct3D11.Texture2D;

#endregion Using Directives

namespace Odyssey.Engine
{
    public class DesktopPresenter : SwapChainGraphicsPresenter
    {
        public DesktopPresenter(DirectXDevice device, PresentationParameters presentationParameters)
            : base(device, presentationParameters)
        {
        }

        public override bool IsFullScreen
        {
            get { return SwapChain.IsFullScreen; }

            set
            {
                if (SwapChain == null)
                    return;

                var outputIndex = PreferredFullScreenOutputIndex;

                // no outputs connected to the current graphics adapter
                var output = DirectXDevice.Adapter == null || DirectXDevice.Adapter.OutputsCount == 0
                    ? null
                    : DirectXDevice.Adapter.GetOutputAt(outputIndex);

                Output currentOutput = null;

                try
                {
                    RawBool isCurrentlyFullscreen;
                    SwapChain.GetFullscreenState(out isCurrentlyFullscreen, out currentOutput);

                    // check if the current fullscreen monitor is the same as new one
                    if (isCurrentlyFullscreen == value && output != null && currentOutput != null &&
                        currentOutput.NativePointer == ((Output)output).NativePointer)
                        return;
                }
                finally
                {
                    if (currentOutput != null)
                        currentOutput.Dispose();
                }

                bool switchToFullScreen = value;
                // If going to fullscreen mode: call 1) SwapChain.ResizeTarget 2) SwapChain.IsFullScreen
                var description = new ModeDescription(BackBuffer.Width, BackBuffer.Height, Description.RefreshRate,
                    Description.BackBufferFormat);
                if (switchToFullScreen)
                {
                    Description.IsFullScreen = true;
                    // Destroy and recreate the full swapchain in case of fullscreen switch
                    // It seems to be more reliable then trying to change the current swap-chain.
                    var backBuffer = BackBuffer;
                    var swapChain = SwapChain;
                    RemoveAndDispose(ref backBuffer);
                    RemoveAndDispose(ref swapChain);

                    swapChain = CreateSwapChain();
                    BackBuffer = ToDispose(RenderTarget2D.New(DirectXDevice, swapChain.GetBackBuffer<Texture2D>(0)));
                }
                else
                {
                    Description.IsFullScreen = false;
                    SwapChain.IsFullScreen = false;

                    // call 1) SwapChain.IsFullScreen 2) SwapChain.Resize
                    Resize(BackBuffer.Width, BackBuffer.Height, BackBuffer.Format);
                }

                // If going to window mode:
                if (!switchToFullScreen)
                {
                    // call 1) SwapChain.IsFullScreen 2) SwapChain.Resize
                    description.RefreshRate = new Rational(0, 0);
                    SwapChain.ResizeTarget(ref description);
                }
            }
        }

        protected override SwapChain CreateSwapChainResources()
        {
            IntPtr? handle = null;
            var control = Description.DeviceWindowHandle as Control;
            if (control != null) handle = control.Handle;
            else if (Description.DeviceWindowHandle is IntPtr) handle = (IntPtr)Description.DeviceWindowHandle;

            if (!handle.HasValue)
            {
                throw new NotSupportedException(
                    string.Format("DeviceWindowHandle of type [{0}] is not supported. Only System.Windows.Control or IntPtr are supported",
                        Description.DeviceWindowHandle != null ? Description.DeviceWindowHandle.GetType().Name : "null"));
            }

            int requestedMultiSampleCount = (int)Description.MultiSampleCount;
            int multiSampleCount = Math.Min((int)DirectXDevice.Features[Description.BackBufferFormat].MSAALevelMax, requestedMultiSampleCount);
            if (multiSampleCount < (int)Description.MultiSampleCount)
                LogEvent.Engine.Warning(string.Format("Requested MultiSampleCount of {0} not supported, using {1} instead", requestedMultiSampleCount, multiSampleCount));

            BufferCount = 1;
            var description = new SwapChainDescription
            {
                ModeDescription = new ModeDescription(Description.BackBufferWidth, Description.BackBufferHeight, Description.RefreshRate,
                        Description.BackBufferFormat),
                BufferCount = BufferCount, // TODO: Do we really need this to be configurable by the user?
                OutputHandle = handle.Value,
                SampleDescription = new SampleDescription(multiSampleCount, 0),
                SwapEffect = SwapEffect.Discard,
                Usage = Description.RenderTargetUsage,
                IsWindowed = true,
                Flags = Description.Flags,
            };

            var newSwapChain = new SwapChain(GraphicsAdapter.Factory, (Device)DirectXDevice, description);
            if (Description.IsFullScreen)
            {
                // Before fullscreen switch
                newSwapChain.ResizeTarget(ref description.ModeDescription);

                // Switch to full screen
                newSwapChain.IsFullScreen = true;

                // This is really important to call ResizeBuffers AFTER switching to IsFullScreen
                newSwapChain.ResizeBuffers(BufferCount, Description.BackBufferWidth, Description.BackBufferHeight,
                    Description.BackBufferFormat, SwapChainFlags.AllowModeSwitch);
            }

            return newSwapChain;
        }
    }
}
#region Using Directives
using Odyssey.Graphics;
using Odyssey.Text.Logging;
using SharpDX.DXGI;
using System;
using System.Windows.Forms;
using SharpDX.Mathematics.Interop;
using Device = SharpDX.Direct3D11.Device;
using Texture2D = SharpDX.Direct3D11.Texture2D;
#endregion Using Directives

namespace Odyssey.Engine
{
    public sealed class DesktopPresenter : MonoSwapChainPresenter
    {
        public DesktopPresenter(DirectXDevice device, PresentationParameters presentationParameters)
            : base(device, presentationParameters)
        {
            var rawBackbuffer = SwapChain.GetBackBuffer<Texture2D>(0);
            BackBuffer = ToDispose(RenderTarget2D.New(device, rawBackbuffer));
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
            var handle = FindHandle(Description.DeviceWindowHandle);

            int requestedMultiSampleCount = (int) Description.MultiSampleCount;
            int multiSampleCount = Math.Min((int) DirectXDevice.Features[Description.BackBufferFormat].MSAALevelMax, requestedMultiSampleCount);
            if (multiSampleCount < (int) Description.MultiSampleCount)
                LogEvent.Engine.Warning($"Requested MultiSampleCount of {requestedMultiSampleCount} not supported, using {multiSampleCount} instead");
           
            BufferCount = 1;

            var description = new SwapChainDescription
            {
                ModeDescription = new ModeDescription(Description.BackBufferWidth, Description.BackBufferHeight, Description.RefreshRate,
                    Description.BackBufferFormat),
                BufferCount = BufferCount,
                OutputHandle = handle.Value,
                SampleDescription = new SampleDescription(multiSampleCount, 0),
                SwapEffect = SwapEffect.Discard,
                Usage = Description.RenderTargetUsage,
                IsWindowed = true,
                Flags = Description.Flags,
            };
            var swapChain = new SwapChain(GraphicsAdapter.Factory, (Device) DirectXDevice, description);

            if (!Description.IsFullScreen) return swapChain;

            // Before fullscreen switch
            swapChain.ResizeTarget(ref description.ModeDescription);

            // Switch to full screen
            swapChain.IsFullScreen = true;

            // This is really important to call ResizeBuffers AFTER switching to IsFullScreen
            swapChain.ResizeBuffers(BufferCount, Description.BackBufferWidth, Description.BackBufferHeight,
                Description.BackBufferFormat, SwapChainFlags.AllowModeSwitch);
            return swapChain;
        }

        internal static IntPtr? FindHandle(object deviceWindowHandle)
        {
            IntPtr? handle = null;
            var control = deviceWindowHandle as Control;
            if (control != null) handle = control.Handle;
            else if (deviceWindowHandle is IntPtr) handle = (IntPtr)deviceWindowHandle;

            if (!handle.HasValue)
            {
                throw new NotSupportedException(
                    $"DeviceWindowHandle of type [{deviceWindowHandle?.GetType().Name ?? "null"}] is not supported. Only System.Windows.Control or IntPtr are supported");
            }

            return handle;
        }
    }
}
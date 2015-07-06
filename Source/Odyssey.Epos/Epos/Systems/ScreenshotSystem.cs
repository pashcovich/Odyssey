using System.Linq;
using Odyssey.Engine;
using Odyssey.Epos.Components;
using Odyssey.Epos.Messages;
using Odyssey.Graphics;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace Odyssey.Epos.Systems
{
    public sealed class ScreenshotSystem : UpdateableSystemBase, ITarget
    {
        private RenderTarget2D renderTarget;
        private DepthStencilBuffer depthStencil;

        public ScreenshotSystem() : base(Selector.All(typeof (ScreenshotComponent)))
        {
            IsEnabled = false;
        }

        public override void Start()
        {
            Messenger.Register<ScreenshotMessage>(this);
        }

        public override void Stop()
        {
            Messenger.Unregister<ScreenshotMessage>(this);
        }

        public override void Process(ITimeService time)
        {
            IsEnabled = false;
            var device = Services.GetService<IGraphicsDeviceService>().DirectXDevice;
            var cScreenshot = Entities.First().GetComponent<ScreenshotComponent>();
            var backBuffer = device.BackBuffer;
            var textureDescription = new Texture2DDescription()
            {
                CpuAccessFlags = CpuAccessFlags.None,
                Format = backBuffer.Format.Value,
                Height = backBuffer.Height,
                Usage = ResourceUsage.Default,
                Width = backBuffer.Width,
                ArraySize = 1,
                SampleDescription = new SampleDescription(1, 0),
                BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                MipLevels = 1,
                OptionFlags = ResourceOptionFlags.None
            };

            renderTarget = RenderTarget2D.New(device, textureDescription);

            var depthStencilDesc = new Texture2DDescription()
            {
                ArraySize = 1,
                MipLevels = 1,
                BindFlags = BindFlags.DepthStencil,
                Format = device.DepthStencilBuffer.Format.Value,
                Width = textureDescription.Width,
                Height = textureDescription.Height,
                SampleDescription = textureDescription.SampleDescription,
                CpuAccessFlags = CpuAccessFlags.None,
                Usage = ResourceUsage.Default,
                OptionFlags = ResourceOptionFlags.None
            };
            depthStencil = DepthStencilBuffer.New(device, depthStencilDesc);

            
            ((DeviceContext)device).ResolveSubresource(backBuffer, 0, renderTarget, 0, textureDescription.Format);
            renderTarget.Initialize();
            depthStencil.Initialize();

            device.SetRenderTargets(depthStencil, renderTarget);
            renderTarget.Save("test.png", ImageFileType.Png);

            renderTarget.Dispose();
            depthStencil.Dispose();
            IsEnabled = false;
        }

        protected override void OnBlockingMessageReceived(MessageEventArgs args)
        {
            base.OnBlockingMessageReceived(args);
            while (MessageQueue.HasItems<ScreenshotMessage>())
            {
                var mScreenshot = MessageQueue.Dequeue<ScreenshotMessage>();
                if (mScreenshot != null)
                {
                    IsEnabled = true;
                }
            }
        }

        DepthStencilView ITarget.DepthStencilBuffer
        {
            get { return depthStencil; }
        }

        RenderTargetView ITarget.RenderTarget
        {
            get { return renderTarget; }
        }
    }
}

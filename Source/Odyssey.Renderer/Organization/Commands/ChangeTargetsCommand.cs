using Odyssey.Graphics;
using Odyssey.Graphics.Organization;
using SharpDX.Mathematics;
using SharpDX.Direct3D11;

namespace Odyssey.Organization.Commands
{
    public class ChangeTargetsCommand : EngineCommand, ITarget
    {
        public ITarget Target { get; set; }

        public ChangeTargetsCommand(IServiceRegistry services) : base(services, CommandType.Engine) {}

        public override void Initialize()
        {
            var device = DeviceService.DirectXDevice;
            DepthStencilBuffer = Target.DepthStencilBuffer ?? device.DepthStencilBuffer;
            RenderTarget = Target.RenderTarget ?? device.BackBuffer;
        }

        public override void Execute()
        {
            DeviceService.DirectXDevice.SetRenderTargets(DepthStencilBuffer, RenderTarget);
        }

        public DepthStencilView DepthStencilBuffer { get; private set; }

        public RenderTargetView RenderTarget { get; private set; }
    }
}

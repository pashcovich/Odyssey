using Odyssey.Core;
using Odyssey.Graphics;
using Odyssey.Graphics.Organization;
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
            DepthStencilBuffer = Target == null ? device.DepthStencilBuffer : Target.DepthStencilBuffer;
            RenderTarget = Target == null ? device.BackBuffer : Target.RenderTarget;
        }

        public override void Execute()
        {
            DeviceService.DirectXDevice.SetRenderTargets(DepthStencilBuffer, RenderTarget);
        }

        public DepthStencilView DepthStencilBuffer { get; private set; }

        public RenderTargetView RenderTarget { get; private set; }
    }
}

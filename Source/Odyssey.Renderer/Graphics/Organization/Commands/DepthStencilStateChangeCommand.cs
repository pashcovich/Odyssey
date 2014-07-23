using SharpDX;

namespace Odyssey.Graphics.Organization.Commands
{
    public class DepthStencilStateChangeCommand: Command
    {
        private readonly DepthStencilState depthStencilState;

        public DepthStencilState DepthStencilState
        {
            get { return depthStencilState; }
        }

        public DepthStencilStateChangeCommand(IServiceRegistry services, DepthStencilState depthStencilState)
            : base(services, CommandType.DepthStencilStateChange)
        {
            this.depthStencilState = ToDispose(depthStencilState);
        }

        public override void Initialize()
        {
            depthStencilState.Initialize();
            IsInited = true;
        }

        public override void Execute()
        {
            DeviceService.DirectXDevice.SetDepthStencilState(depthStencilState);
        }

        
    }
}

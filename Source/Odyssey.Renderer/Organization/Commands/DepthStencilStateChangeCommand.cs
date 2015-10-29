using Odyssey.Core;
using Odyssey.Graphics;
using Odyssey.Graphics.Organization;

namespace Odyssey.Organization.Commands
{
    public class DepthStencilStateChangeCommand : EngineCommand
    {
        private readonly DepthStencilState depthStencilState;

        public DepthStencilState DepthStencilState
        {
            get { return depthStencilState; }
        }

        public DepthStencilStateChangeCommand(IServiceRegistry services, DepthStencilState depthStencilState)
            : base(services, CommandType.ChangeDepthStencilState)
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

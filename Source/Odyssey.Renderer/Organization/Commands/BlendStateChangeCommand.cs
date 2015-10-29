using Odyssey.Core;
using Odyssey.Graphics;
using Odyssey.Graphics.Organization;

namespace Odyssey.Organization.Commands
{
    public class BlendStateChangeCommand : EngineCommand
    {
        private readonly BlendState blendState;

        public BlendState BlendState => blendState;

        public BlendStateChangeCommand(IServiceRegistry services, BlendState blendState)
            : base(services, CommandType.ChangeBlendState)
        {
            this.blendState = ToDispose(blendState);
        }

        public override void Initialize()
        {
            blendState.Initialize();
            IsInited = true;
        }

        public override void Execute()
        {
            DeviceService.DirectXDevice.SetBlendState(blendState);
        }
       
    }
}

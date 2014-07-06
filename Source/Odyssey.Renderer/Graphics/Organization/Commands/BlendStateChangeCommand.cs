using SharpDX;

namespace Odyssey.Graphics.Organization.Commands
{
    public class BlendStateChangeCommand : Command
    {
        private readonly BlendState blendState;

        public BlendState BlendState
        {
            get { return blendState; }
        }

        public BlendStateChangeCommand(IServiceRegistry services, BlendState blendState)
            : base(services, CommandType.BlendStateChange)
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

        public override void Unload()
        {
            if (blendState!=null)
                blendState.Dispose();
            IsInited = false;
        }

       
    }
}

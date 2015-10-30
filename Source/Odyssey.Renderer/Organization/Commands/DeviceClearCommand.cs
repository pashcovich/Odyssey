using Odyssey.Core;
using Odyssey.Engine;
using Odyssey.Graphics.Organization;
using SharpDX.Mathematics;

namespace Odyssey.Organization.Commands
{
    public class DeviceClearCommand:EngineCommand
    {
        private readonly DirectXDevice device;

        public DeviceClearCommand(IServiceRegistry services) : base(services, CommandType.DeviceClear)
        {
            device = services.GetService<IGraphicsDeviceService>().DirectXDevice;
        }

        public override void Initialize()
        {
            IsInited = true;
        }

        public override void Execute()
        {
            device.Clear(Color.Black);
        }
    }
}

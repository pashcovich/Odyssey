using Odyssey.Core;
using Odyssey.Engine;
using Odyssey.Graphics.Organization;

namespace Odyssey.Organization.Commands
{
    public class AlternateStereoRenderingCommand : EngineCommand
    {
        private readonly IStereoCamera camera;
        private readonly StereoSwapChainPresenter stereoPresenter;

        public AlternateStereoRenderingCommand(IServiceRegistry services) : base(services, CommandType.AlternateStereoRendering)
        {
            camera = (IStereoCamera) Services.GetService<ICameraService>().MainCamera;
            var device = Services.GetService<IGraphicsDeviceService>().DirectXDevice;
            stereoPresenter = (StereoSwapChainPresenter)device.Presenter;
        }

        public override void Initialize()
        {
            IsInited = true;
        }

        public override void Execute()
        {
            camera.AlternateChannel();
            stereoPresenter.AlternateTargets();
        }
    }
}

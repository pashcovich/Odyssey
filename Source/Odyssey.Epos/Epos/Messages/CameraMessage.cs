using Odyssey.Engine;

namespace Odyssey.Epos.Messages
{
    public class CameraMessage : EntityChangeMessage
    {
        public ICamera Camera { get; private set; }

        public CameraMessage(Entity source, ICamera camera, UpdateType action, bool isSynchronous = false) : base(source, action, isSynchronous)
        {
            Camera = camera;
        }
    }
}

using Odyssey.Content;
using Odyssey.Core;
using Odyssey.Engine;
using Odyssey.Graphics;
using SharpDX.Mathematics;

namespace Odyssey.Epos.Components
{
    public sealed class StereoCameraComponent : CameraComponent, IStereoCamera    {
        private StereoChannel currentChannel;

        StereoChannel IStereoCamera.CurrentChannel => currentChannel;
        public Matrix ProjectionLeft { get; set; }
        public Matrix ProjectionRight { get; set; }

        public StereoCameraComponent() : base(ComponentTypeManager.GetType<CameraComponent>())
        {
            Type = CameraType.PerspectiveStereo;

        }

        public void AlternateChannel()
        {
            if (currentChannel == StereoChannel.Left)
            {
                Projection = ProjectionRight;
                currentChannel = StereoChannel.Right;
            }
            else
            {
                Projection = ProjectionLeft;
                currentChannel = StereoChannel.Left;
            }
        }
    }
}

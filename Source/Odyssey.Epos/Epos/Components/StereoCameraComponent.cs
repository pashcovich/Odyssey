using Odyssey.Core;
using Odyssey.Engine;
using Odyssey.Graphics;
using SharpDX.Mathematics;

namespace Odyssey.Epos.Components
{
    public class StereoCameraComponent : CameraComponent
    {
        internal StereoChannel CurrentChanne { get; set; }
        public Matrix ProjectionLeft { get; set; }
        public Matrix ProjectionRight { get; set; }

        public StereoCameraComponent() : base(ComponentTypeManager.GetType<StereoCameraComponent>())
        {
            Type = CameraType.Stereo;
            var deviceSettings = Services.GetService<IDirectXDeviceSettings>();
            float ar = deviceSettings.PreferredBackBufferWidth/(float)deviceSettings.PreferredBackBufferHeight;
            ProjectionLeft = StereoHelper.StereoProjectionFovRH(new StereoParameters(), FieldOfView, ar, NearClip, FarClip, StereoChannel.Left);
            ProjectionRight = StereoHelper.StereoProjectionFovRH(new StereoParameters(), FieldOfView, ar, NearClip, FarClip, StereoChannel.Right);
        }

        

    }
}

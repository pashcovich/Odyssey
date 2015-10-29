using Odyssey.Graphics;
using SharpDX.Mathematics;

namespace Odyssey.Engine
{
    public interface ICamera
    {
        int Index { get; }
        Vector3 Position { get; }
        Matrix View {get;}
        Matrix Projection { get; }
        ViewportF Viewport { get; }
        float NearClip { get; }
        float FarClip { get; }
        float FieldOfView { get; }
        bool Changed { get;}
    }

    public interface IStereoCamera : ICamera
    {
        Matrix ProjectionLeft { get; }
        Matrix ProjectionRight { get; }
        StereoChannel CurrentChannel { get; }
        void AlternateChannel();
    }
}

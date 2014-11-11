using SharpDX.Mathematics;

namespace Odyssey.Engine
{
    public interface ICamera
    {
        int Index { get; }
        Matrix View {get;}
        Matrix Projection { get; }
        ViewportF Viewport { get; }
    }

    public interface IStereoCamera : ICamera
    {
        Matrix LeftProjection { get; }
        Matrix RightProjection { get; }
    }
}

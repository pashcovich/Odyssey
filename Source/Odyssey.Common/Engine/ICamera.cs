using SharpDX;

namespace Odyssey.Engine
{
    public interface ICamera
    {
        int Id { get; }
        Matrix World {get;}
        Matrix View {get;}
        Matrix Projection { get; }
        Matrix WorldViewProjection { get; }
        ViewportF Viewport { get; }
    }

    public interface IStereoCamera : ICamera
    {
        Matrix LeftProjection { get; }
        Matrix RightProjection { get; }
    }
}

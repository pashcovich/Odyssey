#if !ODYSSEY_ENGINE
using SharpDX;

namespace Odyssey.Engine
{
    public interface ICameraProvider
    {
        Matrix View {get;}
        Matrix Projection { get; }
        Viewport Viewport { get; }

    }
}
#endif
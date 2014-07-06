using SharpDX;

namespace Odyssey.Graphics.Shaders
{
    public interface IParameter
    {
        int Index { get; }
        int Size { get; }
        string ParamHandle { get; }
        Vector4[] ToArray();
    }


}

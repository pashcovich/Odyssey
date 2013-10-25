using Odyssey.Graphics.Meshes;
namespace Odyssey.Graphics.Effects
{
    public interface IParameter
    {
        string ParamHandle { get; }
        SharpDX.Vector4[] ToArray();
        object Value { get; }
    }

    public interface IInstanceParameter : IParameter
    {
        void Apply(IInstance instance);
    }
}

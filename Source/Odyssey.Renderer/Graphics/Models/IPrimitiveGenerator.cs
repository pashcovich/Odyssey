using Odyssey.Graphics.Meshes;

namespace Odyssey.Graphics.Models
{
    public interface IPrimitiveGenerator<TVertex>
        where TVertex : struct
    {
        void GenerateMesh(out TVertex[] vertices, out int[] indices);
    }
}
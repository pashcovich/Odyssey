using Odyssey.Engine;

namespace Odyssey.Graphics.Models
{
    public interface IModelMesh
    {
        void DrawUnindexed(DirectXDevice device);
        void DrawIndexed(DirectXDevice device);
        void DrawIndexedInstanced(DirectXDevice device, int instanceCount);
    }
}
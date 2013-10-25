#if !ODYSSEY_ENGINE
using Odyssey.Engine;

namespace Odyssey.Graphics
{
    public interface IRenderable
    {
        void Initialize(IDirectXProvider directX);
        void Render(IDirectXTarget target);
    }
}
#endif

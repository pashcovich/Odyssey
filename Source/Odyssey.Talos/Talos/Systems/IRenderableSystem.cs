using Odyssey.Engine;

namespace Odyssey.Talos.Systems
{
    public interface IRenderableSystem : ISystem
    {
        bool BeginRender();
        void Render(ITimeService service);
    }
}
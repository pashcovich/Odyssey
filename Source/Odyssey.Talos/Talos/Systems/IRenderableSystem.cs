namespace Odyssey.Talos.Systems
{
    public interface IRenderableSystem : ISystem
    {
        bool BeginRender();
        void Render();
    }
}
namespace Odyssey.Epos.Systems
{
    public interface IRenderableSystem : ISystem
    {
        bool BeginRender();
        void Render();
    }
}
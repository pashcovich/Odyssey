namespace Odyssey.Graphics.Organization
{
    public interface IRenderCommand
    {
        void PreRender();
        void Render();
        void PostRender();
    }
}

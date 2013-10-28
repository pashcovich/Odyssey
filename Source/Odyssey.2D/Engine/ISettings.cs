#if !ODYSSEY_ENGINE && DIRECTX11_1

namespace Odyssey.Engine
{
    public interface ISettings
    {
        int ScreenWidth { get; }
        int ScreenHeight { get; }
        float Dpi { get; }
    }
}
#endif
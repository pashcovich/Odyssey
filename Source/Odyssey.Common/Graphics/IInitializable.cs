namespace Odyssey.Graphics
{
    public interface IInitializable
    {
        bool IsInited { get; }
        void Initialize();
    }
}

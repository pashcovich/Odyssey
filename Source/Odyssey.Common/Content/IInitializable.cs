namespace Odyssey.Content
{
    public interface IInitializable
    {
        bool IsInited { get; }
        void Initialize();
    }
}

namespace Odyssey.Graphics.Organization
{
    public interface ICommand
    {
        string Name { get; }
        CommandType Type { get; }
        void Initialize();
        void Execute();
        void Dispose();
    }

}
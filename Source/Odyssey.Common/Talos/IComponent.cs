namespace Odyssey.Talos
{
    public interface IComponent
    {
        string Name { get; }
        long KeyPart { get; }
        long Id { get; }
        bool Validate();
    }
}

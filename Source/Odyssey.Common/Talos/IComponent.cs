using Odyssey.Content;

namespace Odyssey.Epos
{
    public interface IComponent : IResource
    {
        long KeyPart { get; }
        long Id { get; }
        bool Validate();
    }
}

using Odyssey.Animation;

namespace Odyssey.Graphics
{
    public interface IResourceProvider
    {
        TResource GetResource<TResource>(string resourceName)
            where TResource : class, IResource;
    }
}
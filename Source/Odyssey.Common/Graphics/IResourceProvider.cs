using System.Collections;
using System.Collections.Generic;
using Odyssey.Animations;

namespace Odyssey.Graphics
{
    public interface IResourceProvider
    {
        bool ContainsResource(string resourceName);
        TResource GetResource<TResource>(string resourceName)
            where TResource : class, IResource;

        IEnumerable<IResource> Resources { get; }
    }
}
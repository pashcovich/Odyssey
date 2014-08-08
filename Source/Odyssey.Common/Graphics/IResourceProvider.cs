using System.Collections;
using System.Collections.Generic;
using Odyssey.Animation;

namespace Odyssey.Graphics
{
    public interface IResourceProvider
    {
        TResource GetResource<TResource>(string resourceName)
            where TResource : class, IResource;

        IEnumerable<IResource> Resources { get; }
    }
}
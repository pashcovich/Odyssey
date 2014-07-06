using System.IO;

namespace Odyssey.Content
{
    public interface IResourceResolver
    {
        Stream Resolve(string assetName);
    }
}

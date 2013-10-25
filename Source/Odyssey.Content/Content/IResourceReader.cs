using System;
using System.IO;

namespace Odyssey.Content
{
    public interface IResourceReader
    {
        object ReadContent(string resourceName, Stream stream);
    }
}

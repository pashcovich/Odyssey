using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Content
{
    public interface IResourceResolver
    {
        Stream Resolve(string assetName);
    }
}

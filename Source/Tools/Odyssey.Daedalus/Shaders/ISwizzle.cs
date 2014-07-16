using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Daedalus.Shaders
{
    public interface ISwizzle
    {
        bool HasSwizzle { get; }
        Swizzle[] Swizzle { get; }
        string PrintSwizzle();
    }
}

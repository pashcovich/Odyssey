using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Tools.ShaderGenerator.Shaders
{
    public interface ISwizzle
    {
        bool HasSwizzle { get; }
        Swizzle[] Swizzle { get; }
        string PrintSwizzle();
    }
}

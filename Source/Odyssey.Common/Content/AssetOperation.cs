using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Content
{
    [Flags]
    public enum AssetOperation
    {
        None = 0,
        Merge = 1 << 0,
    }
}

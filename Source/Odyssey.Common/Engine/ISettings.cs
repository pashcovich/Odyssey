using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Engine
{
    public interface ISettings
    {
        int ScreenWidth { get; }
        int ScreenHeight { get; }
        float Dpi { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Graphics.Rendering.Lights
{
    public enum LightRenderMode
    {
        Static,
        Realtime
    }

    public enum LightType
    {
        Point,
        Spotlight,
        Directional,
        Area
    }

    public enum ShadowAlgorithm
    {
        None,
        Hard,
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Talos.Components
{
    public class ScreenshotComponent : Component
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public bool RenderPostProcessEffects { get; set; }

        public ScreenshotComponent() : base(ComponentTypeManager.GetType<ScreenshotComponent>())
        {
            RenderPostProcessEffects = true;
        }
    }
}

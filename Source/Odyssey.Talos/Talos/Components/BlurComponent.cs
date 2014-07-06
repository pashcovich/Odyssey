using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Graphics.PostProcessing;

namespace Odyssey.Talos.Components
{
    public class BlurComponent : Component
    {
        public BlurComponent() : base(ComponentTypeManager.GetType<BlurComponent>())
        {
            SampleCount = Blur.SampleCount;
        }

        public float SampleCount { get; private set; }
        public float BlurAmount { get; set; }
        public float DownScale { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;

namespace Odyssey.Talos.Components
{
    public class BloomComponent : Component
    {
        public float Threshold { get; set; }
        public float Intensity { get; set; }
        public float BaseIntensity { get; set; }
        public float Saturation { get; set; }
        public float BaseSaturation { get; set; }

        public BloomComponent() : base(ComponentTypeManager.GetType<BloomComponent>())
        {
        }

        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Talos.Components
{
    public class GlowComponent : Component
    {
        public float GlowStrength { get; set; }

        public GlowComponent()
            : base(ComponentTypeManager.GetType<GlowComponent>())
        {
        }
    }
}

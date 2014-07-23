using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Graphics.Shaders;
using SharpDX;

namespace Odyssey.Talos.Initializers
{
    public delegate bool SelectorDelegate(ConstantBufferDescription cb);

    public class InitializerParameters
    {
        public Technique Technique { get; private set; }
        public IServiceRegistry Services { get; private set; }
        public long EntityId { get; private set; }
        public SelectorDelegate Selector { get; private set; }

        public InitializerParameters(long entityId, Technique technique, IServiceRegistry services, SelectorDelegate selector)
        {
            EntityId = entityId;
            Technique = technique;
            Services = services;
            Selector = selector;
        }
    }
}

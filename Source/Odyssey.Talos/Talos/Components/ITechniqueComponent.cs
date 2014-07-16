using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Graphics.Shaders;

namespace Odyssey.Talos.Components
{
    public interface ITechniqueComponent : IComponent
    {
        IEnumerable<Technique> Techniques { get; }
    }
}

using System.Collections.Generic;
using Odyssey.Graphics.Shaders;

namespace Odyssey.Talos.Components
{
    public interface ITechniqueComponent : IComponent
    {
        IEnumerable<Technique> Techniques { get; }
    }
}

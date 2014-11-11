using System.Collections.Generic;
using Odyssey.Graphics.Shaders;

namespace Odyssey.Epos.Components
{
    public interface ITechniqueComponent : IComponent
    {
        IEnumerable<Technique> Techniques { get; }
    }
}

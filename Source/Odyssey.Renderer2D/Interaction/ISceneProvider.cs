#if !ODYSSEY_ENGINE

using System.Collections.Generic;

namespace Odyssey.Interaction
{
    public interface ISceneProvider
    {
        IEnumerable<IInteractive3D> Items { get; }
    }
}
#endif
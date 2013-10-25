using System.Collections.Generic;

namespace Odyssey.Interaction
{
    public interface IInteractiveItemsProvider
    {
        IEnumerable<IInteractive3D> Items { get; }
    }
}

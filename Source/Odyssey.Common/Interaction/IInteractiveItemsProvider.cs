#region Using Directives

using System.Collections.Generic;

#endregion Using Directives

namespace Odyssey.Interaction
{
    public interface IInteractiveItemsProvider
    {
        IEnumerable<IInteractive3D> Items { get; }
    }
}
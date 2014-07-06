using System.Collections.Generic;

namespace Odyssey.Renderer.Graphics.Rendering
{
    public interface ISceneItemsProvider
    {
        IEnumerable<IRenderableModel> Items { get; }
    }
}

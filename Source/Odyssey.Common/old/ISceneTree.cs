using Odyssey.Interaction;
using System.Collections.Generic;

namespace Odyssey.Renderer.Graphics.Rendering.SceneGraph
{
    public interface ISceneTree : IInteractiveItemsProvider, ISceneItemsProvider
    {
        SceneNode RootNode { get; }
        void UpdateAllNodes();
        IEnumerable<SceneNode> Nodes { get; }
    }
}

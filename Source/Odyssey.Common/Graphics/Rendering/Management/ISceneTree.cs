using Odyssey.Interaction;
using System;
using System.Collections.Generic;

namespace Odyssey.Graphics.Rendering.Management
{
    public interface ISceneTree : IInteractiveItemsProvider, ISceneItemsProvider
    {
        SceneNode RootNode { get; }
        void UpdateAllNodes();
        IEnumerable<SceneNode> Nodes { get; }
    }
}

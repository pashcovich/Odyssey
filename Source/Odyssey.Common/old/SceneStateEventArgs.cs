using Odyssey.Engine;
using System;

namespace Odyssey.Renderer.Graphics.Rendering
{
    public class SceneStateEventArgs : EventArgs
    {
        public IDirectXProvider DirectX { get; private set; }
        public IScene Scene { get; private set; }
        public ICamera ActiveCamera { get; private set; }

        public SceneStateEventArgs(IDirectXProvider directX, IScene scene, ICamera camera)
        {
            DirectX = directX;
            ActiveCamera = camera;
            Scene = scene;
        }
    }
}

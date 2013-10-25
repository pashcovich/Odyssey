using Odyssey.Engine;
using Odyssey.Graphics.Rendering.Lights;
using System;

namespace Odyssey.Graphics
{
    public interface IRenderer
    {
        void Initialize(InitializeDirectXEventArgs e);
        void Render(RenderEventArgs e);
    }
}
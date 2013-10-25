using Odyssey.Engine;
using Odyssey.UserInterface.Controls;
using System;

namespace Odyssey.Graphics.Rendering2D
{

    public interface IRenderer : IDisposable
    {
        Overlay Overlay { get; }

        bool IsInited { get; }

        void Initialize(InitializeDirectXEventArgs e);

        void Render(RenderEventArgs target);

    }
}
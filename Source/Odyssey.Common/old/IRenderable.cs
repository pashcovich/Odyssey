using Odyssey.Engine;
using Odyssey.Renderer.Graphics.Meshes;
using SharpDX;
using System;

namespace Odyssey.Graphics
{
    public interface IRenderable
    {
        void Initialize(IDirectXProvider directX);
        void Render(IDirectXTarget target);
        
    }

    public interface IRenderableModel : IRenderable, IDisposable, IInstance
    {
        string Name { get; }

        bool Inited { get; }

        bool CastsShadows { get; }

        bool Disposed { get; }

        bool IsVisible { get; set; }

        bool IsInViewFrustum();

        IMesh[] Meshes { get; }

        void Rotate(Vector3 axis, float angle);

        void Translate(Vector3 axis, float amount);


    }
}
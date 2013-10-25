using Odyssey.Engine;
using Odyssey.Graphics.Meshes;
using Odyssey.Graphics.Rendering.Management;
using SharpDX;
using System;

namespace Odyssey.Graphics
{
    public interface IRenderable
    {
        void Initialize(IDirectXProvider directX);
        void Render(IDirectXTarget target);
    }

    public interface IRenderableMesh : IRenderable, IDisposable, IInstance
    {
        string Name { get; }

        bool Inited { get; }

        bool IsVisible { get; set; }

        bool CastsShadows { get; }

        bool Disposed { get; }

        bool IsInViewFrustum();

        IGeometry Geometry { get; }

        void Rotate(Vector3 axis, float angle);

        void Translate(Vector3 axis, float amount);


    }
}
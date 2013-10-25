using Odyssey.Engine;
using Odyssey.UserInterface.Controls;
using SharpDX;
using SharpDX.Direct2D1;

namespace Odyssey.Graphics.Rendering2D
{
    public abstract class Renderer : Component, IRenderer, IDeviceDependentComponent
    {
        public bool IsInited { get; private set; }
        public Overlay Overlay { get; protected set; }
        public bool EnableClear { get; set; }
        public Color ClearColor { get; set; }

        public Renderer()
        {
            ClearColor = Color.Transparent;
        }

        public void Initialize(InitializeDirectXEventArgs e)
        {
            OnInitialize(e);
            EnableClear = true;
            IsInited = true;
        }

        protected abstract void OnInitialize(InitializeDirectXEventArgs e);

        public void Render(RenderEventArgs e)
        {
            IDirectXTarget target = e.Target;
            Begin(target);
            OnRender(target);
            End(target);
        }

        protected virtual void Begin(IDirectXTarget target)
        {
            DeviceContext d2dContext = target.Direct2D.Context;
            d2dContext.Target = target.BitmapTarget;
            d2dContext.BeginDraw();
            if (EnableClear)
                d2dContext.Clear(Color.Transparent);
        }

        protected abstract void OnRender(IDirectXTarget target);

        protected virtual void End(IDirectXTarget target)
        {
            target.Direct2D.Context.EndDraw();
        }

    }
}

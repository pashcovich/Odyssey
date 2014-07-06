using Odyssey.Engine;
using Odyssey.Renderer.Graphics;
using Odyssey.UserInterface.Controls;
using SharpDX;
using SharpDX.Direct2D1;

namespace Odyssey.Graphics.Rendering2D
{
    public abstract class Renderer : Component, IRenderer
    {
        private readonly Direct2DDevice device;

        internal DeviceContext Context { get { return device; } }

        protected Direct2DDevice Device { get { return device; } }

        public bool IsInited { get; private set; }

        public OverlayBase Overlay { get; protected set; }

        public bool EnableClear { get; set; }

        public Color ClearColor { get; set; }

        protected Renderer(Direct2DDevice device)
        {
            this.device = device;
            ClearColor = Color.Transparent;
        }

        public virtual void Initialize()
        {
            IsInited = true;
        }

        public virtual void Render()
        {
            Begin();
            Render();
            End();
        }

        protected virtual void Begin()
        {
            Context.Target = device.BackBuffer;
            Context.BeginDraw();
            if (EnableClear)
                Context.Clear(Color.Transparent);
        }

        protected virtual void End()
        {
            Context.EndDraw();
        }
    }
}
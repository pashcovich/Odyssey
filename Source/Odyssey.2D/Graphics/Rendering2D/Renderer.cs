using Odyssey.Engine;
using Odyssey.Graphics.Rendering;
using Odyssey.UserInterface.Controls;
using SharpDX;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Graphics.Rendering2D
{
    public abstract class Renderer : Component, IRenderer, IDisposable
    {
        public bool IsInited { get; private set;}
        public Overlay Overlay { get; protected set; }

        public void Initialize(InitializeDirectXEventArgs e)
        {
            OnInitialize(e);
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
            d2dContext.Clear(Color.Black);
        }

        protected abstract void OnRender(IDirectXTarget target);

        protected virtual void End(IDirectXTarget target)
        {
            target.Direct2D.Context.EndDraw();
        }
        
    }
}

using Odyssey.Engine;
using Odyssey.Graphics;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Phone.Input.Interop;

namespace Odyssey.WP8.Engine
{
    public class OdysseyInterop : IDrawingSurfaceManipulationHandler
    {
        DrawingSurfaceTarget target;
        IRenderer renderer;
        WP8DeviceSettings settings;

        public IRenderer Renderer
        {
            get
            {
                return renderer;
                
            }
            set
            {
                renderer = value;
                target.Initialize += (s, e) => renderer.Initialize(e);
            }
        }

        public OdysseyInterop(WP8DeviceSettings settings)
        {
            this.target = new DrawingSurfaceTarget(settings);
            this.settings = settings;
        }

        public object CreateContentProvider()
        {
            return new OdysseyContentProvider(this);
        }

        // IDrawingSurfaceManipulationHandler
        public void SetManipulationHost(Windows.Phone.Input.Interop.DrawingSurfaceManipulationHost manipulationHost)
        {
            manipulationHost.PointerPressed += OnPointerPressed;
            manipulationHost.PointerMoved += OnPointerMoved;
            manipulationHost.PointerReleased += OnPointerReleased;
        }

        // Event Handlers
        protected void OnPointerPressed(Windows.Phone.Input.Interop.DrawingSurfaceManipulationHost sender, Windows.UI.Core.PointerEventArgs args)
        {
            // Insert your code here.
        }

        protected void OnPointerMoved(Windows.Phone.Input.Interop.DrawingSurfaceManipulationHost sender, Windows.UI.Core.PointerEventArgs args)
        {
            // Insert your code here.
        }

        protected void OnPointerReleased(Windows.Phone.Input.Interop.DrawingSurfaceManipulationHost sender, Windows.UI.Core.PointerEventArgs args)
        {
            // Insert your code here.
        }

        internal void Connect()
        {
            Contract.Requires<NullReferenceException>(Renderer != null);
            target.Initialize += (s, e) => renderer.Initialize(e);
        }

        internal void Disconnect()
        {
            renderer.Dispose();
        }

        internal void UpdateForWindowSizeChange(float width, float height)
        {
            target.UpdateForWindowSizeChange(width, height);
        }

        internal void Render(Device device, DeviceContext context, RenderTargetView renderTargetView)
        {
            target.Update(device, context, renderTargetView);
            renderer.Render(new Odyssey.Engine.RenderEventArgs(target));
        }


    }
}

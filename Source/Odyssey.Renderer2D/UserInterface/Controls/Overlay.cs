#region Using Directives

using SharpDX;
using SharpDX.Direct2D1;

#endregion Using Directives

namespace Odyssey.UserInterface.Controls
{
    public class Overlay : OverlayBase
    {
        public Overlay(IServiceRegistry services)
            : base(services)
        {
        }

        public override void Display(bool clear = false)
        {
            Device.Target = Device.BackBuffer;

            DeviceContext context = Device;

            context.BeginDraw();
            if (clear)
                context.Clear(Color.Transparent);
            Render();
            context.EndDraw();
        }

        public override void Initialize()
        {
            foreach (UIElement element in Controls)
            {
                element.Initialize();
            }
            Layout();
        }
    }
}
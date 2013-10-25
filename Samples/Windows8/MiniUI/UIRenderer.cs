using Odyssey;
using Odyssey.Engine;
using Odyssey.Graphics.Rendering2D;
using Odyssey.UserInterface.Controls;
using Odyssey.UserInterface.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniUI
{
    public class UIRenderer : Renderer
    {

        protected override void OnInitialize(InitializeDirectXEventArgs e)
        {
            // define an overlay control: in this case it will cover the entine screen
            Overlay = ToDispose(Overlay.FromDescription(
               new OverlayDescription(width: e.Settings.ScreenWidth,
                   height: e.Settings.ScreenHeight)));

            // signal that we are starting to design the UI
            Overlay.BeginDesign();

            Panel panel = new Panel() { Position = new SharpDX.Vector2(128, 128), Width = 480, Height = 320 };
            Panel panel1 = new Panel() { Position = new SharpDX.Vector2(480, 64), Width = 256, Height = 200 };
            Label label = new Label() { Position = new SharpDX.Vector2(8, 8), Text = "I'm a label." };
            Button button = new Button() { Position = new SharpDX.Vector2(8, 128), Text = "Click me!" };

            button.Tap += (s, eventArgs) => button.Text = "It works!";
            
            panel.Add(label);
            panel.Add(button);
            Overlay.Add(panel);
            Overlay.Add(panel1);
            
            // we're done: BeginDesign() and EndDesign() are required for correct initialization
            Overlay.EndDesign(e.DirectX);
        }

        protected override void OnRender(IDirectXTarget e)
        {
            Overlay.Render(e);
        }
    }
}

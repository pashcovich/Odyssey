#region Using Directives

using Odyssey.Core;
using Odyssey.Engine;
using Odyssey.UserInterface.Controls;
using SharpDX;
using SharpDX.Mathematics;

#endregion Using Directives

namespace MiniUI
{
    public static class SampleOverlay
    {
        public static Overlay New(IServiceRegistry services)
        {
            var settings = services.GetService<IDirectXDeviceSettings>();
            var overlay = new Overlay(services)
            {
                Width = settings.PreferredBackBufferWidth,
                Height = settings.PreferredBackBufferHeight
            };

            // signal that we are starting to design the UI
            overlay.BeginDesign();

            var panel1 = new Canvas { Position = new Vector2(128, 128), Width = 480, Height = 320 };
            var panel2 = new Canvas { Position = new Vector2(480, 64), Width = 256, Height = 200 };
            var label = new TextBlock { Position = new Vector2(8, 8), Text = "I'm a label.", Width = 200, Height = 48 };
            var button = new Button { Position = new Vector2(16, 64), Content = new TextBlock { Text = "Button", TextStyleClass = "SmallCenter" } };
            var fpsCounter = new FpsCounter { Position = new Vector2(16, 16) };
            button.Tap += (s, eventArgs) => ((TextBlock)button.Content).Text = "It works!";
            var canvas = new Canvas();
            panel1.Add(label);
            canvas.Add(panel2);
            canvas.Add(panel1);
            canvas.Add(fpsCounter);
            canvas.Add(button);

            overlay.Content = canvas;

            // we're done: BeginDesign() and EndDesign() are required for correct initialization
            overlay.EndDesign();

            return overlay;
        }
    }
}
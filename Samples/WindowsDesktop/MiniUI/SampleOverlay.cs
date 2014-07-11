#region Using Directives

using Odyssey.Engine;
using Odyssey.UserInterface.Controls;
using SharpDX;

#endregion Using Directives

namespace MiniUI
{
    public class SampleOverlay : Overlay
    {
        protected SampleOverlay(IServiceRegistry services)
            : base(services)
        {
        }

        public static SampleOverlay New(IServiceRegistry services)
        {
            IDirectXDeviceSettings settings = services.GetService<IDirectXDeviceSettings>();
            var overlay = new SampleOverlay(services)
            {
                Width = settings.PreferredBackBufferWidth,
                Height = settings.PreferredBackBufferHeight
            };

            overlay.BeginDesign();

            // signal that we are starting to design the UI
            overlay.BeginDesign();

            Panel panel = new Panel() { Position = new Vector2(128, 128), Width = 480, Height = 320 };
            Panel panel1 = new Panel() { Position = new Vector2(480, 64), Width = 256, Height = 200 };
            Label label = new Label() { Position = new Vector2(8, 8), Text = "I'm a label.", Width = 200, Height = 48 };
            Button button = new Button() { Position = new Vector2(8, 128), Content = new Label() { Text = "Click me!", TextDescriptionClass = "Small" } };
            FpsCounter fpsCounter = new FpsCounter() { Position = new Vector2(16, 16) };
            button.Tap += (s, eventArgs) => ((Label)button.Content).Text = "It works!";

            panel.Add(label);
            panel.Add(button);
            overlay.Add(panel);
            overlay.Add(panel1);
            overlay.Add(fpsCounter);

            // we're done: BeginDesign() and EndDesign() are required for correct initialization
            overlay.EndDesign();

            return overlay;
        }

        public class SampleVM
        {
            public string Label { get; set; }
        }
    }
}
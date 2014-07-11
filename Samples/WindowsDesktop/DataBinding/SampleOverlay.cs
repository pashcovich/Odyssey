#region Using Directives

using Odyssey.Engine;
using Odyssey.UserInterface.Controls;
using Odyssey.UserInterface.Data;
using Odyssey.UserInterface.Style;
using SharpDX;
using System.Collections.Generic;

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

            var stackPanel = new StackPanel()
            {
                Width = 256,
                Height = 92,
                Position = new Vector2(8, 8)
            };

            DataTemplate commandTemplate = new DataTemplate()
            {
                DataType = typeof(StackPanel),
                Key = "CommandTemplate",
                VisualTree = new Button()
                {
                    Width = 64,
                    Height = 64,
                    Margin = new Thickness(4),
                    Name = "Button",
                    Content = new Label() { Name = "Label", TextDescriptionClass = "Small" }
                },
                Bindings = new Dictionary<string, Binding>
                {
                    {"Text", new Binding("CommandName", "Label")},
                }
            };

            stackPanel.DataTemplate = commandTemplate;
            stackPanel.ItemsSource = new SampleVM[]
            {
                new SampleVM() {CommandName = "AA"},
                new SampleVM() {CommandName = "BB"},
            };

            overlay.Add(stackPanel);
            overlay.EndDesign();

            return overlay;
        }

        public class SampleVM
        {
            public string CommandName { get; set; }
        }
    }
}
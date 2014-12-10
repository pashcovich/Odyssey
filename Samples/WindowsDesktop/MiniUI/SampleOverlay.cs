#region Using Directives

using Odyssey.Core;
using Odyssey.Engine;
using Odyssey.UserInterface;
using Odyssey.UserInterface.Controls;
using Odyssey.UserInterface.Data;
using Odyssey.UserInterface.Style;
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

            var canvas = new Canvas();
            var border = new Border() { StyleClass = "Panel", Position = LayoutManager.Point(2, 2), Width = LayoutManager.Units(30), Height = LayoutManager.Units(20) };
            
            var fpsCounter = new FpsCounter { Position = LayoutManager.Point(0.5f, 0.5f) };
            var dockPanel = new DockPanel { };

            var label = new TextBlock { Text = "This is a Dockpanel.", Margin = new Thickness(8)};
            label.DependencyProperties.Add(DockPanel.DockPropertyKey, Dock.Top);

            var stackPanel1 = new StackPanel() {Orientation = Orientation.Vertical}; 
            stackPanel1.DependencyProperties.Add(DockPanel.DockPropertyKey, Dock.Bottom);

            var listBox = new ListBox
            {
                Padding = new Thickness(8),
                Margin = new Thickness(8),
                ItemsSource = new[]
                {"This is a Listbox", "bound to a string array.", "Its itemtemplate specifies that each entry", "will be bound to a TextBlock"}
            };

            // The size of the button comes from the Defaul.oxil theme file
            var button = new Button
            {
                Content = new TextBlock { Text = "Button", HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center },
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(32)
            };
            button.Tap += (s, eventArgs) => ((TextBlock)button.Content).Text = "It works!";

            //stackPanel1.Add(button);
            button.DependencyProperties.Add(DockPanel.DockPropertyKey, Dock.Bottom);

            dockPanel.Add(button);
            dockPanel.Add(label);
            dockPanel.Add(listBox);
            border.Content = dockPanel;
            canvas.Add(border);
            canvas.Add(fpsCounter);

            overlay.Content = canvas;

            // we're done: BeginDesign() and EndDesign() are required for correct initialization
            overlay.EndDesign();

            return overlay;
        }
    }
}
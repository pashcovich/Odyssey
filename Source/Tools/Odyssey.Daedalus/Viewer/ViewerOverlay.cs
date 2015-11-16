using Odyssey.Core;
using Odyssey.UserInterface.Controls;

namespace Odyssey.Daedalus.Viewer
{
    public static class Viewer
    {
        public static Overlay CreateOverlay(IServiceRegistry services, DirectXViewer application)
        {
            Overlay overlay = new Overlay(services) { Width = 576, Height = 576};
            overlay.BeginDesign();

            Button bCaptureFrame = new Button() { Width = 64, Height = 64, Content = new TextBlock{Text = "D"} };
            Button bSwitchToCube = new Button() { Width = 64, Height = 64, Content = new TextBlock { Text = "C" } };

            //overlay.Add(bCaptureFrame);
            //overlay.Add(bSwitchToCube);

            overlay.EndDesign();

            return overlay;
        }
    }
}
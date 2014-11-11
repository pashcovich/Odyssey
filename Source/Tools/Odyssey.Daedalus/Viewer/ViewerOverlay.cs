using Odyssey.Core;
using Odyssey.UserInterface.Controls;
using Odyssey.UserInterface.Style;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Daedalus.Viewer
{
    public static class Viewer
    {
        public static Overlay CreateOverlay(IServiceRegistry services, DirectXViewer application)
        {
            Overlay overlay = new Overlay(services) { Width = 576, Height = 576};
            overlay.BeginDesign();

            Button bCaptureFrame = new Button() { Width = 64, Height = 64, Content = new Label{Text = "D"} };
            Button bSwitchToCube = new Button() { Width = 64, Height = 64, Content = new Label { Text = "C" } };

            overlay.Add(bCaptureFrame);
            overlay.Add(bSwitchToCube);

            overlay.EndDesign();

            return overlay;
        }
    }
}
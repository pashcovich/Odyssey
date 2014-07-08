using Odyssey.UserInterface.Controls;
using Odyssey.UserInterface.Style;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Tools.ShaderGenerator.Viewer
{
    public class ViewerOverlay : Overlay
    {
        internal DirectXViewer Viewer { get; private set; }

        private ViewerOverlay(IServiceRegistry services)
            : base(services)
        {
        }

        public static ViewerOverlay CreateOverlay(IServiceRegistry services, DirectXViewer application)
        {
            ViewerOverlay overlay = new ViewerOverlay(services) { Width = 576, Height = 576, Viewer = application };
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
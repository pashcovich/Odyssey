#region Using Directives

using Odyssey.Engine;
using Odyssey.Graphics.Shaders;
using Odyssey.Utilities.Logging;
using SharpDX.Diagnostics;
using System.Windows;

#endregion Using Directives

namespace Odyssey.Tools.ShaderGenerator.Viewer
{
    /// <summary>
    /// Interaction logic for DirectXWindow.xaml
    /// </summary>
    public partial class DirectXWindow : Window
    {
        private DirectXViewer dxViewer;
        private ShaderCollection shaderCollection;
        private string techniqueKey;

        public DirectXWindow()
        {
            InitializeComponent();
        }

        public void SetTechnique(ShaderCollection shaderCollection, string techniqueKey)
        {
            this.shaderCollection = shaderCollection;
            this.techniqueKey = techniqueKey;
        }

        public void Shutdown()
        {
            LogEvent.Tool.Info("Shutting down");
            Close();
            dxViewer.Exit();
            dxViewer.Dispose();
            LogEvent.Tool.Info(ObjectTracker.ReportActiveObjects());
        }

        public void Start()
        {
            Daedalus.DirectXWindow = this;
            dxViewer = new DirectXViewer();
            dxViewer.SetTechnique(shaderCollection, techniqueKey);
            dxViewer.Run(new DesktopWpfApplicationContext(ApplicationSurface)
            {
                RequestedWidth = 576,
                RequestedHeight = 576
            });
            Show();
        }
    }
}
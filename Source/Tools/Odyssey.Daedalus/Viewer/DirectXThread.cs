#region Using Directives

using Odyssey.Engine;
using System.Threading;
using System.Windows;

#endregion Using Directives

namespace Odyssey.Tools.ShaderGenerator.Viewer
{
    public class DirectXThread
    {
        private readonly DirectXViewer directXViewer;
        private readonly Thread thread;
        private bool done;

        public DirectXThread()
        {
            thread = new Thread(Run);
            thread.SetApartmentState(ApartmentState.STA);
            directXViewer = new DirectXViewer();
        }

        private void Run(object host)
        {
            using (directXViewer)
            {
                directXViewer.Run(new DesktopWpfApplicationContext(host));
            }
        }

        public void Start(UIElement element)
        {
            thread.Start(element);
        }

        public void Stop()
        {
            thread.Abort();
            directXViewer.Exit();
            directXViewer.Dispose();
        }
    }
}
using Odyssey.Engine;
using Odyssey.Platforms.Windows;
using SharpDX;
using SharpDX.DXGI;
using SharpDX.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Platforms.Windows
{
    public class FormTarget : SwapChainTargetBase, IOdysseyTarget
    {
        RenderForm form;

        public FormTarget(RenderForm form)
        {
            this.form = form;
            //form.SizeChanged += form_SizeChanged;
            ControlBounds = form.ClientRectangle;
        }

        void form_SizeChanged(object sender, EventArgs e)
        {
            UpdateForSizeChange();
        }

        protected override System.Drawing.Rectangle CurrentControlBounds
        {
            get { return form.Bounds; }
        }

        protected override int Width
        {
            get
            {
                return 0; // Returns 0 to fill the CoreWindow
            }
        }

        protected override int Height
        {
            get
            {
                return 0; // Returns 0 to fill the CoreWindow
            }
        }

        protected override SharpDX.DXGI.SwapChain1 CreateSwapChain(SharpDX.DXGI.Factory2 factory, SharpDX.Direct3D11.Device1 device, SharpDX.DXGI.SwapChainDescription1 desc)
        {
            // Creates a SwapChain from a CoreWindow pointer
            SwapChainFullScreenDescription scFullScreenDesc = new SwapChainFullScreenDescription()
            {
                Windowed = DeviceManager.Settings.IsWindowed,
                RefreshRate = new Rational(120, 1)
            };
                return factory.CreateSwapChainForHwnd(device, form.Handle, ref desc,
                    scFullScreenDesc, null);
        }


    }
}

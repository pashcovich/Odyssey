#region License

// Copyright © 2013-2014 Avengers UTD - Adalberto L. Simeone
//
// The Odyssey Engine is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License Version 3 as published by
// the Free Software Foundation.
//
// The Odyssey Engine is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details at http://gplv3.fsf.org/

#endregion License

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
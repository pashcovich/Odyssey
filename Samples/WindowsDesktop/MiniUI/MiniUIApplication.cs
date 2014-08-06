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

using Odyssey.Content;
using Odyssey.Engine;
using Odyssey.UserInterface;
using Odyssey.UserInterface.Controls;
using Odyssey.UserInterface.Style;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;
using System.Drawing;
using Color = SharpDX.Color;

#endregion Using Directives

namespace MiniUI
{
    public class MiniUIApplication : Component, IWindowService
    {
        private readonly SimpleDeviceManager d3dDeviceManager;
        private readonly RenderForm form;
        private readonly Overlay overlay;
        private readonly UserInterfaceManager uiManager;

        public MiniUIApplication()
        {
            var services = new ServiceRegistry();
            // Needs to be declared before the D3D device manager as it hooks to
            // the D3D device creation event
            var d2dDeviceManager = new Direct2DDeviceManager(services);
            d3dDeviceManager = ToDispose(new SimpleDeviceManager(services)
            {
                HorizontalDpi = 96.0f,
                VerticalDpi = 96.0f,
                PreferredBackBufferWidth = 1280,
                PreferredBackBufferHeight = 720,
                PreferredBackBufferFormat = Format.R8G8B8A8_UNorm,
                IsFullScreen = false,
                IsStereo = false,
                PreferredGraphicsProfile = new[] {FeatureLevel.Level_11_0,},
                PreferredMultiSampleCount = 1
            });
            form = new RenderForm("OdysseyUI Test")
            {
                ClientSize = new Size(d3dDeviceManager.PreferredBackBufferWidth, d3dDeviceManager.PreferredBackBufferHeight),
            };

            d3dDeviceManager.CreateDevice(form.Handle, DeviceCreationFlags.Debug | DeviceCreationFlags.BgraSupport);

            var content = new ContentManager(services);
            var styleManager = new StyleManager(services);
            uiManager = new DesktopUserInterfaceManager(services);

            services.AddService(typeof (IStyleService), styleManager);
            services.AddService(typeof (IUserInterfaceState), uiManager);
            services.AddService(typeof (IWindowService), this);
            services.AddService(typeof (IDirect2DService), d2dDeviceManager);

            content.LoadAssetList("Assets/Assets.yaml");

            overlay = ToDispose(SampleOverlay.New(services));
            uiManager.Initialize();
            uiManager.CurrentOverlay = overlay;
        }

        
        public object NativeWindow
        {
            get { return form; }
        }

        public void Run()
        {
            d3dDeviceManager.Context.OutputMerger.SetTargets(d3dDeviceManager.BackBuffer);
            RenderLoop.Run(form, () =>
            {
                d3dDeviceManager.Context.ClearRenderTargetView(d3dDeviceManager.BackBuffer, Color.Black);
                Render();
                d3dDeviceManager.SwapChain.Present(0, PresentFlags.None);
            });
        }

        private void Render()
        {
            uiManager.Update();
            overlay.Display();
        }
    }
}
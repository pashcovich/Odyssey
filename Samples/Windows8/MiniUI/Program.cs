using Odyssey;
using Odyssey.Engine;
using Odyssey.Platforms.Windows;
using Odyssey.UserInterface;
using Odyssey.UserInterface.Style;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniUI
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            // we need to load all control and text style definitions first
            StyleManager.LoadControlDescription("UI/Default.ocd");
            StyleManager.LoadTextDescription("UI/Default.otd");

            DeviceSettings settings = new DeviceSettings
            {
                AdapterOrdinal = 0,
                CreationFlags = DeviceCreationFlags.Debug,
                Dpi = 96.0f,
                ScreenWidth = 800,
                ScreenHeight = 600,
                SampleDescription = new SampleDescription(1, 0),
                Format = Format.R8G8B8A8_UNorm,
                IsStereo = false,
                IsWindowed = true,
            };
            RenderForm form = new RenderForm()
            {
                Text = string.Format("OdysseyUI demo - v{0}", Assembly.GetExecutingAssembly().GetName().Version),
                ClientSize = new System.Drawing.Size(settings.ScreenWidth, settings.ScreenHeight)
            };

            // DeviceManager are the Odyssey Engine's modified version of SharpDX's version defined
            // in the CommonDX library. You don't need to use thse. However in order to use the
            // Odyssey UI you will need to implement three
            // interfaces: IDirectXTarget, IDirect2DProvider and IDirect3DProvider
            // The latter is not strictly necessary but is required by IDirectXTarget and may be
            // reserved for future use. These interfaces just provide access to the Direct2/3D
            // device
            DeviceManager deviceManager = new DeviceManager();
            FormTarget target = new FormTarget(form);
            WindowsForms.SubscribeToMouseEvents(form);

            UIRenderer uiRenderer = new UIRenderer();
            deviceManager.Initialized += (s, e) => target.Initialize(e);
            deviceManager.Initialized += (s, e) => uiRenderer.Initialize(e);
            deviceManager.Initialize(settings);
            deviceManager.InitializeDeviceDependentResources();
            target.Render += (s, e) => uiRenderer.Render(e);

            RenderLoop.Run(form, () =>
                {

                        target.RenderAll();
                        target.Present();

                });
            uiRenderer.Dispose();
            target.Close();
            deviceManager.Dispose();
        }
    }
}
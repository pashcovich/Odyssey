﻿#region Using Directives

using GalaSoft.MvvmLight.Messaging;
using Odyssey.Engine;
using Odyssey.Graphics;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Models;
using Odyssey.Graphics.Shaders;
using Odyssey.Talos;
using Odyssey.Talos.Components;
using Odyssey.Talos.Systems;
using Odyssey.Tools.ShaderGenerator.ViewModel.Messages;
using Odyssey.UserInterface;
using SharpDX;
using SharpDX.Diagnostics;
using SharpDX.Direct3D11;
using System;

#endregion Using Directives

namespace Odyssey.Tools.ShaderGenerator.Viewer
{
    public class DirectXViewer : DesktopWpfApplication
    {
        private readonly Lazy<VSGraphicsDebugger> vsDebugger;
        private bool captureNextFrame;
        private VSGraphicsDebugger.CaptureToken captureToken;
        private ShaderComponent cShader;
        private Entity previewModel;
        private Scene scene;
        private ShaderCollection shaderCollection;
        private VertexShaderFlags vsFlags;

        public DirectXViewer()
        {
            DirectXDeviceManager directXDeviceManager = new DirectXDeviceManager(this);
            directXDeviceManager.DeviceCreationFlags |= DeviceCreationFlags.BgraSupport | DeviceCreationFlags.Debug;
            directXDeviceManager.PreferredBackBufferWidth = 576;
            directXDeviceManager.PreferredBackBufferHeight = 576;
            directXDeviceManager.PreferMultiSampling = true;
            directXDeviceManager.PreferredMultiSampleCount = 1;

            vsDebugger = new Lazy<VSGraphicsDebugger>(() => new VSGraphicsDebugger("Debug.vsglog"));
            Messenger.Default.Register<ViewerMessage>(this, ReceiveMessage);
        }

        public bool IsInited { get; private set; }

        public void SetTechnique(ShaderCollection shaderCollection, string techniqueKey)
        {
            this.shaderCollection = shaderCollection;
            Content.Store(shaderCollection.Name, shaderCollection);
            cShader = new ShaderComponent { AssetName = shaderCollection.Name, Key = techniqueKey };
            vsFlags = shaderCollection[techniqueKey].Key.VertexShader;
        }

        protected override bool BeginDraw()
        {
            if (captureNextFrame)
            {
                captureToken = vsDebugger.Value.BeginCapture();
            }
            return base.BeginDraw();
        }

        protected override void EndDraw()
        {
            base.EndDraw();
            if (captureNextFrame)
            {
                captureToken.Dispose();
                captureNextFrame = false;
            }
        }

        protected override void Initialize()
        {
            Content.LoadAssetList("Assets/Assets.yaml");
            DefineScene();
            SetScene(scene);
            Window.IsMouseVisible = true;
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            base.OnExiting(sender, args);
            scene.Unload();
        }

        private void DefineScene()
        {
            var device = Services.GetService<IOdysseyDeviceService>().DirectXDevice;
            scene = new Scene(Services);

            var cube = CubeMesh.New(device, modelOperations: ModelAnalyser.DetectModelRequirements(vsFlags));
            Content.Store(cube.Name, cube);

            var modelSystem = new ContentLoadingSystem<ModelComponent>();
            var shaderSystem = new ContentLoadingSystem<ShaderComponent>();
            var sInitialization = new InitializationSystem();
            var cameraSystem = new PerspectiveCameraSystem();
            var lightSystem = new PointLightSystem();
            var positionSystem = new PositionSystem();
            var sOptimization = new OptimizationSystem();
            var sRendering = new RenderSystem();
            var sUI = new UserInterfaceSystem();

            scene.BeginDesign();

            scene.AddSystem(modelSystem);
            scene.AddSystem(shaderSystem);
            scene.AddSystem(cameraSystem);
            scene.AddSystem(lightSystem);
            scene.AddSystem(sInitialization);
            scene.AddSystem(positionSystem);
            scene.AddSystem(sOptimization);
            scene.AddSystem(sRendering);
            scene.AddSystem(sUI);

            Entity root = scene.CreateEntity("Root");
            root.RegisterComponent(new TransformComponent());
            root.RegisterComponent(new PositionComponent());

            Entity camera = scene.CreateEntity("Camera");
            camera.RegisterComponent(new CameraComponent { Target = new Vector3(0, 0, 0) });
            camera.RegisterComponent(new PositionComponent { Position = new Vector3(0, 0f, 3f) });
            camera.RegisterComponent(new UpdateComponent { UpdateFrequency = UpdateFrequency.RealTime });

            Entity pointLight = scene.CreateEntity("PointLight");
            pointLight.RegisterComponent(new PointLightComponent { });
            pointLight.RegisterComponent(new PositionComponent { Position = new Vector3(0, 0f, 3f) });
            pointLight.RegisterComponent(new UpdateComponent { UpdateFrequency = UpdateFrequency.RealTime });
            pointLight.RegisterComponent(new ParentComponent() { Entity = root });
            pointLight.RegisterComponent(new TransformComponent());

            previewModel = scene.CreateEntity("PreviewModel");
            previewModel.RegisterComponent(new PositionComponent());
            previewModel.RegisterComponent(new TransformComponent());
            previewModel.RegisterComponent(new ModelComponent() { AssetName = cube.Name });
            previewModel.RegisterComponent(new UpdateComponent() { UpdateFrequency = UpdateFrequency.RealTime });
            previewModel.RegisterComponent(new MaterialComponent() { Diffuse = Color.Blue });
            previewModel.RegisterComponent(cShader);

            Entity ui = scene.CreateEntity("UI");
            ui.RegisterComponent(new UserInterfaceComponent { Overlay = ViewerOverlay.CreateOverlay(Services, this) });
            ui.RegisterComponent(new UpdateComponent() { UpdateFrequency = UpdateFrequency.RealTime });

            scene.EndDesign();
        }

        private void ReceiveMessage(ViewerMessage viewerMessage)
        {
            switch (viewerMessage.Action)
            {
                case ViewerAction.None:
                    break;

                case ViewerAction.CaptureFrame:
                    captureNextFrame = true;
                    break;

                default:
                    throw new ArgumentOutOfRangeException("viewerMessage");
            }
        }
    }
}
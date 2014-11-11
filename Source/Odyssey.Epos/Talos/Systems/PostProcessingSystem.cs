using System.Collections.Generic;
using System.Linq;
using Odyssey.Engine;
using Odyssey.Epos.Components;
using Odyssey.Epos.Initializers;
using Odyssey.Epos.Messages;
using Odyssey.Graphics.PostProcessing;
using Odyssey.Graphics.Shaders;
using Odyssey.Organization;
using Odyssey.Organization.Commands;

namespace Odyssey.Epos.Systems
{
    public class PostProcessingSystem : UpdateableSystemBase, IRenderableSystem
    {
        private readonly List<Command> sceneCommands;
        private bool process;


        public PostProcessingSystem() : base(Selector.All(typeof (PostProcessComponent)))
        {
            sceneCommands = new List<Command>();
        }

        public override void Start()
        {
            Messenger.Register<CommandUpdateMessage>(this);
            Messenger.Register<ResizeOutputMessage>(this);
        }

        public override void Stop()
        {
            Messenger.Unregister<CommandUpdateMessage>(this);
            Messenger.Unregister<ResizeOutputMessage>(this);
        }

        public bool BeginRender()
        {
            return true;
        }

        public void Render()
        {
            foreach (Entity entity in Entities)
            {
                if (!entity.IsEnabled)
                    continue;
                var cPostProcess = entity.GetComponent<PostProcessComponent>();
                cPostProcess.CommandManager.Run();
            }
        }

        public override void Unload()
        {
            foreach (Entity entity in Entities)
            {
                if (!entity.IsEnabled)
                    continue;
                var cPostProcess = entity.GetComponent<PostProcessComponent>();
                cPostProcess.CommandManager.Unload();
            }
        }

        protected override void HandleMessages()
        {
            if (MessageQueue.HasItems<CommandUpdateMessage>())
            {
                var mOptimization = MessageQueue.Dequeue<CommandUpdateMessage>();
                sceneCommands.AddRange(mOptimization.Commands);
                IsEnabled = true;
            }
            if (MessageQueue.HasItems<ResizeOutputMessage>())
            {
                var mResize = MessageQueue.Dequeue<ResizeOutputMessage>();
                ShaderInitializer.InitializerMap.Clear();
                foreach (var entity in Entities)
                {
                    var cPostProcess = entity.GetComponent<PostProcessComponent>();
                    cPostProcess.OutputWidth = mResize.Width;
                    cPostProcess.OutputHeight = mResize.Height;

                    // Reinitialize Shaders
                    foreach (var technique in cPostProcess.Techniques)
                        technique.ClearBuffers();

                    foreach (var technique in cPostProcess.Techniques)
                    {
                        Messenger.SendToSystem<InitializationSystem, ContentMessage<Technique>>(
                            new ContentMessage<Technique>(entity, cPostProcess.AssetName, technique), true);
                    }

                    SetupEntity(entity);
                }
            }
        }

        void SetupEntity(Entity entity)
        {
            var cPostProcess = entity.GetComponent<PostProcessComponent>();
            cPostProcess.CommandManager.Clear();
            var postProcessor = new PostProcessor(Services, cPostProcess.OutputWidth, cPostProcess.OutputHeight);
            var techniques = cPostProcess.Techniques.ToDictionary(t => t.Name, t => t);
            var actions = cPostProcess.Actions.ToArray();

            foreach (PostProcessAction ppAction in actions)
            {
                if (ppAction.AssetName == Param.Engine)
                {
                    switch (ppAction.Technique)
                    {
                        case Param.EngineActions.RenderSceneToTexture:
                            postProcessor.ProcessAction(ppAction, sceneCommands, cPostProcess.TagFilter);
                            break;
                    }
                }
                else
                {
                    var cModel = entity.GetComponent<ModelComponent>();
                    Technique effect = techniques[string.Format("{0}.{1}", ppAction.AssetName, ppAction.Technique)];
                    postProcessor.ProcessAction(ppAction, cModel.Model, effect, entity);
                }
            }

            var commandManager = cPostProcess.CommandManager;
            commandManager.AddLast(postProcessor.Result());
            if (cPostProcess.Target == TargetOutput.Backbuffer)
            {
                var lastCommand = commandManager.Last();
                commandManager.AddBefore(lastCommand, new ChangeTargetsCommand(Services) { Target = Services.GetService<IGraphicsDeviceService>().DirectXDevice });
            }
            commandManager.Initialize();

            var postProcessCommands = commandManager.OfType<IPostProcessCommand>().ToArray();
            for (int i = 1; i < postProcessCommands.Length; i++)
            {
                var ppAction = actions[i];
                postProcessCommands[i].SetInputs(ppAction.InputIndices.Select(idx => postProcessCommands[idx].Output));
            }
        }

        public override void Process(ITimeService time)
        {
            foreach (var entity in Entities)
                SetupEntity(entity);
        }

        public override void AfterUpdate()
        {
            base.AfterUpdate();
            IsEnabled = false;
        }
    }
}
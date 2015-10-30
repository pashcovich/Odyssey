using Odyssey.Engine;
using Odyssey.Epos.Messages;
using Odyssey.Organization.Commands;
using SharpDX.Mathematics;

namespace Odyssey.Epos.Systems
{
    public class RenderSystem : UpdateableSystemBase, IRenderableSystem
    {
        private readonly CommandManager commandManager;

        public RenderSystem() : base(Selector.None())
        {
            commandManager = new CommandManager();
        }

        public override void Start()
        {
            Messenger.Register<CommandUpdateMessage>(this);
        }

        public override void Stop()
        {
            Messenger.Unregister<CommandUpdateMessage>(this);
        }

        public bool BeginRender()
        {
            return true;
        }

        public void Render()
        {
            foreach (Command command in commandManager)
                command.Execute();
        }

        public override void Unload()
        {
            commandManager.Unload();
        }

        public override bool BeforeUpdate()
        {
            if (MessageQueue.HasItems<CommandUpdateMessage>())
            {
                var mUpdate = MessageQueue.Dequeue<CommandUpdateMessage>();

                switch (mUpdate.UpdateType)
                {
                    case UpdateType.Add:
                        commandManager.AddLast(mUpdate.Commands);
                        commandManager.Initialize();
                        break;
                }
            }
            return base.BeforeUpdate();
        }

        public override void Process(ITimeService time)
        {
        }
    }
}

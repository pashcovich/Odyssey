using System.Collections.Generic;
using Odyssey.Engine;
using Odyssey.Epos.Components;
using Odyssey.Epos.Messages;
using Odyssey.Graphics;
using Odyssey.Graphics.Effects;
using Odyssey.Organization;
using Odyssey.Organization.Commands;
using Odyssey.Text.Logging;

namespace Odyssey.Epos.Systems
{
    public class OptimizationSystem : UpdateableSystemBase
    {
        private readonly RenderMapper renderMapper;
        readonly ComponentType tShader;
        readonly ComponentType tModel;

        public OptimizationSystem() : base(Selector.All(typeof (ModelComponent), typeof(ShaderComponent)))
        {
            tShader = ComponentTypeManager.GetType<ShaderComponent>();
            tModel = ComponentTypeManager.GetType<ModelComponent>();
            renderMapper = new RenderMapper();
        }

        public override bool BeforeUpdate()
        {
            renderMapper.Clear();
            return base.BeforeUpdate();
        }

        public override void Process(ITimeService time)
        {
            foreach (var entity in Entities)
            {
                var cModel = entity.GetComponent<ModelComponent>(tModel.KeyPart);
                var cShader = entity.GetComponent<ShaderComponent>(tShader.KeyPart);
                
                renderMapper.AddEntity(cShader.Technique, cModel.Model, entity);
            }
            renderMapper.Group();
            CreateCommands();
        }

        void CreateCommands()
        {
            var commands = new LinkedList<Command>();
            foreach (var renderable in renderMapper)
            {
                var technique = renderable.Technique;
                var model = renderable.Model;
                RenderCommand renderCommand;
                if (technique.Mapping.Key.Supports(VertexShaderFlags.InstanceWorld))
                {
                    renderCommand = new InstancingRenderCommand(Services, technique, model, renderable.Entities);
                }
                else
                {
                    renderCommand = new TechniqueRenderCommand(Services, technique, model, renderable.Entities);
                }
                commands.AddLast(renderCommand);
            }

            var sv = new StateViewer(Services, commands);
            var resultCommands = sv.Analyze();

            if (resultCommands == null)
                LogEvent.Engine.Error("Nothing to render!");
            else
            {
                Messenger.Send(new CommandUpdateMessage(resultCommands));
            }
            IsEnabled = false;
        }
    }
}

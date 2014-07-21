using System.Collections.Generic;
using System.Linq;
using Odyssey.Engine;
using Odyssey.Graphics;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Models;
using Odyssey.Graphics.Organization.Commands;
using Odyssey.Graphics.Shaders;
using Odyssey.Talos.Components;
using Odyssey.Talos.Messages;
using Odyssey.Utilities.Logging;
using SharpYaml.Serialization;

namespace Odyssey.Talos.Systems
{
    [YamlTag("Optimization")]
    public class OptimizationSystem : UpdateableSystemBase, IUpdateableSystem
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

        public override void BeforeUpdate()
        {
            renderMapper.Clear();
        }

        public override void Process(ITimeService time)
        {
            foreach (IEntity entity in Entities.Where(e => e.IsEnabled))
            {
                var cModel = entity.GetComponent<ModelComponent>(tModel.KeyPart);
                var cShader = entity.GetComponent<ShaderComponent>(tShader.KeyPart);
                Effect effect = cShader.Technique.Effect;
                if (!renderMapper.ContainsKey(effect))
                    renderMapper.DefineNewEffect(effect);
                if (!renderMapper.ContainsModel(effect, cModel.Model))
                    renderMapper.AssociateModel(effect, cModel.Model);

                renderMapper.AddMesh(effect, cModel.Model, entity);
            }
        }

        public override void AfterUpdate()
        {
            CreateCommands(renderMapper.EffectModelsPairs);
        }

        void CreateCommands(IEnumerable<KeyValuePair<Effect, Dictionary<Model, List<IEntity>>>> effectModelsPairs)
        {
            LinkedList<Command> commands = new LinkedList<Command>();

            foreach (var pair in effectModelsPairs)
            {
                Effect effect = pair.Key;
                foreach (var effectModelPair in pair.Value)
                {
                    Model model = effectModelPair.Key;
                    RenderCommand renderCommand;
                    if (effect.TechniqueKey.Supports(VertexShaderFlags.InstanceWorld))
                    {
                        renderCommand = new InstancingRenderCommand(Services, effect, model.Meshes, effectModelPair.Value);
                    }
                    else
                    {
                        renderCommand = new EffectRenderCommand(Services, effect, model.Meshes, effectModelPair.Value);
                    }
                    commands.AddLast(renderCommand);
                }
            }
            

            StateViewer sv = new StateViewer(Services, commands);
            LinkedList<Command> resultCommands = sv.Analyze();

            if (resultCommands == null)
                LogEvent.Engine.Error("Nothing to render!");
            else
            {
                Messenger.Send(new OptimizationCompleteMessage(resultCommands));
            }
            IsEnabled = false;

        }
    }
}

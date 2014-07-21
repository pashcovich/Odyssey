using Odyssey.Engine;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Shaders;
using Odyssey.Talos.Components;
using Odyssey.Talos.Initializers;
using Odyssey.Talos.Messages;
using System;
using System.Linq;

namespace Odyssey.Talos.Systems
{
    public class InitializationSystem : UpdateableSystemBase
    {
        public InitializationSystem()
            : base(Selector.One(typeof(ShaderComponent), typeof(PostProcessComponent)))
        {
        }

        public override void BeforeUpdate()
        {
            // Before each frame we check if a new shader has been added to the scene.
            // If so, we need to initialize it.
            while (MessageQueue.HasItems<ContentLoadedMessage<ShaderComponent>>())
            {
                var mShader = MessageQueue.Dequeue<ContentLoadedMessage<ShaderComponent>>();
                SetupEntity(mShader.Content.Technique);
            }
            while (MessageQueue.HasItems<ContentLoadedMessage<PostProcessComponent>>())
            {
                var mPostProcess = MessageQueue.Dequeue<ContentLoadedMessage<PostProcessComponent>>();
                foreach (var technique in mPostProcess.Content.Techniques)
                    SetupEntity(technique);
            }
        }

        public override void Process(ITimeService time)
        {
            var data = (from entity in Entities
                        let techniqueComponents = entity.Components.OfType<ITechniqueComponent>()
                        from techniqueRange in techniqueComponents
                        from technique in techniqueRange.Techniques
                        select technique.Effect).Distinct();
            // Update each per frame constant buffer
            foreach (Effect effect in data)
            {
                effect.UpdateBuffers(UpdateType.InstanceStatic);
                effect.UpdateBuffers(UpdateType.SceneFrame);
                effect.UpdateBuffers(UpdateType.InstanceFrame);
            }
        }

        public override void Start()
        {
            Messenger.Register<ContentLoadedMessage<ShaderComponent>>(this);
            Messenger.Register<ContentLoadedMessage<PostProcessComponent>>(this);
        }

        public override void Stop()
        {
            Messenger.Unregister<ContentLoadedMessage<ShaderComponent>>(this);
            Messenger.Unregister<ContentLoadedMessage<PostProcessComponent>>(this);
        }

        private void SetupEntity(Technique technique)
        {
            Effect effect = technique.Effect;
            var shaderInitializer = new ShaderInitializer(Services, effect, technique.ActiveTechnique);
            shaderInitializer.Initialize();

            if (!technique.ActiveTechnique.Validate())
                throw new InvalidOperationException(string.Format("[{0}] was not properly initialized.", technique.Name));

            effect.AssembleBuffers();
        }
    }
}
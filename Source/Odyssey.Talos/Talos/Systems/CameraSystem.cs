using System.Linq;
using Odyssey.Engine;
using Odyssey.Talos.Components;
using Odyssey.Talos.Initializers;
using Odyssey.Talos.Messages;
using Odyssey.Talos.Nodes;
using System;
using System.Collections.Generic;

namespace Odyssey.Talos.Systems
{
    public abstract class CameraSystem<TCamera> : SystemBase, IUpdateableSystem
        where TCamera : CameraNode
    {
        protected ComponentType CameraComponentType { get; private set; }

        readonly CameraCollection cameraNodes;
        private CameraNode activeCamera;

        protected CameraSystem(Aspect aspect) : base(aspect)
        {
            CameraComponentType = ComponentTypeManager.GetType<CameraComponent>();
            cameraNodes = new CameraCollection();
        }

        public override void Start()
        {
            base.Start();
            Services.AddService(typeof(ICameraService), cameraNodes);
            Messenger.Register<ContentLoadedMessage<ShaderComponent>>(this);
            Messenger.Register<EntityChangeMessage>(this);
        }

        public override void Stop()
        {
            base.Stop();
            Messenger.Unregister<ContentLoadedMessage<ShaderComponent>>(this);
            Messenger.Unregister<EntityChangeMessage>(this);
        }

        protected void SetupEntity(IEntity entity)
        {
            TCamera nCamera = (TCamera)Activator.CreateInstance(typeof(TCamera), new object[] { entity });
            nCamera.Reset();
            cameraNodes.Add(nCamera.Id, nCamera);
            var cCamera = entity.GetComponent<CameraComponent>(CameraComponentType.KeyPart);
            if (!cCamera.IsInited)
                cCamera.Initialize();
            Messenger.Send(new CameraMessage(entity, nCamera, ChangeType.Added));
        }
        
        void RemoveEntity(IEntity entity)
        {
            ICamera camera = (from kvp in cameraNodes where kvp.Value.EntityId == entity.Id select kvp.Value).First();
            cameraNodes.Remove(camera.Id);
            Messenger.Send(new CameraMessage(entity, camera, ChangeType.Removed));
        }

        public void BeforeUpdate()
        {
            // Entity change
            while (MessageQueue.HasItems<EntityChangeMessage>())
            {
                EntityChangeMessage mEntity = MessageQueue.Dequeue<EntityChangeMessage>();
                if (mEntity.Action == ChangeType.Added)
                    SetupEntity(mEntity.Source);
                else if (mEntity.Action == ChangeType.Removed)
                    RemoveEntity(mEntity.Source);
            }

            // Todo improve camera system
            activeCamera = cameraNodes.Values.First();

            // Set up shader
            //while (MessageQueue.HasItems<ContentLoadedMessage<ShaderComponent>>())
            //{
            //    var mShader = MessageQueue.Dequeue<ContentLoadedMessage<ShaderComponent>>();
            //    ShaderInitializer shaderInitializer = new ShaderInitializer(Scene.Services, mShader.Content.Technique.Effect, mShader.Content.Technique.ActiveTechnique);
            //    shaderInitializer.InitializeCamera(activeCamera);
            //}

        }

        public void Process(ITimeService time)
        {
            foreach (IEntity entity in Entities)
            {
                var cameraNode = (from kvp in cameraNodes
                    where kvp.Value.EntityId == entity.Id
                    select kvp.Value).First();

                cameraNode.Update(time);
            }
        }

        public abstract void AfterUpdate();
    }
}

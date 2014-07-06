using Odyssey.Content;
using Odyssey.Engine;
using Odyssey.Talos;
using Odyssey.Talos.Components;
using Odyssey.Talos.Maps;
using Odyssey.Talos.Messages;
using Odyssey.Talos.Systems;
using Odyssey.Utilities.Logging;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using IComponent = Odyssey.Talos.IComponent;

namespace Odyssey
{
    [ContentReader(typeof(SceneReader))]
    public class Scene : IScene
    {
        private readonly List<IUpdateableSystem> updateableSystems;
        private readonly List<IRenderableSystem> renderableSystems;
        private readonly List<IUpdateableSystem> currentlyUpdatingSystems;
        private readonly List<IRenderableSystem> currentlyRenderingSystems;
        private readonly Messenger messenger;
        private readonly EntityMap entityMap;
        private readonly SystemMap systemMap;

        public bool IsDesignMode { get; private set; }

        public bool IsFirstUpdateDone { get; private set; }

        internal Messenger Messenger { get { return messenger; } }

        internal EntityMap EntityMap { get { return entityMap; } }

        internal SystemMap SystemMap { get { return systemMap; } }

        public IEnumerable<IEntity> Entities { get { return entityMap.Entities; } }

        public IEnumerable<ISystem> Systems { get { return systemMap.Systems; } }

        public IEnumerable<IComponent> Components { get { return entityMap.Components; } }

        public IServiceRegistry Services { get; private set; }

        public Scene(IServiceRegistry services)
        {
            Contract.Requires<ArgumentNullException>(services != null, "services");
            messenger = new Messenger();
            entityMap = new EntityMap(this);
            systemMap = new SystemMap(this);
            updateableSystems = new List<IUpdateableSystem>();
            renderableSystems = new List<IRenderableSystem>();
            currentlyUpdatingSystems = new List<IUpdateableSystem>();
            currentlyRenderingSystems = new List<IRenderableSystem>();
            Services = services;
            Services.AddService(typeof(IScene), this);
            var content = Services.GetService<IAssetProvider>();
            content.AddMapping("Scene", typeof(Scene));

            entityMap.EntityAdded += systemMap.OnEntityAdded;
            entityMap.EntityRemoved += systemMap.OnEntityRemoved;
            entityMap.EntityComponentAdded += systemMap.OnEntityComponentAdded;
            entityMap.EntityComponentRemoved += systemMap.OnEntityComponentRemoved;

            systemMap.SystemAdded += SystemMapOnSystemAdded;
            systemMap.SystemRemoved += SystemMapOnSystemRemoved;
        }

        public Scene(IEnumerable<ISystem> systems, IServiceRegistry services)
            : this(services)
        {
            foreach (ISystem system in systems)
                AddSystem(system);
        }

        private void SystemMapOnSystemAdded(object sender, SystemEventArgs args)
        {
            var system = args.Source;

            var updateableSystem = system as IUpdateableSystem;
            if (updateableSystem != null)
                updateableSystems.Add(updateableSystem);

            var renderableSystem = system as IRenderableSystem;
            if (renderableSystem != null)
                renderableSystems.Add(renderableSystem);
        }

        private void SystemMapOnSystemRemoved(object sender, SystemEventArgs args)
        {
            var system = args.Source;

            var updateableSystem = system as IUpdateableSystem;
            if (updateableSystem != null)
            {
                lock (updateableSystems)
                    updateableSystems.Remove(updateableSystem);
            }

            var renderableSystem = system as IRenderableSystem;
            if (renderableSystem != null)
            {
                lock (renderableSystems)
                    renderableSystems.Remove(renderableSystem);
            }
        }

        public void BeginDesign()
        {
            IsDesignMode = true;
        }

        public void EndDesign()
        {
            IsDesignMode = false;
            Validate();
            StartSystems();
        }

        private void StartSystems()
        {
            foreach (ISystem system in systemMap.Systems)
                system.Start();
        }

        private void Validate()
        {
            if (entityMap.Count == 0)
                throw new InvalidOperationException("No entities registered to scene");
            if (entityMap.CountEntities(e => e.ContainsComponent(ComponentTypeManager.GetKeyPart<CameraComponent>())) == 0)
                LogEvent.Engine.Warning("No camera registered to scene");
            if (entityMap.CountEntities(e => e.ContainsComponent(ComponentTypeManager.GetKeyPart<PointLightComponent>())) == 0)
                LogEvent.Engine.Warning("No light registered to scene");
            if (!entityMap.Validate())
                throw new InvalidOperationException("Entities are missing required components");
        }

        public void AddSystem(ISystem system)
        {
            systemMap.AddSystem(system);
        }

        public void RemoveSystem(ISystem system)
        {
            systemMap.RemoveSystem(system);
        }

        public IEntity SelectEntity(long id)
        {
            return entityMap.SelectEntity(id);
        }

        public bool ContainsEntity(IEntity entity)
        {
            return entityMap.ContainsEntity(entity);
        }

        public bool SystemHasEntities(ISystem system)
        {
            return systemMap.SystemHasEntities(system);
        }

        public void Update(ITimeService time)
        {
            lock (updateableSystems)
            {
                foreach (var updateable in updateableSystems)
                    currentlyUpdatingSystems.Add(updateable);
            }

            foreach (IUpdateableSystem system in currentlyUpdatingSystems)
            {
                system.BeforeUpdate();

                if (!system.IsEnabled)
                    continue;

                system.Process(time);
                system.AfterUpdate();
            }

            currentlyUpdatingSystems.Clear();
            IsFirstUpdateDone = true;
        }

        public void Render(ITimeService time)
        {
            lock (renderableSystems)
            {
                foreach (var renderable in renderableSystems)
                    currentlyRenderingSystems.Add(renderable);
            }

            foreach (IRenderableSystem system in currentlyRenderingSystems)
            {
                if (!system.IsEnabled)
                    continue;

                if (system.BeginRender())
                {
                    system.Render(time);
                    //system.EndRender();
                }
            }

            currentlyRenderingSystems.Clear();
        }

        public void SendMessage<TMessage>(TMessage message) where TMessage : IMessage
        {
            Messenger.Send(message);
        }

        public Entity CreateEntity(string name)
        {
            Entity entity = new Entity(name);
            AddEntity(entity);
            return entity;
        }

        public void AddEntity(Entity entity)
        {
            entity.AssignToScene(this);
            entityMap.AddEntity(entity);
        }

        public void RemoveEntity(Entity entity)
        {
            entityMap.RemoveEntity(entity);
        }

        public void AddComponentToEntity(IComponent component, IEntity entity)
        {
            entityMap.AddComponentToEntity(component, entity);
        }

        public void RemoveComponentFromEntity(IComponent component, IEntity entity)
        {
            entityMap.RemoveComponentFromEntity(component, entity);
        }

        public IEnumerable<IComponent> SelectEntityComponents(IEntity entity)
        {
            return entityMap.GetEntityComponents(entity);
        }

        public IEnumerable<TComponent> SelectComponents<TComponent>()
            where TComponent : IComponent
        {
            return entityMap.SelectComponents<TComponent>();
        }

        public TComponent GetEntityComponent<TComponent>(IEntity entity, long keyPart)
            where TComponent : IComponent
        {
            return entityMap.GetEntityComponent<TComponent>(entity, keyPart);
        }

        public bool TryGetEntityComponent<TComponent>(IEntity entity, long keyPart, out TComponent component)
            where TComponent : IComponent
        {
            return entityMap.TryGetEntityComponent(entity, keyPart, out component);
        }

        public bool EntityHasComponent(IEntity entity, long keyPart)
        {
            return entityMap.EntityHasComponent(entity, keyPart);
        }

        public void Unload()
        {
            entityMap.Unload();
            systemMap.Unload();
        }
    }
}
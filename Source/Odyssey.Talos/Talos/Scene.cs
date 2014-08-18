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

#endregion

#region Using Directives

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Odyssey.Content;
using Odyssey.Engine;
using Odyssey.Talos.Components;
using Odyssey.Talos.Maps;
using Odyssey.Talos.Messages;
using Odyssey.Talos.Systems;
using Odyssey.Utilities.Logging;
using SharpDX;

#endregion

namespace Odyssey.Talos
{
    [ContentReader(typeof (SceneReader))]
    public class Scene : IScene
    {
        private readonly List<IRenderableSystem> currentlyRenderingSystems;
        private readonly List<IUpdateableSystem> currentlyUpdatingSystems;
        private readonly EntityMap entityMap;
        private readonly Messenger messenger;
        private readonly List<IRenderableSystem> renderableSystems;
        private readonly SystemMap systemMap;
        private readonly List<IUpdateableSystem> updateableSystems;

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
            Services.AddService(typeof (IScene), this);
            var content = Services.GetService<IAssetProvider>();
            content.AddMapping("Scene", typeof (Scene));

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

        public bool IsDesignMode { get; private set; }

        public IEnumerable<ISystem> Systems
        {
            get { return systemMap.Systems; }
        }

        internal Messenger Messenger
        {
            get { return messenger; }
        }

        internal SystemMap SystemMap
        {
            get { return systemMap; }
        }

        public IEnumerable<IComponent> Components
        {
            get { return entityMap.Components; }
        }

        public IEnumerable<IEntity> Entities
        {
            get { return entityMap.Entities; }
        }

        public bool IsFirstUpdateDone { get; private set; }

        public IServiceRegistry Services { get; private set; }

        public void BeginDesign()
        {
            IsDesignMode = true;
        }

        public bool ContainsEntity(IEntity entity)
        {
            return entityMap.ContainsEntity(entity);
        }

        public void EndDesign()
        {
            IsDesignMode = false;
            Validate();
            StartSystems();
        }

        public bool EntityHasComponent(IEntity entity, long keyPart)
        {
            return entityMap.EntityHasComponent(entity, keyPart);
        }

        public TComponent GetEntityComponent<TComponent>(IEntity entity, long keyPart)
            where TComponent : IComponent
        {
            return entityMap.GetEntityComponent<TComponent>(entity, keyPart);
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

        public IEntity SelectEntity(long id)
        {
            return entityMap.SelectEntity(id);
        }

        public IEnumerable<IComponent> SelectEntityComponents(IEntity entity)
        {
            return entityMap.GetEntityComponents(entity);
        }

        public void SendMessage<TMessage>(TMessage message) where TMessage : Message
        {
            Messenger.Send(message);
        }

        public bool TryGetEntityComponent<TComponent>(IEntity entity, long keyPart, out TComponent component)
            where TComponent : IComponent
        {
            return entityMap.TryGetEntityComponent(entity, keyPart, out component);
        }

        public void Unload()
        {
            entityMap.Unload();
            systemMap.Unload();
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

        public void AddComponentToEntity(IComponent component, IEntity entity)
        {
            entityMap.AddComponentToEntity(component, entity);
        }

        public void AddEntity(Entity entity)
        {
            entity.AssignToScene(this);
            entityMap.AddEntity(entity);
        }

        public void AddSystem(ISystem system)
        {
            systemMap.AddSystem(system);
        }

        public Entity CreateEntity(string name)
        {
            Entity entity = new Entity(name);
            AddEntity(entity);
            return entity;
        }

        public IEnumerable<IEntity> GetChildren(IEntity parent)
        {
            return entityMap.SelectChildren(parent);
        }

        public IEntity FindChild(IEntity parent, string name)
        {
            return entityMap.FindChild(parent, name);
        }

        public void RemoveComponentFromEntity(IComponent component, IEntity entity)
        {
            entityMap.RemoveComponentFromEntity(component, entity);
        }

        public void RemoveEntity(Entity entity)
        {
            entityMap.RemoveEntity(entity);
        }

        public void RemoveSystem(ISystem system)
        {
            systemMap.RemoveSystem(system);
        }

        public IEnumerable<TComponent> SelectComponents<TComponent>()
            where TComponent : IComponent
        {
            return entityMap.SelectComponents<TComponent>();
        }

        public bool SystemHasEntities(ISystem system)
        {
            return systemMap.SystemHasEntities(system);
        }

        private void StartSystems()
        {
            foreach (ISystem system in systemMap.Systems)
                system.Start();
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
    }
}
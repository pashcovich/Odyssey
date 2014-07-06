using Odyssey.Graphics;
using Odyssey.Talos.Components;
using Odyssey.Talos.Messages;
using Odyssey.Utilities.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Odyssey.Talos.Maps
{
    public class EntityMap
    {
        readonly Scene scene;
        readonly Dictionary<long, Dictionary<long, IComponent>> componentsByEntity;
        readonly Dictionary<long, IEntity> entities;
        private readonly Dictionary<long, IComponent> components; 

        public event EventHandler<EntityEventArgs> EntityAdded;
        public event EventHandler<EntityEventArgs> EntityRemoved;
        public event EventHandler<EntityChangedEventArgs> EntityComponentAdded;
        public event EventHandler<EntityChangedEventArgs> EntityComponentRemoved;

        protected Messenger Messenger { get { return scene.Messenger; } }
        public IEnumerable<IEntity> Entities { get { return entities.Values; } }
        public IEnumerable<IComponent> Components { get { return components.Values; } } 

        public EntityMap(Scene scene)
        {
            this.scene = scene;
            entities = new Dictionary<long, IEntity>();
            components = new Dictionary<long, IComponent>();
            componentsByEntity = new Dictionary<long, Dictionary<long, IComponent>>();
        }

        protected virtual void OnEntityAdded(EntityEventArgs args)
        {
            var handler = EntityAdded;
            if (handler != null)
                handler(this, args);
        }

        protected virtual void OnEntityRemoved(EntityEventArgs args)
        {
            var handler = EntityRemoved;
            if (handler != null)
                handler(this, args);
        }

        protected virtual void OnEntityComponentRemoved(EntityChangedEventArgs args)
        {
            var handler = EntityComponentRemoved;
            if (handler != null)
                handler(this, args);
        }

        protected virtual void OnEntityComponentAdded(EntityChangedEventArgs args)
        {
            var handler = EntityComponentAdded;
            if (handler != null)
                handler(this, args);
        }

        public void AddEntity(IEntity entity)
        {
            if (entity.Scene != null && entity.Scene != scene)
            {
                RemoveEntity(entity);
                LogEvent.Engine.Warning("Entity was already registered in another scene.");
            }
            componentsByEntity.Add(entity.Id, new Dictionary<long, IComponent>());
            entities.Add(entity.Id, entity);
            OnEntityAdded(new EntityEventArgs(entity));
        }

        public void RemoveEntity(IEntity entity)
        {
            if (entity.Scene != null && entity.Scene != scene)
            {
                RemoveEntity(entity);
                LogEvent.Engine.Warning("Entity was already registered in another scene.");
            }
            componentsByEntity.Remove(entity.Id);
            entities.Remove(entity.Id);
            OnEntityRemoved(new EntityEventArgs(entity));
        }

        public IEntity SelectEntity(long id)
        {
            return entities[id];
        }

        public int Count
        {
            get { return entities.Count; }
        }

        public bool ContainsEntity(IEntity entity)
        {
            return entities.ContainsKey(entity.Id);
        }

        public IEntity GetEntity(long entityId)
        {
            return entities[entityId];
        }

        public void Unload()
        {
            foreach (IInitializable initializableComponent in components.Values.OfType<IInitializable>())
                initializableComponent.Unload();
        }

        public bool Validate()
        {
            bool test = true;
            foreach (IEntity entity in entities.Values)
            {
                bool result = entity.Validate();
                if (!result)
                    test = false;
            }
            return test;
        }

        internal IEnumerable<TComponent> SelectComponents<TComponent>()
            where TComponent : IComponent
        {
            return components.Values.OfType<TComponent>();
        }

        internal IEnumerable<IComponent> GetEntityComponents(IEntity entity)
        {
            return componentsByEntity[entity.Id].Values;
        }

        internal TComponent GetEntityComponent<TComponent>(IEntity entity, long keyPart)
            where TComponent : IComponent
        {
            return (TComponent)componentsByEntity[entity.Id][keyPart];
        }

        internal bool TryGetEntityComponent<TComponent>(IEntity entity, long keyPart, out TComponent component)
            where TComponent : IComponent
        {
            IComponent componentTemp;
            bool test = componentsByEntity[entity.Id].TryGetValue(keyPart, out componentTemp);
            component = (TComponent) componentTemp;
            return test;
        }

        internal int CountEntities(Func<IEntity, bool> function)
        {
            return entities.Count(kvp => function(kvp.Value));
        }

        internal bool EntityHasComponent(IEntity entity, long keyPart)
        {
            return componentsByEntity[entity.Id].ContainsKey(keyPart);
        }

        internal void AddComponentToEntity(IComponent component, IEntity entity)
        {
            componentsByEntity[entity.Id].Add(component.KeyPart, component);
            if (!components.ContainsKey(component.Id))
                components.Add(component.Id, component);
            OnEntityComponentAdded(new EntityChangedEventArgs(entity, component));
        }

        internal void RemoveComponentFromEntity(IComponent component, IEntity entity)
        {
            componentsByEntity[entity.Id].Remove(component.KeyPart);
            components.Remove(component.Id);
            OnEntityComponentRemoved(new EntityChangedEventArgs(entity, component));
        }

    }
}
